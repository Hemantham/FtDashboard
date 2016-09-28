

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { DashboardView, ProductView, ViewSplit, Product } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartValueService } from "../Charts/services/chart.services";
import { CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass } from '@angular/common';
import { SELECT_DIRECTIVES, SelectComponent } from 'ng2-select';
import { SelectItem } from 'ng2-select/components/select/select-item';
import * as Charts from "../Charts/domain/chart.domain";
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Rx'
import * as Enumerable from "linq-es2015";
import {DropDownTools} from  "../common/utilities/common.dropdowntools";

declare var jQuery: any;

@Component({
    selector: 'overall-chart-with-filters',
    templateUrl: 'app/dashboards/templates/OvarallChartsWithFilters.html',
    providers: [DashboardService, ChartValueService , DropDownTools],
    directives: [ChartContainerComponent, SELECT_DIRECTIVES, NgClass, CORE_DIRECTIVES, FORM_DIRECTIVES]
})

export class OverallChartsWithFilters implements OnInit{

    public errorMessage: any;

   // public productView: ProductView;
   // public splitFilters: Array<Charts.FieldValueModel>;
    
   // public multipleSplitField: string;

    public views: Array<any> = [];
    public selectedView: any = { id: 0 };

    public recenciesTypes: Array<any> = [];
    public selectedRecencyType: any = { id: 0, text: 'Weekly' };

    public selectedRecencies: Array<Charts.Recency> = [];
    public recencies: Array<any> = [];

    public selectedCodeframes: Array<Charts.Response> = [];
    public codeframes: Array<any> = [];

    public selectedProducts: Array<Product> = [];
    public products: Array<any> = [];
    
    public searchCriteria: Charts.ChartSearchCriteria ;
   // private viewid: number;
    private paramsSubscription: any;

    @ViewChild(ChartContainerComponent)
    private chartContainerComponent: ChartContainerComponent;

    @ViewChild('dropdownRecency')
    private dropdownRecency: SelectComponent;

    @ViewChild('dropdownRecencyType')
    private dropdownRecencyType: SelectComponent;

    @ViewChild('dropdownCodeframes')
    private dropdownCodeframes: SelectComponent;

    @ViewChild('dropdownProduct')
    private dropdownProduct: SelectComponent;

    @ViewChild('dropdownView')
    private dropdownView: SelectComponent;
    
    constructor(private service: DashboardService,
        private elementRef: ElementRef,
        private chartService: ChartValueService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private dropdowntools: DropDownTools
    ) {
        
    }

    ngOnInit(): void {

     //   this.views.push({ id: 1, text:'test' });
        
        //this.dropdowntools.loadSingleDropDownData(this.service.getDashboardViews(),
        //    this.dropdownView,
        //    this.views,
        //    this.selectedView,
        //    (r) => {
        //        return {
        //            id: r.Id,
        //            text: r.Name
        //        }
        //    },
        //    () => {
        //        this.loadChart(true);
        //    }
        //);



        this.service.getDashboardViews()
            .subscribe((res: Array<DashboardView>) => {

                this.views = res.map((r) => {
                    return {
                        id: r.Id,
                        text: r.Name
                    }
                });

                if (this.views.length > 0) {
                    this.selectedView = this.views[0];
                    setTimeout(() => {
                            if (this.dropdownView.itemObjects.length > 0)
                            this.dropdownView.active = [this.dropdownView.itemObjects[0]];
                            this.loadChart(true);
                        },
                        500);

                   
                }

            }, this.alertError);



        this.dropdowntools.loadSingleDropDownData(this.chartService.getRecencyTypes(),
            this.dropdownRecencyType,
            this.recenciesTypes,
            this.selectedRecencyType,
            (r) => {
                return {
                    id: r.RecencyTypes,
                    text: r.Name
                }
            },
            null
        );

        //this.chartService
        //    .getRecencyTypes()
        //    .subscribe((res: Charts.RecencyType[]) => {
        //        this.recenciesTypes = res.map((r) => {
        //            return {
        //                id: r.RecencyTypes,
        //                text: r.Name
        //            }
        //        });

        //        this.selectedRecencyType = { id: 0, text: 'Weekly' };

        //    }, this.alertError);

        this.service.getProducts()
            .subscribe((res: Array<Product>) => {

                this.products = res.map((r) => {
                    return {
                        id: r.Id,
                        text: r.Name
                    }
                });

                this.dropdowntools.setMultipleDropDownAllActive(this.dropdownProduct,
                    this.selectedProducts,
                    (item: SelectItem) => {return new Product(parseInt(item.id),item.text, null,null,null);},
                    true);

            }, this.alertError);
        
        //this.paramsSubscription = this.activatedRoute.params
        //    .subscribe((params: any) => {

                //re-initialise when route changes
               // this.viewid = parseInt(params['viewid']);
               
                //if (this.dropdownRecencyType.itemObjects.length > 0)
                //    this.dropdownRecencyType.active = [this.dropdownRecencyType.itemObjects[0]];

                //if (this.dropdownView.itemObjects.length > 0)
                //    this.dropdownView.active = [this.dropdownView.itemObjects[0]];

                ////reinit the bound fields when the view changes
                //this.selectedView = { id: 0, text: 'Overall' };
                //this.selectedRecencyType = { id: 0, text: 'Weekly' };
                //this.selectedRecencies = [];
               
                //wait for both responses to finish
                //Observable.forkJoin(
                //    this.service.getDashboardViews()
                //).subscribe((res: any) => {
                    //this.loadProducts(res[0]);
                    //this.views = res[1];
                    ////this.splitFilters = res[1];
                   // this.loadChart(true);

                //    }, this.alertError
                //);
         //   });
    }

    alertError(error: any): void {
        alert(error);
        this.errorMessage = <any>error;
    }

    ngOnDestroy() {
        this.paramsSubscription.unsubscribe();
    }

    viewSelected(value: any) {
        this.selectedView = { id: value.id, text: value.text };
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

    productSelected(value: any) {

        this.selectedProducts.push(new Product(value.id,value.text,null,null,null));
        this.dropdownProduct.active.sort((left, right) => parseInt(left.id) - parseInt(right.id));
        this.loadChart(false);
    }

    productRemoved(value: any) {
        this.selectedProducts.splice(
            this.selectedProducts.indexOf(
                this.selectedProducts.filter((r) => r.Id == value.id)[0]), 1);
        this.loadChart(false);
    }

    codeframeSelected(value: any) {
        this.selectedCodeframes.push(new Charts.Response(null,null,null,null,null,
            value.text,null));

        this.dropdownCodeframes.active.sort((left, right) => parseInt(left.id) - parseInt(right.id));
        this.loadChart(false);
    }

    codeframeRemoved(value: any) {
        this.selectedCodeframes.splice(
            this.selectedCodeframes.indexOf(
                this.selectedCodeframes.filter((r) => r.answer == value.id)[0]), 1);
        this.loadChart(false);
    }


    recencyTypeSelected(value: any) {
        this.selectedRecencyType = { id: value.id, text: value.text };
        this.selectedRecencies = [];
        this.loadChart(true);
    }

    //limitSelected(field: Charts.FieldValueModel, checked: boolean): void {

    //    field.IsSelected = checked;
    //    if (checked && this
    //                    .splitFilters
    //                    .filter((f) => f.IsSelected).length > 2) {

    //        this.splitFilters.filter((f) => f.Id !== field.Id && f.IsSelected)[0].IsSelected = false;
    //    }

    //    this.loadChart(false);
    //}

    //setMultipleDropDownAllActive<T>(dropdown: SelectComponent,
    //    array: Array<T>,
    //    callback: (selectItem: SelectItem) => T,
    //    isInitial: boolean): void {

    //    setTimeout(() => {
    //        if (dropdown.itemObjects.length > 0) {
    //            if (isInitial) {
    //                dropdown.active.splice(0);
    //                array.splice(0);
    //                dropdown.itemObjects.forEach((item) => {
    //                    dropdown.active.push(item);
    //                    array.push(callback(item));
    //                });
    //            }
    //        } else {
    //            array = [];
    //            dropdown.active = [];
    //        }
    //    }, 500);
    //}

    loadRecencies(cm: Array<Charts.Recency>, isInitial: boolean): void {

        if (isInitial) {
            this.recencies = cm.map((m) => {
                return {
                    id: m.RecencyNumber,
                    text: m.Lable
                }
            });
        }

        this.dropdowntools.setMultipleDropDownAllActive(     this.dropdownRecency,
                                                        this.selectedRecencies,
                                                        (item: SelectItem) => {return new Charts.Recency(parseInt(item.id),item.text);},
                                                        isInitial);
       
    }

    loadChart(isInitial: boolean): void {

       

        this.searchCriteria = new Charts.ChartSearchCriteria(null,
            null,
            null,
            this.selectedRecencyType.id,
            this.selectedRecencies, this.selectedView.id
        );

        this.chartContainerComponent.load(this.searchCriteria,
            (cm) => {
                this.loadRecencies(cm != null && cm.length > 0 ? cm[0].recencies : null, isInitial);
                this.loadCodeFrames(cm);
            });
    }

    loadCodeFrames(chartModels: Array<Charts.ChartModel>) {

        let modelList = Enumerable.asEnumerable(chartModels);

        this.codeframes = modelList
            .SelectMany((m: Charts.ChartModel) => m.series)
            .Select((s: Charts.ChartSeriesModel) => {
               return {
                   id: s.key,
                   text: s.key
               }
            })
            .ToArray();
    }

    //loadProducts(response: Array<Product>): void {

    //    this.products = response;
    //    //////this.splits = this.productView
    //    //////    .ViewSplits
    //    //////    .filter(s => s.SplitType === 'All')
    //    //////    .map((s) => {
    //    //////        return { id: s.Id, text: s.SplitName };
    //    //////    });

    //    //////this.splits.unshift(this.selectedSplit);

    //    //////let multipleSplits: any = this.productView
    //    //////    .ViewSplits
    //    //////    .filter(s => s.SplitType === 'Multiple');

    //    //////if (multipleSplits != null && multipleSplits.length > 0) {
    //    //////    this.multipleSplitField = multipleSplits[0].SplitName;
    //    //////}
    //}
}
