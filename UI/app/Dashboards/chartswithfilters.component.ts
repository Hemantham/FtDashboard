

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef  } from '@angular/core';
import { DashboardView,ProductView , ViewSplit } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartValueService } from "../Charts/services/chart.services";
import { SELECT_DIRECTIVES } from 'ng2-select/ng2-select';
//import "babel-polyfill"

//import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';
declare var jQuery: any;

@Component({
    selector: 'chart-with-filters',
    templateUrl: 'app/dashboards/templates/ChartsWithFilters.html',
    providers: [DashboardService, ChartValueService],
    directives: [ChartContainerComponent, SELECT_DIRECTIVES]
})
   

export class ChartsWithFilters implements OnInit {
    
    public dashboard: ProductView;
    public viewSplits: ViewSplit[] ;
    public errorMessage: any;
    private viewid: number;
    private paramsSubscription: any;
    
    constructor(private service: DashboardService,
        private elementRef: ElementRef,
        private chartService: ChartValueService
    ) {
        
        this.viewid = 3;
        //this.paramsSubscription = this.activatedRoute.params.subscribe((params: any) => {
        //    this.productId = params['productid'];  //get your param
        //    alert(`${this.productId} is the productid`);
        //    // call your function that needs the route param
        //});
    }

    ngOnInit(): void {
         this.service.getView(this.viewid)
             .subscribe((response: ProductView) => {
                 this.dashboard = response;
                 this.viewSplits = this.dashboard.viewSplits.filter(s => s.splitType === 'All');
                 },
            error => this.errorMessage = <any>error
        );
    }

    ngOnDestroy() {
       // this.paramsSubscription.unsubscribe();
    }
}
