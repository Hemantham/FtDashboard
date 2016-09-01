export class ChartModel {
    xAxislable: string;
    yAxislable: string;
    xAxisFormat: string;
    yAxisFormat: string;
    series: ChartSeriesModel[];
}

export class ChartSeriesModel {
    data: DataPointModel[];
    key: string;
    color: string;
}

export class DataPointModel {
    x: any;
    y: any;
}