
import { Component, OnInit, Input  } from '@angular/core';
import { DashboardView, Product } from './dashboards/domain/dashboard.domain';
import { DashboardService }  from "./dashboards/services/dashboard.services";
import { ChartsWithFilters } from "./dashboards/chartswithfilters.component";

declare var jQuery: any;

@Component({
    selector: 'app',
    template: '<router-outlet></router-outlet>'
  
})
   

export class AppComponent  {
    
  
}
