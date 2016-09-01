import {ChartModel, ChartSeriesModel, DataPointModel} from '../domain/chart.domain'

export class ChartService {
    getChart(): ChartModel {
        return new ChartModel();
    }
}