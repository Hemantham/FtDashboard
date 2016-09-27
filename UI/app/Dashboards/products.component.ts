

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input  } from '@angular/core';
import { DashboardView, Product } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartsWithFilters } from "./chartswithfilters.component";

declare var jQuery: any;

@Component({
    selector: 'products',
    templateUrl: 'app/dashboards/templates/ProductsComponent.html',
    providers: [DashboardService],
    directives: [ChartsWithFilters]
})
   

export class ProductsComponent implements OnInit {
    
    public products: Product[];
    public errorMessage: any;
    private paramsSubscription: any;
    private listRendered: boolean = false;

    constructor(private service: DashboardService) {
         
    }

    ngOnInit(): void {
         this.service.getProducts()
             .subscribe((response: Product[]) => {
                     this.products = response;
                 },
            error => this.errorMessage = <any>error
        );
    }
}
