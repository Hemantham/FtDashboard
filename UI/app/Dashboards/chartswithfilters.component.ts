

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef  } from '@angular/core';
import { DashboardView,ProductView , ViewSplit } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartValueService } from "../Charts/services/chart.services";
import { CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass } from '@angular/common';
import { SELECT_DIRECTIVES } from 'ng2-select';
import * as Charts from "../Charts/domain/chart.domain";

declare var jQuery: any;

@Component({
    selector: 'chart-with-filters',
    templateUrl: 'app/dashboards/templates/ChartsWithFilters.html',
    providers: [DashboardService, ChartValueService],
    directives: [ChartContainerComponent, SELECT_DIRECTIVES, NgClass, CORE_DIRECTIVES, FORM_DIRECTIVES]
})
   

export class ChartsWithFilters implements OnInit {
    
    public productView: ProductView;
    public splitFilters: Charts.Response[];
    public errorMessage: any;
    private viewid: number;
    private paramsSubscription: any;
    public splits: Array<any> = [];
    
    constructor(private service: DashboardService,
        private elementRef: ElementRef,
        private chartService: ChartValueService) {
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
                                        this.productView = response;
                                        this.splits = this.productView
                                                         .ViewSplits
                                                         .filter(s => s.SplitType === 'All')
                                                         .map((s) => {
                                                              return { id: s.SplitField, text: s.SplitName };
                                                         });
                }
                , error => this.errorMessage = <any>error
                );

          

            this.chartService
                .getFields(this.viewid)
                .subscribe((response: Charts.Response[]) => {
                    this.splitFilters = response;
                }
                , error => this.errorMessage = <any>error
                );


    }



    ngOnDestroy() {
       // this.paramsSubscription.unsubscribe();
    }


}
