import { Component, OnInit, Input } from '@angular/core';
import { ChartValueService } from './services/chart.services';
import { CHART_DIRECTIVES, Highcharts} from 'angular2-highcharts';
import * as Chartdomain from "./domain/chart.domain";
import * as Enumerable from "linq-es2015";

declare let jQuery: any;

@Component({
    selector: 'highchart_chart',
    directives: [CHART_DIRECTIVES],
    providers: [ChartValueService],
    styles: [`
      chart {
        display: block;      
      }
    `],
    template: '<chart  [options]="options" (load)="saveInstance($event.context)" ></chart>'
})

export class HighChartComponent implements OnInit {

    options: HighchartsChartOptions;
    chartObject: any;

    //this component takes in parameters from outside
    @Input() chart: Chartdomain.ChartModel;
    @Input() renderIn: string;
   

    constructor() {
    
    }

    public reflowIfRenderedInModal(width:number,height:number) {
        if (this.renderIn === 'modal') {
           
            this.chartObject.setSize(width, height);
            //this.chartObject.redraw();
            //this.chartObject.reflow();
        }
    }

    saveInstance(chartInstance: any) {
        this.chartObject = chartInstance;
    }

    getBarAndLineChartValues(chart: Chartdomain.ChartModel): any {

        let seriesList = Enumerable.asEnumerable(chart.series);

        return seriesList.Select((s: Chartdomain.ChartSeriesModel, i: number) => {
            return {
                name: s.key,
                type:  ((i == 0) ?  'spline' : 'column') ,
                yAxis:  ((i == 0) ? 0 : 1 ),
                data: s.data.map((dp) => { return { y: dp.y, n: dp.samples } })
            };
        })
            .OrderByDescending((s) => s.yAxis)
            .ToArray();

    }

    getLineChartValues(chart: Chartdomain.ChartModel): any {

        let seriesList = Enumerable.asEnumerable(chart.series);

        return seriesList.Select((s: Chartdomain.ChartSeriesModel, i: number) => {
            return {
                name: s.key,
                data: s.data.map((dp) => { return { y: dp.y, n : dp.samples } })
            };
        }).ToArray();

    }

    getChartXAxis(chart: Chartdomain.ChartModel): any {
        let seriesList = Enumerable.asEnumerable(chart.series);
        let recenciesList = Enumerable.asEnumerable(chart.recencies);
        var x = seriesList.SelectMany((s) => s.data)
            .Select((d: Chartdomain.DataPointModel) => recenciesList.FirstOrDefault(r => r.RecencyNumber === d.x).Lable)
            .Distinct()
            .ToArray();
        return x;
    }

    private getBarAndLineChartOptions(chart: Chartdomain.ChartModel) {

        return {
            chart: {
                // renderTo: 'container',
                zoomType: 'xy',
              
            },
            title: {
                text: ''
            },
          
            xAxis: [{
                categories: this.getChartXAxis(chart),
                crosshair: true
            }],
            yAxis: [

                { // Primary yAxis
                    labels: {
                        // format: '{value}°C',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    title: {
                        text: 'Mean', //todo
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    opposite: true,
                    min: 0,
                    max: 10

                },

                { // Secondary yAxis
                title: {
                    text: ' % intention to return', //todo
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    formatter: function () {
                        return  `${this.axis.defaultLabelFormatter.call(this)}%`;
                    },
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                
                min: 0,
                max: 100

                }],

            tooltip: {
                pointFormat: "{point.y:.2f}%",
                formatter: function () {
                    let format =
                        `<table style='min-width= 200px;' > 
                            <thead>
                            <tr>                               
                                <td style='color: ${this.series.color} ;' > <div style='min-width :150px;  font-size:13px;'> <strong>${this.series.name}</strong></div></td>
                            </tr>
                            </thead> 
                            <tbody> 
                            <tr>
                                <td style='font-size:11px;'  > ${this.x} <strong>(n=${this.point.n})</strong></td>                           
                            </tr>
                            <tr>
                                <td style='font-size:11px;' ><strong> ${this.y.toFixed(2)}%</strong> </td>                           
                            </tr>
                            </tbody>
                            </table>`;
                    return format;
                },
                useHTML: true
               
            },
            legend: {
                // layout: 'vertical',
                // align: 'middle',
                //// x: 120,
                // verticalAlign: 'top',
                // y: 100,
                // floating: true,
                // backgroundColor:  '#FFFFFF'
            },
            plotOptions: {
                column: {
                    stacking: 'normal'
                },
                 series: {
                    animation: this.renderIn !== 'printer'
                }
            },
            series: this.getBarAndLineChartValues(chart)
        };
    }

    private getLineChartOptions(chart: Chartdomain.ChartModel) {

     
        return {
            chart: {
                zoomType: 'xy',
               
            },

            plotOptions: {
                series: {
                    animation: this.renderIn !== 'printer' 
                }
            },

            title: {
                text: ''
            },
          
            xAxis: {
                categories: this.getChartXAxis(chart),
                crosshair: true
            },
            yAxis: { 
                    labels: {
                       
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    title: {
                        text: '', //todo
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    min: 0,
                    max: this.chart.dataAnlysisType === 'percentage' ? 100 : 10
                },



            tooltip: {
                pointFormat: "{point.y:.2f}%",
                formatter: function () {
                    let format =
                        `<table style='min-width= 200px;' > 
                            <thead>
                            <tr>                               
                                <td style='color: ${this.series.color} ;' > <div style='min-width :150px;  font-size:13px;'> <strong>${this.series.name}</strong></div></td>
                            </tr>
                            </thead> 
                            <tbody> 
                            <tr>
                                <td style='font-size:11px;'  > ${this.x} <strong>(n=${this.point.n})</strong></td>                           
                            </tr>
                            <tr>
                                <td style='font-size:11px;' ><strong> ${this.y.toFixed(2)}%</strong> </td>                           
                            </tr>
                            </tbody>
                            </table>`;
                    return format;
                },
                useHTML: true
               
            },
            series: this.getLineChartValues(chart)
        };
    }


    ngOnInit() {

        switch (this.chart.chartRenderType) {
            case "lineAndBar": 
                this.options = this.getBarAndLineChartOptions(this.chart);
                break;
            default:
                this.options = this.getLineChartOptions(this.chart);
        }
    }

    errorMessage: any;
}