import * as Charts from '../domain/chart.domain'
import * as Enumerable from "linq-es2015";
import { Injectable }     from '@angular/core';
import { Http, Response, Request, Headers, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import '../../rxjs-operators'

@Injectable()
export class ChartValueService {
    constructor(private http: Http) {
        this.headersGet = new Headers();
        this.headersGet.append('Accept', 'application/json');

    }

    private headersGet: Headers;
    private headersPost: Headers;

   
    public getCharts(chartCriteria: Charts.ChartSearchCriteria): Observable<Charts.ChartsContainerModel> {
        
        let headers = new Headers({ 'Content-Type': 'application/json' });
        headers.append('Accept', 'application/json');
        let options = new RequestOptions({ headers: headers });

        return (this.http
            .post('/Dashboard.Rest/api/charts/data', JSON.stringify(chartCriteria), options)
            .map(this.extractChartData)
            .catch(this.handleError));
    }

    public getSplitFields(id: number): Observable<Charts.FieldValueModel[]> {
        
        return this.http
            .get(`/Dashboard.Rest/api/products/views/${id}/splitfilters`, { headers: this.headersGet })
            .map(this.extractFieldData)
            .catch(this.handleError);
    }

     public getRecencyTypes(): Observable<Charts.RecencyType[]> {
         
        return this.http
            .get(`/Dashboard.Rest/api/charts/recencytypes`, { headers: this.headersGet })
            .map((res: any) => {
                let body: Charts.RecencyType[] = res.json();
                return body;
            })
            .catch(this.handleError);
    }


    private extractFieldData(res: Response): Charts.FieldValueModel[] {
        let body: Charts.FieldValueModel[] = res.json();
        return body;
    }

    private extractChartData(res: Response): Charts.ChartModel[] {

        try {
            

        let chartContainerModel = res.json();
        let data: Array<Charts.DataChart> = chartContainerModel.Charts;
        let charts: Array<Charts.ChartModel> = new Array<Charts.ChartModel>();

          

        data.forEach(datachart => {

            let chartEntries: Charts.ChartEntry[] = datachart.ChartValues;
            let chartsList = Enumerable.asEnumerable(chartEntries);

                let chart = new Charts.ChartModel();

                chart.xAxislable = "This is X";
                chart.yAxislable = "This is Y";
                chart.recencies = chartContainerModel.AvailableRecencies;
                chart.allSeriesNames = chartContainerModel.AvailableSeries;
                chart.chartRenderType = chartContainerModel.ChartRenderType;
                chart.dataAnlysisType = chartContainerModel.DataAnlysisType;
                chart.name = datachart.ChartName;
                chart.series = chartsList.GroupBy(chartEntry => chartEntry.Series,
                        chartEntry => chartEntry,
                        (series, values) => {
                            let serieModel: Charts.ChartSeriesModel = new Charts.ChartSeriesModel();
                            serieModel.key = series;
                            serieModel.data = Enumerable
                                .asEnumerable(values)
                                .Select((datavalue) => {
                                    let seriesData: Charts.DataPointModel = new Charts.DataPointModel();
                                    seriesData.y = datavalue.Value;
                                    seriesData.x = datavalue.XAxisId;
                                    seriesData.label = datavalue.XAxisLable;
                                    seriesData.samples = datavalue.Samples;
                                    return seriesData;
                                })
                                .ToArray();
                            return serieModel;
                        })
                    .ToArray();

                charts.push(chart);
            }
        );



        return charts;
        }
        catch (e) {
            console.log('error in extractChartData');
        }
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




