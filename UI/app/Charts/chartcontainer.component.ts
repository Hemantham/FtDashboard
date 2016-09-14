
import { Component, OnInit } from '@angular/core';
import { Chart } from '../charts/chart.component';
import { ChartSearchCriteria, DataChart, ChartModel } from '../charts/domain/chart.domain';
import { ViewSplit } from "../Dashboards/domain/dashboard.domain";
import { ChartValueService } from  "./services/chart.services";


@Component({
    selector: 'chart-container',
    templateUrl: 'app/charts/templates/ChartContainerComponent.html',
    directives: [Chart],
    providers: [ChartValueService]
})
export class ChartContainerComponent implements OnInit {
    private criteria: ChartSearchCriteria;
    public charts: Array<ChartModel>;

    constructor(private service: ChartValueService) {
        this.criteria = new ChartSearchCriteria(
            new ViewSplit(3, null, null, null, null),
            ["Customer Service", "Plans / pricing / inclusions"],
            2);
    }

    ngOnInit() {

        this.service
            .getCharts(this.criteria)
            .subscribe((charts: any) => {
                    debugger;
                    this.charts = charts;
                },
                error => {
                        <any>error;
                        alert(<any>error);
                }
            );

    }
}

