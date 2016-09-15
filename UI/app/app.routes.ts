
import { ModuleWithProviders } from '@angular/core';
import { RouterConfig, RouterModule } from '@angular/router';
import { ChartsWithFilters } from './dashboards/chartswithfilters.component';
import { Homescreen } from './dashboards/Homescreen.component';
import { ChartContainerComponent } from './charts/chartcontainer.component';

export const routes: RouterConfig = [
    {
        path: '',
        component: Homescreen
    },
    {
        path: 'views/:id',
        component: ChartsWithFilters
        //children: [
        //    {
        //        path: '', component: Homescreen
        //    }, // url: views/2/,
        //    {
        //        path: 'charts/:viewid', component: ChartContainerComponent 
        //    } // url: views/2/charts
        //]
    }
];

export const appRoutingProviders: any[] = [

];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes);

