/// <reference path="../../charts/domain/chart.domain.ts" />
// Find all classes with a name ending with Model

import {Question} from '../../charts/domain/chart.domain'

export class DashboardView {
    constructor(public Name: string, public Code: string, public FieldOfInterest: Question[], public XAxislable: string, public XAxisId: string, public Parent: DashboardView, public ViewOrder: number, public ChildrenViews: DashboardView[], public ChartRenderType: string, public DataAnlysisType: string) {

    }
}

// Find all classes with a name ending with Model
export class Filter {
    constructor(public FilterString: string, public Name: string) {

    }
}

// Find all classes with a name ending with Model
export class Product {
    constructor(public Name: string, public Code: string, public ProductViews: ProductView[], public Filter: Filter) {

    }
}

// Find all classes with a name ending with Model
export class ProductView {
    constructor(public ViewSplits: ViewSplit[], public DashboardView: DashboardView, public Product: Product) {

    }
}
// Find all classes with a name ending with Model
export class ViewSplit {
    constructor(public SplitField: string, public SplitName: string, public Filter: Filter, public SplitType: string) {

    }
}
