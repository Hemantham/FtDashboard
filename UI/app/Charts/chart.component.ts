import {Component, OnInit} from '@angular/core';
import {ChartValueService} from './services/chart.services'
import {nvD3} from 'ng2-nvd3';
import Chartdomain = require("./domain/chart.domain");
import * as Enumerable from "linq-es2015";
declare let d3: any;

@Component({
    selector: 'chart',
    directives: [nvD3],
    providers: [ChartValueService],
    template: ` <div>
                  <nvd3 [options]="options" [data]="data"></nvd3>
                </div>
              `
})

export class Chart implements OnInit {

    options: any;
    data: any;
    private service: ChartValueService;

    constructor(service: ChartValueService) {
        this.service = service;
    }

    getBarChartValues(chart: Chartdomain.ChartModel): any {
        return chart.series.map((s) => {
            return {
                key: s.key,
                values: s.data.map((dp) => {
                    return {
                        value: dp.x,
                        label: dp.label
                    };
                })
        };
       });
    }

    ngOnInit() {

        this.options = {
            chart: {
                type: 'lineChart',
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
                rotateLabels : 0 ,     //Angle to rotate x-axis labels.
                showControls : true,   //Allow user to switch between 'Grouped' and 'Stacked' mode.
                groupSpacing : 0.1   //Distance between each group of bars.
            }
        }

        //this.data = () => {

            this.service.getCharts()
                .subscribe(
                (heroes: any) => {
                    let test: any = this.getBarChartValues(heroes);
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