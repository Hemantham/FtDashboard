
import { Component } from '@angular/core';
import { Chart } from './Charts/chart.component';
import { ChartSearchCriteria, ChartFilter } from './charts/domain/chart.domain';


@Component({
    selector: 'chart-container',
    templateUrl: 'app/templates/AppComponent.html',
    directives: [Chart]
})
export class AppComponent {
    criteria: ChartSearchCriteria;
    constructor() {
        this.criteria = new ChartSearchCriteria();

        this.criteria.Filters = [
            new ChartFilter("GROUPS", "CONSUMER"),
            new ChartFilter("CHURNER_FLAG", "CHURNER"),
            new ChartFilter("OLDPRODUCT", "Overall Fixed")
        ];

        this.criteria.FieldOfInterest = "CHURN1";
        this.criteria.XAxisId = "ANALYSED_Week_#";
        this.criteria.XAxislable = "ANALYSED_Week";
    }
}
