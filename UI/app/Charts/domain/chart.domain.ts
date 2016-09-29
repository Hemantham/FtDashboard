import {ViewSplit} from "../../Dashboards/domain/dashboard.domain";

export class ChartModel {
    name : string ;
    xAxislable: string;
    yAxislable: string;
    xAxisFormat: string;
    yAxisFormat: string;
    series: ChartSeriesModel[];
   // allSeries: Array<string>;
    recencies: Array<Recency>;
    chartRenderType: string;
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

export class ChartsContainerModel {
    constructor(public Charts: DataChart[], public AvailableRecencies: Recency[], public ChartRenderType: string) {
    }
}


export class ChartEntry {
   public  Value: number;
   public  XAxisLable: string;
   public  XAxisId: number;
   public  Series: string;
}


export class ChartSearchCriteria {
    constructor(public SelectedSplit: ViewSplit, public SplitFilters: string[], public ProductViewId: number, public RecencyType: number, public SelectedRecencies: Recency[],public  DashboardViewId : number) {
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

export class FieldValueModel {
    constructor(public IsSelected: boolean, public Id: number, public Code: string, public Answer: string, public QuestionId: number) {
    }
}

export class RecencyType {
    constructor(public RecencyTypes: number, public Name: string) {
    }
}

export class Recency {
    constructor(public RecencyNumber: number, public Lable: string) {

    }
}