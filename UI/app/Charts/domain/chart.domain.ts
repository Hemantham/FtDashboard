import {ViewSplit} from "../../Dashboards/domain/dashboard.domain";

export class ChartModel {
    name : string ;
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
    label: string;
}

// Find all classes with a name ending with Model
export class DataChart {
    constructor(public ChartName: string, public ChartValues: ChartEntry[]) {

    }
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
    constructor(public SelectedSplit: ViewSplit, public SplitFilters: string[], public ProductViewId: number) {
    }
}
// Find all classes with a name ending with Model
export class FieldSearchCriteria {
    constructor(public ProductViewId: number, public CuestionCode: string) {
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