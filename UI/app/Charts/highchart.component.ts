﻿import { Component, OnInit, Input } from '@angular/core';
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
    template: '<chart  [options]="options"></chart>'
})

export class HighChartComponent implements OnInit {

    options: HighchartsChartOptions;

    //this component takes in parameters from outside
    @Input() chart: Chartdomain.ChartModel;

    constructor() {
        //this.options = this.getBarAndLineChartOptions(this.chart);
    }

    getChartValues(chart: Chartdomain.ChartModel): any {

        let seriesList = Enumerable.asEnumerable(chart.series);



        return seriesList.Select((s: Chartdomain.ChartSeriesModel, i: number) => {
            return {
                name: s.key,
                type: (i == 0) ?  'spline' : 'column',
                yAxis: (i == 0) ? 0 : 1,
                data: s.data.map((dp) => dp.y)
            };
        })
            .OrderByDescending((s) => s.yAxis)
            .ToArray();

    }

    getChartXAxis(chart: Chartdomain.ChartModel): any {
        let seriesList = Enumerable.asEnumerable(chart.series);
        let recenciesList = Enumerable.asEnumerable(chart.recencies);
        var x = seriesList.SelectMany((s) => s.data)
            .Select((d: Chartdomain.DataPointModel) => recenciesList.FirstOrDefault(r => r.RecencyNumber === d.x).Lable)
            .Distinct()
            .ToArray();

        // console.log(JSON.stringify(x));
        return x;
    }

    private getBarAndLineChartOptions(chart: Chartdomain.ChartModel) {

        return {
            chart: {
                // renderTo: 'container',
                zoomType: 'xy'
            },
            title: {
                text: ''
            },
            //subtitle: {
            //    text: 'Source: WorldClimate.com'
            //},
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
                shared: true
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
                }
            },
            series: this.getChartValues(chart)
        };
    }

    ngOnInit() {

        // setTimeout(() =>
        this.options = this.getBarAndLineChartOptions(this.chart);
        //    3000
        // );

        //setTimeout(() => {
        //    angular2 - highcharts

        //},2000)

        // console.log(JSON.stringify(this.getBarAndLineChartOptions(this.chart)));
        // this.options = {
        //     title: { text: 'chart selection event example' },
        //     chart: { zoomType: 'x' },
        //     series: [{ data: [29.9, 71.5, 106.4, 129.2, 45, 13, 120] }]
        //};


        //this.options = {
        //    chart: {
        //        zoomType: 'xy'
        //    },
        //    title: {
        //        text: 'Average Monthly Temperature and Rainfall in Tokyo'
        //    },
        //    subtitle: {
        //        text: 'Source: WorldClimate.com'
        //    },

        //    //xAxis: [
        //    //    {
        //    //        categories: [
        //    //            'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
        //    //            'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
        //    //        ],
        //    //        crosshair: true
        //    //    }
        //    //],
        //    yAxis: [
        //        { // Primary yAxis
        //            labels: {
        //                format: '{value}°C',
        //                //style: {
        //                //    color: Highcharts.getOptions().colors[1]
        //                //}
        //            },
        //            title: {
        //                text: 'Temperature',
        //                //style: {
        //                //    color: Highcharts.getOptions().colors[1]
        //                //}
        //            }
        //        }, { // Secondary yAxis
        //            title: {
        //                text: 'Rainfall',
        //                //style: {
        //                //    color: Highcharts.getOptions().colors[0]
        //                //}
        //            },
        //            labels: {
        //                format: '{value} mm',
        //                //style: {
        //                //    color: Highcharts.getOptions().colors[0]
        //                //}
        //            },
        //            opposite: true
        //        }
        //    ],
        //    tooltip: {
        //        shared: true
        //    },
        //    legend: {
        //        layout: 'vertical',
        //        align: 'left',
        //        x: 120,
        //        verticalAlign: 'top',
        //        y: 100,
        //        floating: true,
        //        backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        //    },
        //    plotOptions: {
        //        column: {
        //            stacking: 'normal'
        //        }
        //    },
        //    series: [
        //        {
        //            name: 'Rainfall',
        //            type: 'column',
        //            yAxis: 0,
        //            data: [30, 7, 10, 29.2, 44.0, 17, 166, 48, 214, 94.1, 96, 42],


        //        }
        //        ,
        //        {
        //            name: 'Rainfall 2',
        //            type: 'column',
        //            yAxis: 1,
        //            data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]

        //        },
        //         //    {
        //        //    name: 'Temperature',
        //        //    type: 'spline',
        //        //    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]


        //        //}
        //    ]
        //};
    }

    errorMessage: any;
}