import {ViewSplit} from "../../Dashboards/domain/dashboard.domain";

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

//export class ChartFilter {

//    constructor(code: string, answer : string) {
        
//            this.Code = code;
//            this.Answer = answer;
//    }

//    public Code: string;
//    public Answer: string;
//}

export class ChartSearchCriteria {
    constructor(public selectedSplit: ViewSplit, public splitFilters: string[], public productViewId: number) {
    }
}
// Find all classes with a name ending with Model
export class FieldSearchCriteria {
    constructor(public productViewId: number, public questionCode: string) {
    }
}




export class Response {
    constructor(public responseType: string, public responseId: string, public inputId: number, public email: string, public completionDate: Date, public answer: string, public question: Question) {
    }
}
export class Question {
    constructor(public code: string, public text: string, public questionGroup: QuestionGroup) {
    }
}

export class QuestionGroup {
    constructor(public code: string, public text: string) {
    }
}