import {ChartModel, ChartSeriesModel, DataPointModel, ChartEntry , ChartSearchCriteria} from '../domain/chart.domain'
import * as Enumerable from "linq-es2015";

import { Injectable }     from '@angular/core';
import { Http, Response, Request, Headers, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import '../../rxjs-operators'

@Injectable()
export class ChartValueService {
    constructor(private http: Http) { }

    private heroesUrl = 'http://localhost/Dashboard.Rest/api/ChartValues';  // URL to web API

    getCharts(chartCriteria: ChartSearchCriteria): Observable<ChartEntry[]> {
        
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http
            .post(this.heroesUrl,JSON.stringify({ chartCriteria }),options)
            .map(this.extractData)
            .catch(this.handleError);
    }

    private extractData(res: Response): ChartModel {
        let body: ChartEntry[] = res.json();

        let chart: ChartModel = new ChartModel();

        chart.xAxislable = "This is X";
        chart.yAxislable = "This is Y";

        var chartsList = Enumerable.asEnumerable(body);

        chart.series = chartsList.GroupBy(chartEntry => chartEntry.Series, chartEntry => chartEntry,
            (series, values) => {
                let serieModel: ChartSeriesModel = new ChartSeriesModel();
                serieModel.key = series;
                serieModel.data = Enumerable
                                    .asEnumerable(values)
                                    .Select((datavalue) => {
                                        let seriesData: DataPointModel = new DataPointModel();
                                        seriesData.x = datavalue.Value;
                                        seriesData.label = datavalue.XAxisLable;
                                        return seriesData;
                                            })
                    .ToArray();
                return serieModel;
            }).ToArray();

        return chart;
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




