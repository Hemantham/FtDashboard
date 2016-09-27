import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
//import { ChartContainerComponent }  from './Charts/chartcontainer.component';
import { AppComponent }  from './app.component';
//import { ChartsWithFilters } from './dashboards/chartswithfilters.component';
//import { RouterModule } from '@angular/router';

import { routing, appRoutingProviders }  from './app.routes';

@NgModule({
    imports: [BrowserModule, FormsModule, HttpModule, JsonpModule, routing],
    declarations: [AppComponent],
    bootstrap: [AppComponent],
    providers: [
        appRoutingProviders
    ]
})

export class AppModule { }
