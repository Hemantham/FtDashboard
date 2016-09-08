

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef  } from '@angular/core';
import { DashboardView,ProductView } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
//import "babel-polyfill"


//import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';
declare var jQuery: any;


@Component({
    selector: 'dashboard',
    templateUrl: 'app/dashboards/templates/DashboardComponent.html',
    providers: [DashboardService],
    directives: [ChartContainerComponent]
})
   

export class DashboardComponent implements OnInit {

    @ViewChild('sideMenu') sideMenu: ElementRef;

    dashboards: ProductView[];
    errorMessage: any;
    
    private productId: number;
    private paramsSubscription: any;
    private listRendered : boolean = false;
    
    constructor(private service: DashboardService, private elementRef: ElementRef) {
        
        this.productId = 2;
        //this.paramsSubscription = this.activatedRoute.params.subscribe((params: any) => {
        //    this.productId = params['productid'];  //get your param
        //    alert(`${this.productId} is the productid`);
        //    // call your function that needs the route param
        //});
    }


    onListRendered() {
        if (!this.listRendered) {

            jQuery('#side-menu').metisMenu();
            this.listRendered = true;
        }
    }

    ngOnInit(): void {
         this.service.getViews(this.productId)
             .subscribe((response: ProductView[]) => {
                     this.dashboards = response;
                 },
            error => this.errorMessage = <any>error
        );

       
         


    }

    ngOnDestroy() {
       // this.paramsSubscription.unsubscribe();
    }
}
