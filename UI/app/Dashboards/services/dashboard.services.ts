﻿import {DashboardView, ProductViewModel,  Filter , ViewSplit, FilteredDashboardView as ProductView} from "../domain/dashboard.domain.ts" 
import * as Enumerable from "linq-es2015";

import { Injectable }     from '@angular/core';
import { Http, Response, Request, Headers, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import '../../rxjs-operators'

@Injectable()
export class DashboardService {
    constructor(private http: Http) {
        this.headers = new Headers();
        this.headers.append('Accept', 'application/json');
    }
    private headers: Headers;

    public getViews(id: number): Observable<ProductViewModel[]> {
        
        let url = `/Dashboard.Rest/api/products/${id}/views`; 
        return this.http.get(url, {headers: this.headers})
                    .map(this.extractViewData)
                    .catch(this.handleError);
    }

    public getDashboardViews(): Observable<DashboardView[]> {
        
        let url = `/Dashboard.Rest/api/dashboardviews`;
        return this.http.get(url, { headers: this.headers })
            .map((res: Response)=> {
                    let body: DashboardView[] = res.json();
                    return body;
            })
            .catch(this.handleError);
    }

    public getProducts(): Observable<Filter[]> {
      
        let url = `/Dashboard.Rest/api/products`;
        return this.http.get(url, { headers: this.headers })
            .map(this.extractProducts)
            .catch(this.handleError);
    }


    public getView(id: number): Observable<ProductView> {
       
        let url = `/Dashboard.Rest/api/products/views/${id}`;  
        return this.http.get(url, { headers: this.headers })
            .map(this.extractSingleViewData)
            .catch(this.handleError);
    }

    private extractSingleViewData(res: Response): ProductView {
        let body: ProductView = res.json();
        return body;
    }

    private extractViewData(res: Response): ProductViewModel[] {
        let body: ProductViewModel[] = res.json();
        return body;
    }

    private extractProducts(res: Response): Filter[] {
        let body: Filter[] = res.json();
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




