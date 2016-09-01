import { Component } from '@angular/core';
import { Chart } from './Charts/chart.component';


@Component({
    selector: 'my-app',
    templateUrl: 'app/templates/AppComponent.html',
    directives: [Chart]
})
export class AppComponent { }
