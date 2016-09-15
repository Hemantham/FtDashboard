

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef  } from '@angular/core';
import { DashboardView,ProductView , ViewSplit } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartValueService } from "../Charts/services/chart.services";
import { CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass } from '@angular/common';
import { SELECT_DIRECTIVES } from 'ng2-select';
import * as Charts from "../Charts/domain/chart.domain";
import { ActivatedRoute, Router } from '@angular/router';

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
    public splits: Array<any> = [];
    public multipleSplitField : string ;
    public searchCriteria: Charts.ChartSearchCriteria;
    private viewid: number;
    private paramsSubscription: any;
    
    constructor(private service: DashboardService,
        private elementRef: ElementRef,
        private chartService: ChartValueService,
        private activatedRoute: ActivatedRoute,
        private router: Router
    ) {
      
    }

    ngOnInit(): void {

        this.paramsSubscription = this.activatedRoute.params
            .subscribe((params: any) => {

                alert('routes changed');
                this.viewid = parseInt(params['id']);

                this.service.getView(this.viewid)
                    .subscribe((response: ProductView) => {

                        this.productView = response;

                        this.splits = this.productView
                            .ViewSplits
                            .filter(s => s.SplitType === 'All')
                            .map((s) => {
                                return { id: s.SplitField, text: s.SplitName };
                            });

                        let multipleSplits : any =  this.productView
                            .ViewSplits
                            .filter(s => s.SplitType === 'Multiple')
                            .map((s) => {
                                return { id: s.SplitField, text: s.SplitName };
                            });

                        if (multipleSplits != null && multipleSplits.length > 0) {
                            this.multipleSplitField = multipleSplits[0].text;
                        }
                    }
                    , error => this.errorMessage = <any>error
                    );

                this.chartService
                    .getSplitFields(this.viewid)
                    .subscribe((response: Charts.Response[]) => {
                        this.splitFilters = response;
                    }
                    , error => this.errorMessage = <any>error
                );

            });

        this.loadChart();
    }

    ngOnDestroy() {
        this.paramsSubscription.unsubscribe();
    }

    loadChart(): void {
        alert('loard chart');
        this.searchCriteria = new Charts.ChartSearchCriteria(new ViewSplit(5, null, null, null, null),
            ["Customer Service", "Plans / pricing / inclusions"],
            this.viewid
        );
        //  this.router.navigate(['/views/charts', this.viewid ]);
    }
}
