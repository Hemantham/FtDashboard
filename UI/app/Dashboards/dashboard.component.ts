﻿

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit  } from '@angular/core';
import { DashboardView, ProductViewModel } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartsWithFilters } from "./chartswithfilters.component";
import { ActivatedRoute, Router } from '@angular/router';
import { PrintTools }  from "../common/utilities/common.printtools"


declare var jQuery: any;


@Component({
    selector: 'dashboard',
    templateUrl: 'app/dashboards/templates/DashboardComponent.html',
    providers: [DashboardService, PrintTools],
    directives: [ChartsWithFilters]
})
   

export class DashboardComponent implements OnInit {
    
    public productViews: ProductViewModel[];
    public errorMessage: any;
    private paramsSubscription: any;
    private listRendered: boolean = false;
    public productId: number;
    public productLink: string;
    
    constructor(private service: DashboardService, private activatedRoute: ActivatedRoute, private router: Router, private printTools : PrintTools) {
        
    }

    print() {

        event.preventDefault();

        this.printTools.printHeadersAndSvg(jQuery('.chartprinter svg'));

    }

    onListRendered() {
        if (!this.listRendered) {
            jQuery('#side-menu').metisMenu();
            this.listRendered = true;
        }
    }

    onNavigate(anchor : any) {
        var element = jQuery(anchor)
            .addClass('active')
            .parent();
        debugger;
        while (true) {
            if (element.is('li')) {
                element = element.parent().addClass('in').parent();
            } else {
                break;
            }
        }
    }

    ngOnInit(): void {

        this.paramsSubscription = this.activatedRoute.params.subscribe((params: any) => {
            if (params['productid']) {
                this.productId = params['productid']; //get your param
                this.productLink = `/dashboards/${this.productId}/views/`;

                this.service.getViews(this.productId)
                    .subscribe((response: ProductViewModel[]) => {
                        this.productViews = response;
                        //initially load first
                       this.router.navigate([this.productLink, this.productViews[0].Id]);
                    },
                    error => this.errorMessage = <any>error
                    );
            } else {
                this.router.navigate(["dashboards/views"]);
            }
        });
    }

    ngOnDestroy() {
        this.paramsSubscription.unsubscribe();
    }
}
