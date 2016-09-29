
import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { DashboardView, FilteredDashboardView as ProductView, ViewSplit } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartValueService } from "../Charts/services/chart.services";
import { CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass } from '@angular/common';
import { SELECT_DIRECTIVES, SelectComponent } from 'ng2-select';
import { SelectItem } from 'ng2-select/components/select/select-item';
import * as Charts from "../Charts/domain/chart.domain";
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Rx'
import {DropDownTools} from  "../common/utilities/common.dropdowntools";



declare var jQuery: any;

@Component({
    selector: 'chart-with-filters',
    templateUrl: 'app/dashboards/templates/ChartsWithFilters.html',
    providers: [DashboardService, ChartValueService, DropDownTools],
    directives: [ChartContainerComponent, SELECT_DIRECTIVES, NgClass, CORE_DIRECTIVES, FORM_DIRECTIVES]
})

export class ChartsWithFilters implements OnInit{

    public errorMessage: any;

    public productView: ProductView;
    public splitFilters: Array<Charts.FieldValueModel>;
    
    public multipleSplitField: string;

    public splits: Array<any> = [];
    public selectedSplit: any = { id: 0, text: 'Overall' };

    public recenciesTypes: Array<any> = [];
    public selectedRecencyType: any = { id: 0, text: 'Weekly' };

    public selectedRecencies: Array<Charts.Recency> = [];
    public recencies: Array<any> = [];
    
    public searchCriteria: Charts.ChartSearchCriteria ;
    private viewid: number;
    private paramsSubscription: any;

    @ViewChild(ChartContainerComponent)
    private chartContainerComponent: ChartContainerComponent;

    @ViewChild('dropdownRecency')
    private dropdownRecency: SelectComponent;

    @ViewChild('dropdownRecencyType')
    private dropdownRecencyType: SelectComponent;

    @ViewChild('dropdownCut')
    private dropdownCut: SelectComponent;
    
    constructor(private service: DashboardService,
        private elementRef: ElementRef,
        private chartService: ChartValueService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private dropdowntools: DropDownTools
    ) {
        
    }
   

    ngOnInit(): void {

        this.chartService
            .getRecencyTypes()
            .subscribe((res: Charts.RecencyType[]) => {
             
                this.recenciesTypes = res.map((r) => {
                   return  {
                        id: r.RecencyTypes, 
                        text: r.Name
                   }
                });
            }, this.alertError);
        
        this.paramsSubscription = this.activatedRoute.params
            .subscribe((params: any) => {

                //re-initialise when route changes
                this.viewid = parseInt(params['viewid']);
               
                if (this.dropdownRecencyType.itemObjects.length > 0)
                    this.dropdownRecencyType.active = [this.dropdownRecencyType.itemObjects[0]];

                if (this.dropdownCut.itemObjects.length > 0)
                    this.dropdownCut.active = [this.dropdownCut.itemObjects[0]];

                //reinit the bound fields when the view changes
                this.selectedSplit = { id: 0, text: 'Overall' };
                this.selectedRecencyType = { id: 0, text: 'Weekly' };
                this.selectedRecencies = [];
               
                //wait for both responses to finish
                Observable.forkJoin(
                    this.service.getView(this.viewid),
                    this.chartService.getSplitFields(this.viewid)
                ).subscribe((res: any) => {

                    this.loadSplitsAndFilters(res[0]);
                    this.splitFilters = res[1];
                    this.loadChart(true);

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
        this.selectedSplit = { id: value.id, text: value.text };
        this.loadChart(true);
    }

    recencySelected(value: any) {
        this.selectedRecencies.push(new Charts.Recency(
             value.id,
             value.text
        ));

        this.dropdownRecency.active.sort((left,right) => parseInt(left.id) - parseInt(right.id) );
        this.loadChart(false);
    }

    recencyRemoved(value: any) {
        this.selectedRecencies.splice(
            this.selectedRecencies.indexOf(
                this.selectedRecencies.filter((r) => r.RecencyNumber == value.id)[0]), 1);
        this.loadChart(false);
    }

    recencyTypeSelected(value: any) {
        this.selectedRecencyType = { id: value.id, text: value.text };
        this.selectedRecencies = [];
        this.loadChart(true);
    }

    limitSelected(field: Charts.FieldValueModel, checked: boolean): void {

        field.IsSelected = checked;
        if (checked && this
                        .splitFilters
                        .filter((f) => f.IsSelected).length > 2) {

            this.splitFilters.filter((f) => f.Id !== field.Id && f.IsSelected)[0].IsSelected = false;
        }

        this.loadChart(false);
    }

    loadRecencies(recency: Array<Charts.Recency>, isInitial: boolean): void {

        if (isInitial) {
            this.recencies = recency.map((m) => {
                return {
                    id: m.RecencyNumber,
                    text: m.Lable
                }
            });
        }

        this.dropdowntools.setMultipleDropDownAllActive(this.dropdownRecency,
            this.selectedRecencies,
            (item: SelectItem) => { return new Charts.Recency(parseInt(item.id), item.text); },
            isInitial);
    }

    loadChart(isInitial: boolean): void {

        this.searchCriteria = new Charts.ChartSearchCriteria(new ViewSplit(this.selectedSplit.id, null, null, null,null ),
            this.splitFilters
                .filter((s) => s.IsSelected)
                .map((s) => s.Answer),
            this.viewid,
            this.selectedRecencyType.id,
            this.selectedRecencies,
            null
        );

        this.chartContainerComponent.load(this.searchCriteria,(cm)=>
            this.loadRecencies(cm != null && cm.length > 0 ? cm[0].recencies : null, isInitial));
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
