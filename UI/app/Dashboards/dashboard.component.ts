

import { ChartContainerComponent } from "../charts/chartcontainer.component"
import { Component, OnInit, Input, AfterContentInit, ViewChild, ElementRef  } from '@angular/core';
import { DashboardView,ProductView } from './domain/dashboard.domain';
import { DashboardService }  from "../dashboards/services/dashboard.services";
import { ChartsWithFilters } from "./chartswithfilters.component";
import { CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass } from '@angular/common';
//import { BUTTON_DIRECTIVES } from 'ng2-bootstrap/ng2-bootstrap';
import { SELECT_DIRECTIVES } from 'ng2-select';
//import "babel-polyfill"


//import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';
declare var jQuery: any;


@Component({
    selector: 'dashboard',
    templateUrl: 'app/dashboards/templates/DashboardComponent.html',
    providers: [DashboardService],
    directives: [ChartsWithFilters, SELECT_DIRECTIVES, NgClass, CORE_DIRECTIVES, FORM_DIRECTIVES]
})
   

export class DashboardComponent implements OnInit {

    //@ViewChild('sideMenu') sideMenu: ElementRef;

    public dashboards: ProductView[];
    public errorMessage: any;
    private productId: number;
    private paramsSubscription: any;
    private listRendered: boolean = false;


    public items: Array<string> = ['Amsterdam', 'Antwerp', 'Athens', 'Barcelona',
        'Berlin', 'Birmingham', 'Bradford', 'Bremen', 'Brussels', 'Bucharest',
        'Budapest', 'Cologne', 'Copenhagen', 'Dortmund', 'Dresden', 'Dublin',
        'Düsseldorf', 'Essen', 'Frankfurt', 'Genoa', 'Glasgow', 'Gothenburg',
        'Hamburg', 'Hannover', 'Helsinki', 'Kraków', 'Leeds', 'Leipzig', 'Lisbon',
        'London', 'Madrid', 'Manchester', 'Marseille', 'Milan', 'Munich', 'Málaga',
        'Naples', 'Palermo', 'Paris', 'Poznań', 'Prague', 'Riga', 'Rome',
        'Rotterdam', 'Seville', 'Sheffield', 'Sofia', 'Stockholm', 'Stuttgart',
        'The Hague', 'Turin', 'Valencia', 'Vienna', 'Vilnius', 'Warsaw', 'Wrocław',
        'Zagreb', 'Zaragoza'];

    
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
