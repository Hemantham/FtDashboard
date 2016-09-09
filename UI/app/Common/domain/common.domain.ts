// Find all classes with a name ending with Model
export class FieldSearchCriteria {
    constructor(public filters: ValueFilter[], public fieldOfInterest: string) {
    }
}

// Find all classes with a name ending with Model
export class ValueFilter {
    constructor(public code: string, public answer: string) {
    }
}
