/// <reference path="../../charts/domain/chart.domain.ts" />
// Find all classes with a name ending with Model

import {Question} from '../../charts/domain/chart.domain'

export class DashboardView {
    constructor(public Id: number, public Name: string, public Code: string, public FieldOfInterest: Question[], public XAxislable: string, public XAxisId: string, public Parent: DashboardView, public ViewOrder: number, public ChildrenViews: DashboardView[], public ChartRenderType: string, public DataAnlysisType: string, public ProductViews : FilteredDashboardView[]) {

    }
}

// Find all classes with a name ending with Model


// Find all classes with a name ending with Model
export class Filter {
    constructor(public Id:number, public Name: string, public Code: string, public FilteredDashboardViews: FilteredDashboardView[], public FilterString: string) {

    }
}

// Find all classes with a name ending with Model
export class FilteredDashboardView {
    constructor(public Id:number, public ViewSplits: ViewSplit[], public DashboardView: DashboardView, public Filter: Filter) {

    }
}
// Find all classes with a name ending with Model
export class ViewSplit {
    constructor(public Id:number, public SplitField: string, public SplitName: string, public Filter: Filter, public SplitType: string) {

    }
}

export class ProductViewModel {
    constructor(public Id: number, public DashboardView: DashboardView, public ProductId: number, public Children: ProductViewModel[]) {
    }
}

