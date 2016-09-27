
import { ModuleWithProviders } from '@angular/core';
import { RouterConfig, RouterModule } from '@angular/router';
import { ChartsWithFilters } from './dashboards/chartswithfilters.component';
import { Homescreen } from './dashboards/Homescreen.component';
import { DashboardComponent } from './dashboards/dashboard.component';
import {ProductsComponent} from "./Dashboards/products.component";
import {ChartContainerComponent}  from "./Charts/chartcontainer.component";
export const routes: RouterConfig = [
    {
        path: '',
        component: ProductsComponent
    },
    {
        path: 'products',
        component: ProductsComponent
    },
    {
        path: 'dashboards/:productid',
        component: DashboardComponent,
         children: [
            {
                path: '', component: Homescreen
            }, 
            {
                path: 'views/:viewid', component: ChartsWithFilters
            } 
        ]
    }
];

export const appRoutingProviders: any[] = [

];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes);

