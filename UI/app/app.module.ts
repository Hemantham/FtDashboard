import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
import { ChartContainerComponent }  from './Charts/chartcontainer.component';
import { DashboardComponent }  from './Dashboards/dashboard.component';

@NgModule({
    imports: [BrowserModule,FormsModule,HttpModule,JsonpModule],
    declarations: [ DashboardComponent ],
    bootstrap: [ DashboardComponent]
})

export class AppModule { }
