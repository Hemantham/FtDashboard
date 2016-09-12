export class DashboardView {
    constructor(public name: string, public code: string, public fieldOfInterest: string, public parent: DashboardView, public viewOrder: number) {
    }
}


export class Filter {
    constructor(public filterString: string, public name: string) {
    }
}

export class Product {
    constructor(public name: string, public code: string, public productViews: ProductView[], public filter: Filter) {
    }
}

export class ProductView {
    constructor(public viewSplits: ViewSplit[], public dashboardView: DashboardView, public product: Product) {
    }
}

export class ViewSplit {
    constructor(public splitField: string, public splitName: string, public filter: Filter, public splitType: string) {
    }
}