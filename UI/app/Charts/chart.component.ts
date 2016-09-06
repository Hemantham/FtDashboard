import {Component, OnInit, Input} from '@angular/core';
import {ChartValueService} from './services/chart.services'
import {nvD3} from 'ng2-nvd3';
import * as Chartdomain from "./domain/chart.domain";
import * as Enumerable from "linq-es2015";
declare let d3: any;

@Component({
    selector: 'chart',
    directives: [nvD3],
    providers: [ChartValueService],
    templateUrl: 'app/charts/templates/ChartComponent.html'
})

export class Chart implements OnInit {

    options: any;
    data: any;
    private service: ChartValueService;

    //this component takes in parameters from outside
    @Input() chartCriteria: Chartdomain.ChartSearchCriteria;

    constructor(service: ChartValueService) {
        this.service = service;
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
                values: s.data.map((dp) => {
                    return {x:dp.x, y:dp.y};
                })
            };
        });
    }


    getLineChartOptions() {

        return {
            chart: {
                type: 'lineChart',
                height: 500,
                margin: {
                    top: 20,
                    right: 20,
                    bottom: 70,
                    left: 55
                },
                x: function (d: any) { return d.x; },
                y: function (d: any) { return d.y; },
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
                    axisLabel: 'Y Axis'
                    //  axisLabelDistance: -10
                },
                rotateLabels: 0, //Angle to rotate x-axis labels.
                showControls: true, //Allow user to switch between 'Grouped' and 'Stacked' mode.
                groupSpacing: 0.1 //Distance between each group of bars.
            }
        };
    }

    getBarChartOptions() {
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

    ngOnInit() {

        this.options = this.getLineChartOptions();

        this.service.getCharts(this.chartCriteria)
            .subscribe(
            (heroes: any) => {
                
                let test: any = this.getLineChartValues(heroes);;
                //[
                //    {
                //            values: [{ x: 2, y: 10 }, { x: 3, y: 12 }, { x: 3, y: 20 }], //values - represents the array of {x,y} data points
                //        key: 'Sine Wave', //key  - the name of the series.
                //        color: '#ff7f0e' //color - optional: choose your own line color.
                //    },
                //    {
                //        values: [{ x: 2, y: 5 }, { x: 3, y: 12 }, { x: 3, y: 30 }],
                //        key: 'Cosine Wave',
                //        color: '#2ca02c'
                //    }
                //];
                this.data = test;
            },
            error => this.errorMessage = <any>error
            );

        //     return this.getBarChartValues(this.service.getCharts());

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