import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
import { ChartContainerComponent }  from './Charts/chartcontainer.component';

@NgModule({
    imports: [BrowserModule,FormsModule,HttpModule,JsonpModule],
    declarations: [ChartContainerComponent ],
    bootstrap: [ChartContainerComponent ]
})

export class AppModule { }
