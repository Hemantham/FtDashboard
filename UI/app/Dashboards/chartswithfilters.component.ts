

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef  } from '@angular/core';
import { DashboardView, ProductView, ViewSplit } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartValueService } from "../Charts/services/chart.services";
import { CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass } from '@angular/common';
import { SELECT_DIRECTIVES } from 'ng2-select';
import * as Charts from "../Charts/domain/chart.domain";
import { ActivatedRoute, Router } from '@angular/router';
import {Observable} from 'rxjs/Rx'

declare var jQuery: any;

@Component({
    selector: 'chart-with-filters',
    templateUrl: 'app/dashboards/templates/ChartsWithFilters.html',
    providers: [DashboardService, ChartValueService],
    directives: [ChartContainerComponent, SELECT_DIRECTIVES, NgClass, CORE_DIRECTIVES, FORM_DIRECTIVES]
})

export class ChartsWithFilters implements OnInit {
    
    public productView: ProductView;
    public splitFilters: Array<Charts.FieldValueModel>;
    public splits: Array<any> = [];
    public multipleSplitField: string;
    public selectedSplit: any = { id: 0, text: 'Overall' };
    public selectedRecency: any = { id: 1};
    public errorMessage: any;
    public recenciesTypes: Array<any> = [];
    public searchCriteria: Charts.ChartSearchCriteria ;
    private viewid: number;
    private paramsSubscription: any;

    @ViewChild(ChartContainerComponent)
    private chartContainerComponent: ChartContainerComponent;
    
    constructor(private service: DashboardService,
        private elementRef: ElementRef,
        private chartService: ChartValueService,
        private activatedRoute: ActivatedRoute,
        private router: Router
    ) {
        
    }

    ngOnInit(): void {

        this.chartService
            .getRecencies()
            .subscribe((res: Charts.RecencyType[]) => {
             
                this.recenciesTypes = res.map((r) => {
                   return  {
                        id:r.RecencyTypes, 
                        text: r.Name
                   }
                });
            }, this.alertError);
        
        this.paramsSubscription = this.activatedRoute.params
            .subscribe((params: any) => {

                //re-initialise when route changes
                this.viewid = parseInt(params['id']);
                this.selectedSplit = { id: 0, text: 'Overall' };
                this.splits = [];
                //alert(0)
                //wait for both responses to finish
                Observable.forkJoin(
                    this.service.getView(this.viewid),
                    this.chartService.getSplitFields(this.viewid)
                ).subscribe((res: any) => {

                    this.loadSplitsAndFilters(res[0]);
                    this.splitFilters = res[1];
                    this.loadChart();

                    }, this.alertError
                );
            });
    }

    alertError(error: any): void {
        alert(error);
        this.errorMessage = <any>error;
    }
    ngOnDestroy() {
        this.paramsSubscription.unsubscribe();
    }

    splitSelected(value: any) {
        this.selectedSplit = value;
        this.loadChart();
    }

    recencySelected(value: any) {
        debugger;
        this.selectedRecency = value;
        this.loadChart();
    }
    
    limitSelected(field: Charts.FieldValueModel, checked: boolean): void {

        field.IsSelected = checked;
        if (checked && this
                        .splitFilters
                        .filter((f) => f.IsSelected).length > 2) {

            this.splitFilters.filter((f) => f.Id !== field.Id && f.IsSelected)[0].IsSelected = false;
        }

        this.loadChart();
    }

    loadChart(): void {
       
        this.searchCriteria = new Charts.ChartSearchCriteria(new ViewSplit(this.selectedSplit.id, null, null, null,null ),
            this.splitFilters
                .filter((s) => s.IsSelected)
                .map((s) => s.Answer),
            this.viewid,
            this.selectedRecency.id

        );

        this.chartContainerComponent.load(this.searchCriteria);
        //  this.router.navigate(['/views/charts', this.viewid ]);
    }

    loadRecency(value: any): void {
        this.recenciesTypes = value;
    }

    loadSplitsAndFilters(response: ProductView): void {

        this.productView = response;

        this.splits = this.productView
            .ViewSplits
            .filter(s => s.SplitType === 'All')
            .map((s) => {
                return { id: s.Id, text: s.SplitName };
            });

        this.splits.unshift(this.selectedSplit);

        let multipleSplits: any = this.productView
            .ViewSplits
            .filter(s => s.SplitType === 'Multiple');

        if (multipleSplits != null && multipleSplits.length > 0) {
            this.multipleSplitField = multipleSplits[0].SplitName;
        }
    }
}
