using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Dashboard.API.API;
using Dashboard.API.Domain;
using Dashboard.API.Models;
using Dashboard.Models;
using DataEf.Context;

namespace Dashboard.Services
{
    public class ChartDataService : IChartDataService
    {
        private readonly EfUnitOfWork _unitOfWork;
        private readonly IDashboardService _dashboardService;

        public ChartDataService(EfUnitOfWork unitOfWork, IDashboardService dashboardService)
        {
            _unitOfWork = unitOfWork;
            _dashboardService = dashboardService;
        }

        
        //todo : performance
        public IEnumerable<DataChart> GetChartValues(ChartSearchCriteria criteria)
        {
            var productView = _dashboardService.GetProductView(criteria.ProductViewId);

            var filteredResponsesGroupes = FilterByProduct(productView);

            filteredResponsesGroupes = FilterByQuestions(filteredResponsesGroupes, criteria.SplitFilters);

            var splits_AllType = GetDistinctResponses(filteredResponsesGroupes);

            var splits_SelectiveTypeAnswers = criteria.SplitFilters;

            var splits_SelectiveTypeCode = productView.ViewSplits.FirstOrDefault(vs => vs.SplitType == SplitType.Mutiple).SplitField;

            var split_carteasians = from split1 in splits_AllType
                                    from split2 in splits_SelectiveTypeAnswers
                                    select new
                                    {
                                       split1 = new { split1.Answer, split1.Question.Code },
                                       split2 = new { Answer = split2, Code = splits_SelectiveTypeCode },
                                       Name = $"{split1.Answer} - {split2}"
                                    };

            var charts = new List<DataChart>();

            if (split_carteasians.Any())
            {
                foreach (var split_carteasian in split_carteasians)
                {
                    var chart = new DataChart();

                    filteredResponsesGroupes = filteredResponsesGroupes
                        .Where(frg =>
                            frg.Any(
                                res =>
                                    res.Answer == split_carteasian.split1.Answer &&
                                    res.Question.Code == split_carteasian.split1.Code) &&
                            frg.Any(
                                res =>
                                    res.Answer == split_carteasian.split2.Answer &&
                                    res.Question.Code == split_carteasian.split2.Code));

                    var chartValues = GetDataFieldsByPercentage(filteredResponsesGroupes, productView);

                    chart.ChartValues = chartValues;
                    chart.ChartName = split_carteasian.Name;
                    charts.Add(chart);

                }
            }
            else
            {
                var chart = new DataChart();

                if (productView.DashboardView.DataAnlysisType == DataAnlysisType.percentage)
                {
                    chart.ChartValues = GetDataFieldsByPercentage(filteredResponsesGroupes, productView);
                }
                else if (productView.DashboardView.DataAnlysisType == DataAnlysisType.avarage)
                {
                    chart.ChartValues = GetDataFieldsByAvarage(filteredResponsesGroupes, productView);
                }

                chart.ChartName = "Overall";
                charts.Add(chart);
            }
            return charts;
        }

        private static IEnumerable<ChartEntry> GetDataFieldsByPercentage(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, ProductView productView)
        {
            var fingleFieldOfInterest = productView.DashboardView.FieldOfInterest.FirstOrDefault()?.Code;
            var datafields = filteredResponsesGroupes.Select(rg =>
                    new DataPoint
                    {
                        Data = rg.FirstOrDefault(r => r.Question.Code == fingleFieldOfInterest)?.Answer,//select the field we are interested in for charting,
                        Id = rg.Key,
                        XAxisLable = rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.XAxislable)?.Answer,
                        XAxisId =
                        rg.Any(r => r.Question.Code == productView.DashboardView.XAxisId)
                            ? long.Parse(rg.First(r => r.Question.Code == productView.DashboardView.XAxisId).Answer)
                            : 0
                    })
                .GroupBy(d => d.XAxisId)
                .ToList();

            var chartValues = datafields.SelectMany(xg =>
                        xg.GroupBy(vg => vg.Data)
                            .Select(vg => new ChartEntry
                            {
                                Value = vg.Count()*100/xg.Count(),
                                XAxisLable = vg.Any() ? vg.First().XAxisLable : string.Empty,
                                XAxisId = vg.Any() ? vg.First().XAxisId : 0,
                                Series = vg.Key,
                            })
                )
                .OrderBy(df => df.XAxisId)
                .ToList();
            return chartValues;
        }

        private static IEnumerable<ChartEntry> GetDataFieldsByAvarage(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, ProductView productView)
        {

            var chartEntries = new List<ChartEntry>();

            //unpivot fields of interests
            foreach (var fieldOfInterest in productView.DashboardView.FieldOfInterest)
            {


                var xAxisGroups =
                    filteredResponsesGroupes
                        .Select(rg =>
                            new DataPoint
                            {
                                Data = rg.FirstOrDefault(r => r.Question.Code == fieldOfInterest.Code)?.Answer,
                                //select the field we are interested in for charting,
                                KeyCode = fieldOfInterest.Code,
                                KeyName = fieldOfInterest.Text,
                                Id = rg.Key,
                                XAxisLable =
                                    rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.XAxislable)?
                                        .Answer,
                                XAxisId =
                                    rg.Any(r => r.Question.Code == productView.DashboardView.XAxisId)
                                        ? long.Parse(
                                            rg.First(r => r.Question.Code == productView.DashboardView.XAxisId).Answer)
                                        : 0
                            })
                        .Where(d=> !string.IsNullOrWhiteSpace(d.Data))
                        .GroupBy(d => d.XAxisId)
                        .ToList();



                chartEntries.AddRange(xAxisGroups.SelectMany(xAxisGroup =>
                    xAxisGroup
                        .GroupBy(xg => xg.KeyCode)
                        .Select(vg => new ChartEntry
                        {
                            Value = vg.Sum(dp => int.Parse(dp.Data))/vg.Count(),
                            XAxisLable = vg.Any() ? vg.First().XAxisLable : string.Empty,
                            XAxisId = vg.Any() ? vg.First().XAxisId : 0,
                            Series = vg.Any() ? vg.First().KeyName : string.Empty,
                        })));

            }

            return chartEntries;
        }

        public IEnumerable<Response> GetFieldValues(FieldSearchCriteria criteria)
        {
            var productView = _dashboardService.GetProductView(criteria.ProductViewId);

            var filteredResponsesGroupes = FilterByProduct(productView);

            filteredResponsesGroupes = FilterByQuestions(filteredResponsesGroupes
                                                            , new[] { criteria.QuestionCode });

            return GetDistinctResponses(filteredResponsesGroupes);
        }

        private static IEnumerable<Response> GetDistinctResponses(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes)
        {
            return filteredResponsesGroupes
                .Select(rg => rg.First())
                .GroupBy(r => r.Answer)
                .Select(rg => rg.FirstOrDefault())
                .Where(r => r != null);
        }

        private static IEnumerable<IGrouping<string, Response>> FilterByQuestions(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, IEnumerable<string> questionCodes)
        {
            return filteredResponsesGroupes.Where(rg =>
                                                       questionCodes.All(qc =>
                                                                              rg.Any(r => r.Question.Code == qc))
            );
        }

        private IEnumerable<IGrouping<string, Response>> FilterByProduct(ProductView productView)
        {
            var productFilters = productView.Product?
                .Filter?
                .FilterString?
                .ToLower()
                .Split(new[] { "and" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    var comaprisons = s.Split('=');
                    return new
                    {
                        Code = comaprisons[0],
                        Answer = comaprisons[1],
                    };
                });

            var responses = _unitOfWork.GetRepository<Response>()
                .Include(response => response.Question)
                .Get();

            var responsesGroupes = responses.GroupBy(r => r.ResponseId); // create a group for each response 

            //within that response check if we have given filters. 
            //filter by the cuts/filters , etc
            var filteredResponsesGroupes = productFilters != null
                ? responsesGroupes
                    .Where(rg => productFilters.All(q => rg.Any(r => r.Question.Code == q.Code && r.Answer == q.Answer)))
                    .ToList()
                : responsesGroupes;
            return filteredResponsesGroupes;
        }
        
    }
}
