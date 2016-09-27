

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit  } from '@angular/core';
import { DashboardView, ProductViewModel } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartsWithFilters } from "./chartswithfilters.component";
import { ActivatedRoute, Router } from '@angular/router';

declare var jQuery: any;


@Component({
    selector: 'dashboard',
    templateUrl: 'app/dashboards/templates/DashboardComponent.html',
    providers: [DashboardService],
    directives: [ChartsWithFilters]
})
   

export class DashboardComponent implements OnInit {
    
    public productViews: ProductViewModel[];
    public errorMessage: any;
    private paramsSubscription: any;
    private listRendered: boolean = false;
    public productId: number;
    public productLink: string;
    
    constructor(private service: DashboardService, private activatedRoute: ActivatedRoute, private router: Router) {
        
    }

    onListRendered() {
        if (!this.listRendered) {
            jQuery('#side-menu').metisMenu();
            this.listRendered = true;
        }
    }

    ngOnInit(): void {

        this.paramsSubscription = this.activatedRoute.params.subscribe((params: any) => {
            this.productId = params['productid'];  //get your param
            this.productLink = `/dashboards/${this.productId}/views/`;

            // call your function that needs the route param
            this.service.getViews(this.productId)
                .subscribe((response: ProductViewModel[]) => {
                    this.productViews = response;
                    //initially load first
                    //alert(this.productLink);
                    this.router.navigate([this.productLink, this.productViews[0].Id]); 
                },
                error => this.errorMessage = <any>error
                );
        });
    }

    ngOnDestroy() {
        this.paramsSubscription.unsubscribe();
    }
}
