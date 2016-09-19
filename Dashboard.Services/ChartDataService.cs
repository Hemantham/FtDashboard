using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Dashboard.API.API;
using Dashboard.API.Domain;
using Dashboard.API.Enums;
using Dashboard.API.Models;
using Dashboard.Models;
using DataEf.Context;
using Dashboard.Services.Utilities;
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
        public ChartsContainerModel GetChartsContainerModel(ChartSearchCriteria criteria)
        {

            var productView = _dashboardService.GetProductView(criteria.ProductViewId);

            var filteredResponsesGroupes = FilterByProduct(productView);

            IEnumerable<Response> splitsAllType = new List<Response> { null };

           // IEnumerable<Recency> availableRecencies = filteredResponsesGroupes.

            IEnumerable<string> splitsSelectiveTypeAnswers = new List<string> {null};

            string splitsSelectiveTypeCode = null;

            if (criteria.SelectedSplit != null && criteria.SelectedSplit.Id > 0)
            {
                var selectedSplit = _dashboardService.GetViewSplit(criteria.SelectedSplit.Id);

                var distinctResponses = GetDistinctResponses(filteredResponsesGroupes, selectedSplit.Question.Code);

                if (distinctResponses.Any())
                {
                    splitsAllType = distinctResponses;
                }
            }

            if (criteria.SplitFilters != null && criteria.SplitFilters.Any())
            {
                splitsSelectiveTypeAnswers = criteria.SplitFilters;
                splitsSelectiveTypeCode = productView.ViewSplits.FirstOrDefault(vs => vs.SplitType == SplitType.Multiple)?.Question.Code;
                filteredResponsesGroupes = FilterByQuestionAnswers(filteredResponsesGroupes, criteria.SplitFilters, splitsSelectiveTypeCode);
            }
           
            var  split_carteasians =   (from split1 in splitsAllType
                                        from split2 in splitsSelectiveTypeAnswers
                                        select new
                                        {
                                        split1 = split1 != null ? new { split1.Answer, split1.Question.Code } : null,
                                        split2 = ( split2 != null && splitsSelectiveTypeCode != null ) ? new { Answer = split2, Code = splitsSelectiveTypeCode } : null,
                                        Name =  $"{(split1  != null ? split1.Answer : string.Empty)} - { (split2 != null && splitsSelectiveTypeCode != null ? split2 : String.Empty)}".Trim('-',' ')
                                        }).Distinct();
            

            var charts = new List<DataChart>();

            if ( split_carteasians.Any(c=> c.split1 != null || c.split2 != null))
            {
                foreach (var split_carteasian in split_carteasians)
                {
                    var chart = new DataChart();

                   var  filteredResponsesGroupesForSplit = filteredResponsesGroupes
                        .Where(frg =>
                            frg.Any(
                                res => split_carteasian.split1 == null ||
                                    (res.Answer == split_carteasian.split1.Answer && res.Question.Code == split_carteasian.split1.Code)) &&
                            frg.Any(
                                res => split_carteasian.split2 == null ||
                                    (res.Answer == split_carteasian.split2.Answer && res.Question.Code == split_carteasian.split2.Code)));

                    var chartValues = GetDataFieldsByPercentage(filteredResponsesGroupesForSplit, productView, criteria);

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
                    chart.ChartValues = GetDataFieldsByPercentage(filteredResponsesGroupes, productView, criteria);
                }
                else if (productView.DashboardView.DataAnlysisType == DataAnlysisType.avarage)
                {
                    chart.ChartValues = GetDataFieldsByAvarage(filteredResponsesGroupes, productView, criteria);
                }

                chart.ChartName = "Overall";
                charts.Add(chart);
            }

            
            return new ChartsContainerModel
            {
                Charts = charts,
                AvailableRecencies = charts.SelectMany(c=> c.ChartValues.Select(cv=> new Recency
                {
                    RecencyNumber = cv.XAxisId,
                    Lable = cv.XAxisLable,

                }))
                .GroupBy(r=>r.RecencyNumber).Select(r=> r.First())

            }; ;
        }

        public IEnumerable<RecencyType> GetRecencyTypes()
        {
           return  _unitOfWork.GetRepository<RecencyType>().Get();
        }

        //public IEnumerable<Recency> GetRecencyPoints()
        //{
        //    return _unitOfWork.GetRepository<RecencyType>().Get();
        //}


        public IEnumerable<FieldValueModel> GetFieldValues(int productViewId)
        {
            var productView = _dashboardService.GetProductView(productViewId);

            var filteredResponsesGroupes = FilterByProduct(productView);

            return GetDistinctResponses(filteredResponsesGroupes, 
                                productView.ViewSplits.FirstOrDefault(vs => vs.SplitType == SplitType.Multiple)?.Question?.Code)
                                .Select((r,i)=> new FieldValueModel
                    {
                        Id = r.Id,
                        Code = r.Question.Code,
                        Answer = r.Answer,
                        QuestionId = r.Question.Id,
                        IsSelected = i < 2
                    })
                                ;
        }

        private static IEnumerable<ChartEntry> GetDataFieldsByPercentage(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, ProductView productView, ChartSearchCriteria criteria)
        {
            var fingleFieldOfInterest = productView.DashboardView.FieldOfInterest.FirstOrDefault()?.Code;
             
            var datafields = filteredResponsesGroupes.Select(rg =>
                {
                    var responseFoi = rg.FirstOrDefault(r => r.Question.Code == fingleFieldOfInterest);

                    var responseXAxisId = 0;
                    var responseXAxislabel = string.Empty;

                    if (productView.DashboardView.XAxisId != null) // is dashboardview provides the XAxis, pick ffrom that
                    {
                         responseXAxisId = int.Parse(rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.XAxisId)?.Answer ?? "0");
                         responseXAxislabel = rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.XAxislable)?.Answer;
                    }
                    if (responseXAxisId == 0) // else pick from completion date
                    {
                        var recency = responseFoi?.CompletionDate.GetRecency(criteria.RecencyType) ?? new Recency();
                        responseXAxisId = recency.RecencyNumber;
                        responseXAxislabel = recency.Lable;
                    }
                    return new DataPoint
                    {
                        Data = responseFoi?.Answer,
                        CompletionDate = responseFoi?.CompletionDate?? DateTime.MinValue,
                        //select the field we are interested in for charting,

                        Id = rg.Key,
                        XAxisLable = responseXAxislabel,
                        XAxisId = responseXAxisId

                    };
                })
                .Where(d=> d.Data != null && (criteria.SelectedRecencies == null ||
                                             !criteria.SelectedRecencies.Any()||
                                              criteria.SelectedRecencies.Any( r => d.XAxisId == r.RecencyNumber)))
                .GroupBy(d => d.XAxisId) // group the FOI by XAxis
                .ToList();

            var chartValues = datafields.SelectMany(xg =>
                        xg.GroupBy(vg => vg.Data) // group the XAxis data by FOI and calculate the average for each
                            .Select(vg =>
                            {
                                var responseRow = vg.FirstOrDefault();
                                return new ChartEntry
                                {
                                    Value = vg.Count()*100/xg.Count(),
                                    XAxisLable = responseRow?.XAxisLable ,
                                    XAxisId = responseRow?.XAxisId ?? 0 ,
                                    Series = vg.Key,
                                };
                            })
                )
                .OrderBy(df => df.XAxisId)
                .ToList();
            return chartValues;
        }

        private static IEnumerable<ChartEntry> GetDataFieldsByAvarage(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, ProductView productView, ChartSearchCriteria criteria)
        {

            var chartEntries = new List<ChartEntry>();

            //unpivot fields of interests
            foreach (var fieldOfInterest in productView.DashboardView.FieldOfInterest)
            {

                var xAxisGroups =
                    filteredResponsesGroupes
                        .Select(rg =>
                        {
                            var responseFoi = rg.FirstOrDefault(r => r.Question.Code == fieldOfInterest.Code);
                            return new DataPoint
                            {
                                Data = responseFoi?.Answer,
                                CompletionDate = responseFoi?.CompletionDate ?? DateTime.MinValue,
                                //select the field we are interested in for charting,
                                KeyCode = fieldOfInterest.Code,
                                KeyName = fieldOfInterest.Text,
                                Id = rg.Key,
                                XAxisLable =
                                    rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.XAxislable)?
                                        .Answer,
                                XAxisId =
                                    rg.Any(r => r.Question.Code == productView.DashboardView.XAxisId)
                                        ? int.Parse(
                                            rg.First(r => r.Question.Code == productView.DashboardView.XAxisId).Answer)
                                        : 0
                            };
                        })
                        .Where(d=> !string.IsNullOrWhiteSpace(d.Data))
                        .GroupBy(d => d.XAxisId)
                        .ToList();

                chartEntries.AddRange(xAxisGroups.SelectMany(xAxisGroup =>
                    xAxisGroup
                        .GroupBy(xg => xg.KeyCode)
                        .Select(vg =>
                        {
                            var responseRow = vg.FirstOrDefault();
                            var recency = responseRow?.CompletionDate.GetRecency(criteria.RecencyType) ?? new Recency();
                            return new ChartEntry
                            {
                                Value = vg.Sum(dp => int.Parse(dp.Data))/vg.Count(),
                                XAxisLable = responseRow?.XAxisLable ?? recency.Lable,
                                XAxisId = responseRow?.XAxisId ?? recency.RecencyNumber,
                                Series = vg.Any() ? vg.First().KeyName : string.Empty,
                            };
                        })));

            }

            return chartEntries;
        }
        
        private static IEnumerable<Response> GetDistinctResponses(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, string code)
        {

            return filteredResponsesGroupes
                .SelectMany(rg => rg.Where( r=>  r.Question.Code == code) )
                .GroupBy(r => r.Answer)
                .Select(rg => rg.FirstOrDefault())
                .Where(r => r != null);
        }

        //private static IEnumerable<Response> GetDistinctResponses(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes)
        //{
        //    return filteredResponsesGroupes
        //        .SelectMany(rg => rg)
        //        .GroupBy(r => r.Answer)
        //        .Select(rg => rg.FirstOrDefault())
        //        .Where(r => r != null);
        //}

        //private static IEnumerable<IGrouping<string, Response>> FilterByQuestions(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, IEnumerable<string> questionCodes)
        //{
        //    return filteredResponsesGroupes.Where(rg =>
        //                                               questionCodes.All(qc =>
        //                                                                      rg.Any(r => r.Question.Code == qc))
        //    );
        //}

        private static IEnumerable<IGrouping<string, Response>> FilterByQuestionAnswers(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, IEnumerable<string> responses, string code)
        {
            return filteredResponsesGroupes.Where(rg => !responses.Any() ||
                                                        string.IsNullOrWhiteSpace(code) ||
                                                        responses.Any(rs => rg.Any(r => r.Question.Code == code && r.Answer == rs)));
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

            var responsesGroupes = responses.GroupBy(r => r.ResponseId ); // create a group for each response 

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
