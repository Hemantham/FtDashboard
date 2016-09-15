/// <reference path="../../charts/domain/chart.domain.ts" />
// Find all classes with a name ending with Model

import {Question} from '../../charts/domain/chart.domain'

export class DashboardView {
    constructor(public Id: number, public Name: string, public Code: string, public FieldOfInterest: Question[], public XAxislable: string, public XAxisId: string, public Parent: DashboardView, public ViewOrder: number, public ChildrenViews: DashboardView[], public ChartRenderType: string, public DataAnlysisType: string, public ProductViews : ProductView[]) {

    }
}

// Find all classes with a name ending with Model
export class Filter {
    constructor(public Id:number, public FilterString: string, public Name: string) {

    }
}

// Find all classes with a name ending with Model
export class Product {
    constructor(public Id:number, public Name: string, public Code: string, public ProductViews: ProductView[], public Filter: Filter) {

    }
}

// Find all classes with a name ending with Model
export class ProductView {
    constructor(public Id:number, public ViewSplits: ViewSplit[], public DashboardView: DashboardView, public Product: Product) {

    }
}
// Find all classes with a name ending with Model
export class ViewSplit {
    constructor(public Id:number, public SplitField: string, public SplitName: string, public Filter: Filter, public SplitType: string) {

    }
}

export class ProductViewModel {
    constructor(public id: number, public dashboardView: DashboardView, public productId: number, public children: ProductViewModel[]) {
    }
}