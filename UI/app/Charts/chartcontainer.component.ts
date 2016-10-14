
import { Component, OnInit, Input  } from '@angular/core';
import { D3Chart  } from "./d3chart.component";
import { HighChartComponent  } from "./highchart.component";
import { ChartSearchCriteria, ChartModel, ChartsContainerModel, Recency} from '../charts/domain/chart.domain';
import { ViewSplit } from "../Dashboards/domain/dashboard.domain";
import { ChartValueService } from  "./services/chart.services";
import {PrintTools} from "../common/utilities/common.printtools";

declare var jQuery: any;


@Component({
    selector: 'chart-container',
    templateUrl: 'app/charts/templates/ChartContainerComponent.html',
    directives: [D3Chart, HighChartComponent],
    providers: [ChartValueService, PrintTools]
})


export class ChartContainerComponent implements OnInit {
    
    public charts: Array<ChartModel>;
    public isComparisonView: boolean;

    printThisDiv(event: any) {

        event.preventDefault();

        this.printTools.printHeadersAndSvg(jQuery(event.target).closest('.panel').find('svg'));

    }

    public show(element: string) {
        jQuery(element).show();

        jQuery(element)
            .find('.modal-dialog')
            .css({
                width: 'auto',
                height: 'auto',
                'max-height': '100%',
                'max-width': '80%'
            });
    }
    public hide(element: string) {
        jQuery(element).hide();
    }

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
    
    constructor(private service: ChartValueService, private printTools: PrintTools) {
        
    }

    ngOnInit() {

    }
}

