

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef  } from '@angular/core';
import { DashboardView,ProductView , ViewSplit } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartValueService } from "../Charts/services/chart.services";
import { CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass } from '@angular/common';
//import { BUTTON_DIRECTIVES } from 'ng2-bootstrap/ng2-bootstrap';
import { SELECT_DIRECTIVES } from 'ng2-select';
//import "babel-polyfill"

//import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';
declare var jQuery: any;

@Component({
    selector: 'chart-with-filters',
    templateUrl: 'app/dashboards/templates/ChartsWithFilters.html',
    providers: [DashboardService, ChartValueService],
    directives: [ChartContainerComponent, SELECT_DIRECTIVES, NgClass, CORE_DIRECTIVES, FORM_DIRECTIVES]
})
   

export class ChartsWithFilters implements OnInit {
    
    public dashboard: ProductView;
    public viewSplits: any;
    public errorMessage: any;
    private viewid: number;
    private paramsSubscription: any;


    public items: Array<any> = [];

    
    constructor(private service: DashboardService,
        private elementRef: ElementRef,
        private chartService: ChartValueService
    ) {
        
        this.viewid = 2;
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
                 this.items = this.dashboard
                     .ViewSplits
                     .filter(s => s.SplitType === 'All')
                     .map((s) => {
                          return { id: s.SplitField, text: s.SplitName };
                     });
                 },
            error => this.errorMessage = <any>error
        );
    }

    ngOnDestroy() {
       // this.paramsSubscription.unsubscribe();
    }
}
