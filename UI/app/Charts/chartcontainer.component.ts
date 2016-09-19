
import { Component, OnInit, Input } from '@angular/core';
import { Chart } from '../charts/chart.component';
import { ChartSearchCriteria, ChartModel, ChartsContainerModel, Recency} from '../charts/domain/chart.domain';
import { ViewSplit } from "../Dashboards/domain/dashboard.domain";
import { ChartValueService } from  "./services/chart.services";


@Component({
    selector: 'chart-container',
    templateUrl: 'app/charts/templates/ChartContainerComponent.html',
    directives: [Chart],
    providers: [ChartValueService]
})
export class ChartContainerComponent implements OnInit {
    
    public charts: Array<ChartModel>;

    public load(criteria: ChartSearchCriteria, callback: (chartmodels: Array<Recency>) => any) {
        this.service
            .getCharts(criteria)
            .subscribe((charts: any) => {
                   
                callback(charts != null && charts.length > 0 ? charts[0].recencies: null);
                this.charts = charts;
            },
            error => {
                <any>error;
                alert(<any>error);
            }
        );
    }
    
    constructor(private service: ChartValueService) {

        //this.criteria = new ChartSearchCriteria(
        //    new ViewSplit(5, null, null, null, null),
        //    ["Customer Service", "Plans / pricing / inclusions"],
        //    3);
    }

    ngOnInit() {

    }
}

