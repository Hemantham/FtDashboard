import {DashboardView, Filter, Product, ViewSplit, ProductView} from "../domain/dashboard.domain.ts" 
import * as Enumerable from "linq-es2015";

import { Injectable }     from '@angular/core';
import { Http, Response, Request, Headers, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import '../../rxjs-operators'

@Injectable()
export class DashboardService {
    constructor(private http: Http) { }
    
    public getViews(id: number): Observable<ProductView[]> {

        //let headers = new Headers({ 'Content-Type': 'application/json' });
        //let options = new RequestOptions({ headers: headers });
        let url = `/Dashboard.Rest/api/products/${id}/views`; 
        return this.http.get(url)
                    .map(this.extractViewData)
                    .catch(this.handleError);
    }

    public getView(id: number): Observable<ProductView> {

        //let headers = new Headers({ 'Content-Type': 'application/json' });
        //let options = new RequestOptions({ headers: headers });
        let url = `/Dashboard.Rest/api/products/views/${id}`;  
        return this.http.get(url)
            .map(this.extractSingleViewData)
            .catch(this.handleError);
    }

    private extractSingleViewData(res: Response): ProductView {
        let body: ProductView = res.json();
        return body;
    }

    private extractViewData(res: Response): ProductView[] {
        // debugger;
        let body: ProductView[] = res.json();
        return body;
    }

    private handleError(error: any) {
        // In a real world app, we might use a remote logging infrastructure
        // We'd also dig deeper into the error to get a better message
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg); // log to console instead
        return Observable.throw(errMsg);
    }
}




