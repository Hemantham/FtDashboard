export class ChartModel {
    public xAxislable: string;
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
    label: string;
}

export class ChartEntry {
    Value: number;
    XAxisLable: string;
    XAxisValue: any;
    XAxisId: number;
    Series: string;
}

export class ChartFilter {

    constructor(code: string, answer : string) {
        
            this.Code = code;
            this.Answer = answer;
    }

    public Code: string;
    public Answer: string;
}
export class ChartSearchCriteria {
    public  Filters : ChartFilter[];
    public  XAxislable : string ;
    public  XAxisId: string ;
    public  FieldOfInterest: string;

}
