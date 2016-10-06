import {Component, OnInit, Input} from '@angular/core';
import {ChartValueService} from './services/chart.services'
import {nvD3} from 'ng2-nvd3';
import * as Chartdomain from "./domain/chart.domain";

declare let d3: any;

@Component({
    selector: 'd3chart',
    directives: [nvD3],
    providers: [ChartValueService],
    templateUrl: 'app/charts/templates/ChartComponent.html'
})

export class D3Chart implements OnInit {

    options: any;
    data: any;
   // private service: ChartValueService;

    //this component takes in parameters from outside
    @Input() chart: Chartdomain.ChartModel;

    constructor(service: ChartValueService) {
    //    this.service = service;
    }

    getBarChartValues(chart: Chartdomain.ChartModel): any {
        return chart.series.map((s) => {
            return {
                key: s.key,
                values: s.data.map((dp) => {
                    return {
                        value: dp.y,
                        label: dp.label
                    };
                })
            };
        });
    }

    getLineChartValues(chart: Chartdomain.ChartModel): any {

       
        return chart.series.map((s) => {
            return {
                key: s.key,
                values:
                //[[10, 30], [20, null], [30, 15], [40, 55], [60, null], [70, 25], [80, 40]]

                s.data.map((dp, j) => {
                 
                    return {x:dp.x, y:dp.y, i:j};
                })
            };
        });
    }
    
    getChartValuesForLineAndBar(chart: Chartdomain.ChartModel): any {
        return chart.series.map((s,i) => {
            return {
                key: s.key,
                bar: (i !== 0),
                values: s.data.map((dp) => {
                    return { x: dp.x, y: dp.y };
                })
            };
        });
    }

    private getLineChartOptions() {
      //  alert(this.chart.dataAnlysisType);
        return {
            chart: {
                forceY: [0, this.chart.dataAnlysisType === 'percentage' ? 100 : 10],

                type: 'lineChart',
                height: 500,
                margin: {
                    top: 20,
                    right: 50,
                    bottom: 70,
                    left: 70
                },
                x: (d: any) => { return d.x; },
                y: (d: any) => { return d.y; },

                showValues: true,
                // staggerLabels: true,
                valueFormat: function(d: any) {
                    return d3.format(",.1f")(d);
                },
                duration: 500,
                xAxis: {
                    //axisLabel: 'X Axis',

                    tickFormat: (d: any) => {

                       // debugger;
                        let xAx = this.chart.recencies.filter(r => r.RecencyNumber === d)[0];
                        if (xAx == null) {
                            return '';
                        } else {
                            return xAx.Lable;
                        }
                    }
                    ////////tickFormat: (d: any) => {
                    ////////    debugger;
                    ////////    return d3.time.format('%x')(new Date(d));
                    ////////}

                    //tickValues:  (values: any)=> {
                       
                    //    var x = this.chart.recencies.map(r => r.Lable);

                        
                    //   // debugger;

                    //    return x;
                    //    //  return values.map((v:any) => v.i);
                    //    //var a = _.map(values[0].values, function (v, i) {
                    //    //    return i
                    //    //});
                    //    //return a;
                    //}


                },
                yAxis: {
                 //   ticks: [10, 20, 70, 100],
                    axisLabel: 'Answers'
                    tickFormat: (d: any) =>
                    {
                        return this.chart.dataAnlysisType === 'percentage' ? `${d}%` : d;
                    }
                    //{
                    //    return d3.format('.02f')(d);
                    //}
                }
               
               //  rotateLabels: 0 //Angle to rotate x-axis labels.
               // showControls: true, //Allow user to switch between 'Grouped' and 'Stacked' mode.
               // groupSpacing: 0.1 //Distance between each group of bars.
            }
        };
    }

    private getBarChartOptions() {
        return {
            chart: {
                type: 'multiBarChart',
                height: 500,
                margin: {
                    top: 20,
                    right: 20,
                    bottom: 70,
                    left: 55
                },
                x: function (d: any) { return d.label; },
                y: function (d: any) { return d.value; },
                showValues: true,
                staggerLabels: true,
                valueFormat: function (d: any) {
                    return d3.format(",.4f")(d);
                },
                duration: 500,
                xAxis: {
                    axisLabel: 'X Axis'
                },
                yAxis: {
                    axisLabel: 'Y Axis',
                    axisLabelDistance: -10
                },
                rotateLabels: 0,     //Angle to rotate x-axis labels.
                showControls: true,   //Allow user to switch between 'Grouped' and 'Stacked' mode.
                groupSpacing: 0.1   //Distance between each group of bars.
            }
        }
    }

    private getBarAndLineChartOptions() {
      
        return {
            chart: {
                type: 'linePlusBarChart',
                height: 500,
                margin: {
                    top: 20,
                    right: 50,
                    bottom: 70,
                    left: 70
                },
                //x: function (d: any, i: any) {
                //        let xAx = this.chart.recencies.filter( (r:any)=> r.RecencyNumber === d)[0];
                //        if (xAx == null) {
                //            return '';
                //        } else {
                //            return xAx.Lable;
                //        }
                //     },
                // y: function (d: any) { return d[1]; },
                // showValues: true,
                // staggerLabels: true,
                // valueFormat: function (d: any) {
                //    return d3.format(",.4f")(d);
                //},
                duration: 500,
                xAxis: {
                    axisLabel: 'X Axis',
                    tickFormat: (d: any) => {
                        let xAx = this.chart.recencies.filter((r:any) => r.RecencyNumber === d)[0];
                        if (xAx == null) {
                            return '';
                        } else {
                            return xAx.Lable;
                        }
                    }

                    //tickFormat: (d: any) => {
                    //    let xAx = this.chart.recencies.filter(r => r.RecencyNumber === d)[0];
                    //    if (xAx == null) {
                    //        return '';
                    //    } else {
                    //        return xAx.Lable;
                    //    }
                    //}
                },
                yAxis: {
                    axisLabel: 'Y Axis'
                    //  axisLabelDistance: -10
                },
                focusEnable: false,
                rotateLabels: 0, //Angle to rotate x-axis labels.
              
            }
        };
    }
    
    ngOnInit() {

        //todo
        switch (this.chart.chartRenderType) {
            case "line":
                this.options = this.getLineChartOptions();
                this.data = this.getLineChartValues(this.chart);
                break;
            case "lineAndBar":
                this.options = this.getBarAndLineChartOptions();
                this.data = this.getChartValuesForLineAndBar(this.chart);
                debugger;
                break;

            
        default:
        }

        //this.options = this.getLineChartOptions();
        
       

        // return this.getBarChartValues(this.service.getCharts());

        //};

        //    [
        //    {
        //        key: "Cumulative Return",
        //        values: [
        //            {
        //                "label": "A",
        //                "value": -29.765957771107
        //            },
        //            {
        //                "label": "B",
        //                "value": 0
        //            },
        //            {
        //                "label": "C",
        //                "value": 32.807804682612
        //            },
        //            {
        //                "label": "D",
        //                "value": 196.45946739256
        //            },
        //            {
        //                "label": "E",
        //                "value": 0.19434030906893
        //            },
        //            {
        //                "label": "F",
        //                "value": -98.079782601442
        //            },
        //            {
        //                "label": "G",
        //                "value": -13.925743130903
        //            },
        //            {
        //                "label": "H",
        //                "value": -5.1387322875705
        //            }
        //        ]
        //    }
        //];
    }

    errorMessage: any;
}