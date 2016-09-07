 // Find all classes with a name ending with Model
 export  class DashboardView {
        constructor(public name: string, public code: string, public fieldOfInterest: string, public viewSplits: ViewSplit[], public products: Product[], public parent: DashboardView) {
        }
}

export class Filter {
    constructor(public filterString: string, public name: string) {
    }
}

export class Product {
    constructor(public name: string, public code: string, public dashboardViews: DashboardView[], public filter: Filter) {
    }
}

export class ViewSplit {
    constructor(public splitField: string, public splitName: string, public filter: Filter, public splitType: number) {
    }
}
