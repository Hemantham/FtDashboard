

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input  } from '@angular/core';
import { DashboardView, Filter } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartsWithFilters } from "./chartswithfilters.component";


@Component({
    selector: 'products',
    templateUrl: 'app/dashboards/templates/ProductsComponent.html',
    providers: [DashboardService],
    directives: [ChartsWithFilters]
})
   

export class ProductsComponent implements OnInit {
    
    public products: Filter[];
    public errorMessage: any;
    private paramsSubscription: any;
    private listRendered: boolean = false;

    constructor(private service: DashboardService) {
         
    }

    ngOnInit(): void {
         this.service.getProducts()
             .subscribe((response: Filter[]) => {
                     this.products = response;
                 },
            error => this.errorMessage = <any>error
        );
    }
}
