
import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input } from '@angular/core';
import { DashboardView } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";

@Component({
    selector: 'dashboard',
    templateUrl: 'app/dashboards/templates/DashboardComponent.html',
    providers: [DashboardService],
    directives: [ChartContainerComponent]
})

export class DashboardComponent implements OnInit {

    dashboards : DashboardView[];
    private service: DashboardService;

    @Input() productId: number;
    
    constructor( service: DashboardService) {
        this.service = service;
    }

    ngOnInit(): void {
        this.service.getViews(this.productId);
    }
}
