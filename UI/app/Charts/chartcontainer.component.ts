
import { Component } from '@angular/core';
import { Chart } from '../charts/chart.component';
import { ChartSearchCriteria } from '../charts/domain/chart.domain';
import { ViewSplit } from "../Dashboards/domain/dashboard.domain";

@Component({
    selector: 'chart-container',
    templateUrl: 'app/charts/templates/ChartContainerComponent.html',
    directives: [Chart]
})
export class ChartContainerComponent {
    criteria: ChartSearchCriteria;
    constructor() {
        this.criteria = new ChartSearchCriteria(
            null, ["Customer Service", "Plans / pricing / inclusions"], 2);
    }
}
