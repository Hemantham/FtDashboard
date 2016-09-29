
import { Component, OnInit, Input } from '@angular/core';
import { D3Chart  } from "./d3chart.component";
import { HighChartComponent  } from "./highchart.component";
import { ChartSearchCriteria, ChartModel, ChartsContainerModel, Recency} from '../charts/domain/chart.domain';
import { ViewSplit } from "../Dashboards/domain/dashboard.domain";
import { ChartValueService } from  "./services/chart.services";


@Component({
    selector: 'chart-container',
    templateUrl: 'app/charts/templates/ChartContainerComponent.html',
    directives: [D3Chart, HighChartComponent],
    providers: [ChartValueService]
})
export class ChartContainerComponent implements OnInit {
    
    public charts: Array<ChartModel>;
    public isComparisonView : boolean;

    public load(criteria: ChartSearchCriteria, callback: (chartmodels: Array<ChartModel>) => any) {

        this.isComparisonView = criteria.DashboardViewId > 0; 
        this.service
            .getCharts(criteria)
            .subscribe((charts: any) => {
                callback(charts);
                this.charts = charts;
               
            },
            error => {
                <any>error;
                alert(<any>error);
            }
        );
    }
    
    constructor(private service: ChartValueService) {
        
    }

    ngOnInit() {

    }
}

