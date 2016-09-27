
import { Component, Input} from '@angular/core';

@Component({
    selector: 'homescreen',
    template: `<div class="row">
                <div class="col-lg-12">
                <h4> <a [routerLink]="['/products']">Home</a> > {{title}}</h4>
                </div>
                </div>
                <div  style="background: url(./images/intro-bg.jpg) no-repeat center; width:100% ; height: 100%"></div>`
})

export class Homescreen {
    @Input() title: string;
}
