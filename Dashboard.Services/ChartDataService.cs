using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Dashboard.API.API;
using Dashboard.API.API.Cache;
using Dashboard.API.Domain;
using Dashboard.API.Enums;
using Dashboard.API.Models;
using Dashboard.Models;
using Dashboard.Services.Cache;
using DataEf.Context;
using Dashboard.Services.Utilities;
namespace Dashboard.Services
{
    public class ChartDataService : IChartDataService
    {
        private readonly EfUnitOfWork _unitOfWork;
        private readonly IDashboardService _dashboardService;
        private ICacheProvider _cacheProvider;

        public ChartDataService(EfUnitOfWork unitOfWork, IDashboardService dashboardService)
        {
            _unitOfWork = unitOfWork;
            _dashboardService = dashboardService;
            _cacheProvider = MemCachProvider.Instance;
        }

        public ChartsContainerModel GetChartsContainerModelForMultipleFilters(ChartSearchCriteria criteria)
        {

            var allCharts = new List<DataChart>();

            FilteredDashboardView productView = null;

            var filterbasedSplits =
                criteria.SplitCriteria.Where(c => c.SplitType == SplitType.FilterBasedMultiple)
                    .SelectMany(c => c.SplitFilters).ToList();

            foreach (var product in _dashboardService.GetFilters().Where(p => !filterbasedSplits.Any() ||
                                                                                filterbasedSplits.Any(c => (long)c == p.Id)))
            {
                productView = _dashboardService.GetFilter(product.Id)
                 .FilteredDashboardViews
                 .FirstOrDefault(pv =>
                             pv.DashboardView.Id == criteria.DashboardViewId && pv.Filter.Id == product.Id);

                var charts = GetCharts(new ChartSearchCriteria
                {
                    RecencyType = criteria.RecencyType,
                    FilteredDashboardViewId = productView?.Id ?? 0,
                    SelectedRecencies = criteria.SelectedRecencies,
                    UseFilterName = true,
                    OutputFilters = criteria.OutputFilters,
                }, productView);
                allCharts.AddRange(charts);
            }

            var allSeries = GetAllSeries(allCharts);

           

            return new ChartsContainerModel
            {
                Charts = allCharts,
                AvailableRecencies = allCharts.SelectMany(c => c.ChartValues.Select(cv => new XAxis
                {
                    RecencyNumber = cv.XAxisId,
                    Lable = cv.XAxisLable,

                }))
                .GroupBy(r => r.RecencyNumber)
                .Select(r => r.First())
                .OrderBy(r=> r.RecencyNumber),
               // .Select((r, i) => new XAxis { RecencyNumber = i, Lable = r.Lable }),
                AvailableSeries = allSeries,
                ChartRenderType = productView?.DashboardView?.ChartRenderType ?? ChartRenderType.line,
                DataAnlysisType = productView?.DashboardView?.DataAnlysisType ?? DataAnlysisType.percentage,
            };
        }

        private IEnumerable<string> GetAllSeries(List<DataChart> allCharts)
        {
            return allCharts.SelectMany(c => c.ChartValues.Select(cv => cv.Series)).Distinct().ToList();
        }

        private List<ChartEntry> ApplyOutputFilters(ChartSearchCriteria criteria, List<ChartEntry> allCharts)
        {
            // apply output filters
           
            return allCharts
                    .Where(
                        cv => ( criteria.OutputFilters == null || !criteria.OutputFilters.Any() || criteria.OutputFilters.Any(f => cv.Series == f)))
                        .ToList();
           
        }

        //todo : performance
        public ChartsContainerModel GetChartsContainerModel(ChartSearchCriteria criteria)
        {

            var productView = _dashboardService.GetFilteredView(criteria.FilteredDashboardViewId);

            var charts = GetCharts(criteria, productView);

            var allSeries = GetAllSeries(charts);

           

            return new ChartsContainerModel
            {
                Charts = charts,
                AvailableRecencies = charts.SelectMany(c => c.ChartValues.Select(cv => new XAxis
                {
                    RecencyNumber = cv.XAxisId,
                    Lable = cv.XAxisLable,

                }))
                .GroupBy(r => r.RecencyNumber)
                .Select(r => r.First())
                .OrderBy(r => r.RecencyNumber),
                // .Select((r, i)=> new XAxis { RecencyNumber = i, Lable =  r.Lable}),
                AvailableSeries = allSeries,
                ChartRenderType = productView.DashboardView.ChartRenderType,
                DataAnlysisType = productView?.DashboardView?.DataAnlysisType ?? DataAnlysisType.percentage,

            };
        }

        private List<DataChart> GetCharts(ChartSearchCriteria criteria, FilteredDashboardView productView)
        {
            var charts = new List<DataChart>();

            var filteredResponsesGroupes = FilterByProduct(productView);

            IEnumerable<Response> splitsAllType = new List<Response> { null };

            IEnumerable<string> splitsSelectiveTypeAnswers = new List<string> { null };

            string splitsSelectiveTypeCode = null;

            var allSplit = criteria.SplitCriteria?.FirstOrDefault(c => c.SplitType == SplitType.All && c.ViewSplitId > 0);

            var multipleSplits = criteria.SplitCriteria?.Where(c => c.SplitType == SplitType.Multiple)
                                                        .SelectMany(c => c.SplitFilters)
                                                        .Select(sf => sf.ToString()).ToList();


            if (allSplit != null)
            {
                var selectedSplit = _dashboardService.GetViewSplit(allSplit.ViewSplitId);

                var distinctResponses = GetDistinctResponses(filteredResponsesGroupes, selectedSplit.Question.Code);

                if (distinctResponses.Any())
                {
                    splitsAllType = distinctResponses;
                }
            }

            if (multipleSplits != null && multipleSplits.Any())
            {
                splitsSelectiveTypeAnswers = multipleSplits;
                splitsSelectiveTypeCode =
                    productView.ViewSplits.FirstOrDefault(vs => vs.SplitType == SplitType.Multiple)?.Question.Code;
                filteredResponsesGroupes = FilterByQuestionAnswers(filteredResponsesGroupes, multipleSplits,
                    splitsSelectiveTypeCode);
            }

            var split_carteasians = (from split1 in splitsAllType
                                     from split2 in splitsSelectiveTypeAnswers
                                     select new
                                     {
                                         split1 = split1 != null ? new { split1.Answer, split1.Question.Code } : null,
                                         split2 =
                                         (split2 != null && splitsSelectiveTypeCode != null)
                                             ? new { Answer = split2, Code = splitsSelectiveTypeCode }
                                             : null,
                                         Name =
                                         $"{(split1 != null ? split1.Answer : string.Empty)} - {(split2 != null && splitsSelectiveTypeCode != null ? split2 : String.Empty)}"
                                             .Trim('-', ' ')
                                     }).Distinct();


            if (split_carteasians.Any(c => c.split1 != null || c.split2 != null))
            {
                foreach (var split_carteasian in split_carteasians)
                {
                    var chart = new DataChart();

                    var filteredResponsesGroupesForSplit = filteredResponsesGroupes
                        .Where(frg =>
                            frg.Any(
                                res => split_carteasian.split1 == null ||
                                       (res.Answer.Equals(split_carteasian.split1.Answer, StringComparison.InvariantCultureIgnoreCase) &&
                                        res.Question.Code.Equals(split_carteasian.split1.Code, StringComparison.InvariantCultureIgnoreCase))) &&
                            frg.Any(
                                res => split_carteasian.split2 == null ||
                                       (res.Answer.Equals(split_carteasian.split2.Answer, StringComparison.InvariantCultureIgnoreCase) &&
                                        res.Question.Code.Equals(split_carteasian.split2.Code, StringComparison.InvariantCultureIgnoreCase))));

                    //GetByAnalysis 
                    chart.ChartValues = GetByAnalysisType(criteria, productView, filteredResponsesGroupesForSplit);

                    chart.ChartName = split_carteasian.Name;

                    charts.Add(chart);
                }
            }
            else
            {
                var chart = new DataChart();

                chart.ChartValues = GetByAnalysisType(criteria, productView, filteredResponsesGroupes);

                chart.ChartName = criteria.UseFilterName ? productView.Filter.Name : "Overall";

                charts.Add(chart);
            }



            return charts;
        }

        public IEnumerable<RecencyType> GetRecencyTypes()
        {
            return _unitOfWork.GetRepository<RecencyType>().Get();
        }

        public IEnumerable<FieldValueModel> GetFieldValues(int productViewId)
        {
            var productView = _dashboardService.GetFilteredView(productViewId);

            var filteredResponsesGroupes = FilterByProduct(productView);

            return GetDistinctResponses(filteredResponsesGroupes,
                                productView.ViewSplits.FirstOrDefault(vs => vs.SplitType == SplitType.Multiple)?.Question?.Code)
                                .Select((r, i) => new FieldValueModel
                                {
                                    Id = r.Id,
                                    Code = r.Question.Code,
                                    Answer = r.Answer,
                                    QuestionId = r.Question.Id,
                                    IsSelected = i < 2
                                })
                                ;
        }

        private IEnumerable<ChartEntry> GetByAnalysisType(ChartSearchCriteria criteria
        , FilteredDashboardView productView
        , IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes)
            {
                List<ChartEntry> analised = null;

                if (productView.DashboardView.DataAnlysisType == DataAnlysisType.percentage)
                {
                    analised =  GetDataFieldsByPercentage(filteredResponsesGroupes, productView, criteria);
                }
                else if (productView.DashboardView.DataAnlysisType == DataAnlysisType.avarage)
                {
                    analised = GetDataFieldsByAvarage(filteredResponsesGroupes, productView, criteria);
                }
                else if (productView.DashboardView.DataAnlysisType == DataAnlysisType.percentageAndAverage)
                {
                    analised = GetDataFieldsByAvarage(filteredResponsesGroupes, productView, criteria)
                       .Union(GetDataFieldsByPercentage(filteredResponsesGroupes, productView, criteria)).ToList();
                }

                analised = ApplyOutputFilters(criteria, analised);

                analised = AppendNulls(analised);

                return analised;
            }

        private List<ChartEntry> AppendNulls(List<ChartEntry> analised)
        {
            var uniqueXvalues = analised.Select(e => e.XAxisId).Distinct().ToList();

            foreach (var seriesGroup in analised.GroupBy(e => e.Series))
            {
                analised.AddRange(uniqueXvalues.Where(x => !seriesGroup.Any(e => e.XAxisId == x)).Select(x => new ChartEntry
                {
                    Series = seriesGroup.Key,
                    XAxisId = x,
                }));
            }

            return  analised.OrderBy(a => a.XAxisId).ToList();
        }

        private string GetRange(string value, IEnumerable<Range> ranges)
        {
            var valueParsed = float.Parse(value);
            return ranges.Where(r => r.Start <= valueParsed && r.End >= valueParsed)
                            .Select(r => $"{r.Text}({r.Start} - {r.End})")
                            .FirstOrDefault();
        }

        private IList<Range> GetRanges(string rangeText)
        {
            if (string.IsNullOrWhiteSpace(rangeText))
            {
                return null;
            }

            //"Unlikely:0-5|Neutral:6-7|Likely:8-10
            return rangeText.Split('|')
                .Select(r => new Range
                {
                    Text = r.Split(':')[0],
                    Start = float.Parse(r.Split(':')[1].Split('-')[0].Trim()),
                    End = float.Parse(r.Split(':')[1].Split('-')[1].Trim())
                }).ToList();
        }

        private List<ChartEntry> GetDataFieldsByPercentage(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, FilteredDashboardView productView, ChartSearchCriteria criteria)
        {
            var fingleFieldOfInterest = productView.DashboardView.FieldOfInterest.FirstOrDefault()?.Code;

            var dataRanges = GetRanges(productView.DashboardView.ChartRanges);

            var datafields = filteredResponsesGroupes.Select(rg =>
                {
                    var responseFoi = rg.FirstOrDefault(r => r.Question.Code.Equals(fingleFieldOfInterest, StringComparison.InvariantCultureIgnoreCase));

                    var responseXAxis = GetResponseXAxis(productView, criteria, rg, responseFoi);

                    return new DataPoint
                    {
                        Data = dataRanges != null && dataRanges.Any() ? GetRange(responseFoi?.Answer, dataRanges) : responseFoi?.Answer,
                        CompletionDate = responseFoi?.CompletionDate ?? DateTime.MinValue,
                        //select the field we are interested in for charting,

                        Id = rg.Key,
                        XAxisLable = responseXAxis.Lable,
                        XAxisId = responseXAxis.RecencyNumber

                    };
                })
                .Where(d => d.Data != null && (criteria.SelectedRecencies == null ||
                                             !criteria.SelectedRecencies.Any() ||
                                              criteria.SelectedRecencies.Any(r => d.XAxisId == r.RecencyNumber)))
                .GroupBy(d => d.XAxisId)
                .OrderByDescending(d => d.Key)
                .Take(criteria.LastNXAxis)
                // group the FOI by XAxis
                .ToList();

            var chartValues = datafields.SelectMany(xg =>
                        xg.GroupBy(vg => vg.Data) // group the XAxis data by FOI and calculate the average for each
                            .Select(vg =>
                            {
                                var responseRow = vg.FirstOrDefault();
                                return new ChartEntry
                                {
                                    Value = vg.Count() * 100f / xg.Count(),
                                    XAxisLable = responseRow?.XAxisLable,
                                    XAxisId = responseRow?.XAxisId ?? 0,
                                    Series = vg.Key,
                                    Samples = xg.Count(),
                                };
                            })
                )
                .OrderBy(df => df.XAxisId)
                .ToList();
            return chartValues;
        }

        private static XAxis GetResponseXAxis(FilteredDashboardView productView, ChartSearchCriteria criteria, IGrouping<string, Response> rg,
            Response responseFoi)
        {
            var responseXAxisId = 0L;
            var responseXAxislabel = string.Empty;

            if (productView.DashboardView.XAxisId != null) // is dashboardview provides the XAxis, pick ffrom that
            {
                responseXAxisId = int.Parse(rg.FirstOrDefault(r => r.Question.Code.Equals(productView.DashboardView.XAxisId, StringComparison.InvariantCultureIgnoreCase))?.Answer ?? "0");
                responseXAxislabel = rg.FirstOrDefault(r => r.Question.Code.Equals(productView.DashboardView.XAxislable, StringComparison.InvariantCultureIgnoreCase))?.Answer;
            }
            if (responseXAxisId == 0) // else pick from completion date
            {
                var recency = responseFoi?.CompletionDate.GetRecency(criteria.RecencyType) ?? new XAxis();
                responseXAxisId = recency.RecencyNumber;
                responseXAxislabel = recency.Lable;
            }
            return new XAxis
            {
                RecencyNumber = responseXAxisId,
                Lable = responseXAxislabel,
            };
        }

        private static List<ChartEntry> GetDataFieldsByAvarage(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, FilteredDashboardView productView, ChartSearchCriteria criteria)
        {

            var chartEntries = new List<ChartEntry>();

            //unpivot fields of interests
            foreach (var fieldOfInterest in productView.DashboardView.FieldOfInterest)
            {

                var xAxisGroups =
                    filteredResponsesGroupes
                        .Select(rg =>
                        {
                            var responseFoi = rg.FirstOrDefault(r => r.Question.Code.Equals(fieldOfInterest.Code, StringComparison.InvariantCultureIgnoreCase));

                            var responseXAxis = GetResponseXAxis(productView, criteria, rg, responseFoi);

                            return new DataPoint
                            {
                                Data = responseFoi?.Answer,
                                CompletionDate = responseFoi?.CompletionDate ?? DateTime.MinValue,
                                //select the field we are interested in for charting,
                                KeyCode = fieldOfInterest.Code,
                                KeyName = fieldOfInterest.Text,
                                Id = rg.Key,
                                XAxisLable = responseXAxis.Lable,
                                XAxisId = responseXAxis.RecencyNumber,
                            };
                        })
                      .Where(d => !string.IsNullOrWhiteSpace(d.Data) &&
                                             (criteria.SelectedRecencies == null ||
                                             !criteria.SelectedRecencies.Any() ||
                                              criteria.SelectedRecencies.Any(r => d.XAxisId == r.RecencyNumber)))
                      .GroupBy(d => d.XAxisId)
                      .OrderByDescending(d=> d.Key)
                      .Take(criteria.LastNXAxis)
                      .ToList();

                chartEntries.AddRange(xAxisGroups.SelectMany(xAxisGroup =>
                    xAxisGroup
                        .GroupBy(xg => xg.KeyCode)
                        .Select(vg =>
                        {
                            var responseRow = vg.FirstOrDefault();
                            var recency = responseRow?.CompletionDate.GetRecency(criteria.RecencyType) ?? new XAxis();
                            return new ChartEntry
                            {
                                Value = vg.Sum(dp => double.Parse(dp.Data)) / vg.Count(),
                                XAxisLable = responseRow?.XAxisLable ?? recency.Lable,
                                XAxisId = responseRow?.XAxisId ?? recency.RecencyNumber,
                                Series = vg.Any() ? vg.First().KeyName : string.Empty,
                                Samples = xAxisGroup.Count(),
                            };
                        })).OrderBy(df => df.XAxisId)
                        );

            }

            return chartEntries;
        }

        private static IEnumerable<Response> GetDistinctResponses(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, string code)
        {

            return filteredResponsesGroupes
                .SelectMany(rg => rg.Where(r => r.Question.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)))
                .GroupBy(r => r.Answer)
                .Select(rg => rg.FirstOrDefault())
                .Where(r => r != null);
        }

        private static IEnumerable<IGrouping<string, Response>> FilterByQuestionAnswers(IEnumerable<IGrouping<string, Response>> filteredResponsesGroupes, IEnumerable<string> responses, string code)
        {
            return filteredResponsesGroupes.Where(rg => !responses.Any() ||
                                                        string.IsNullOrWhiteSpace(code) ||
                                                        responses.Any(rs => rg.Any(r => r.Question.Code == code && r.Answer == rs)));
        }

        private IEnumerable<IGrouping<string, Response>> FilterByProduct(FilteredDashboardView productView)
        {
           

            var filteredResponsesGroupes =
                _cacheProvider.GetItem<IList<IGrouping<string, Response>>>($"data-filterdview-{productView.Id}");

            if (filteredResponsesGroupes != null)
            {
                return filteredResponsesGroupes.ToList();
            }

            var productFilters = productView.Filter?
                .FilterString?
                .ToLower()
                .Split(new[] { "and" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    var comaprisons = s?.Split('=');
                    return new
                    {
                        Code = comaprisons?[0]?.Trim(' ', '\''),
                        Answer = comaprisons?[1]?.Trim(' ', '\''),
                    };
                }).ToList();

            var responsesGroupes =
                _cacheProvider.GetItemAndAdd<IList<IGrouping<string, Response>>>("data-all", () =>
                {
                    return _unitOfWork.GetRepository<Response>()
                        .Include(response => response.Question)
                        .Get(null, null, true)
                        .GroupBy(r => r.ResponseId)
                        .ToList();
                });
             

            //within that response check if we have given filters. 
            //filter by the cuts/filters , etc
            filteredResponsesGroupes = productFilters != null
               ? responsesGroupes
                   .Where(rg => productFilters.All(q =>
                                                   rg.Any(r => r.Question.Code.Equals(q.Code, StringComparison.InvariantCultureIgnoreCase)
                                                               && r.Answer.Equals(q.Answer, StringComparison.InvariantCultureIgnoreCase))))
                                                   .ToList()
               : responsesGroupes;


            _cacheProvider.AddItem($"data-filterdview-{productView.Id}", filteredResponsesGroupes.ToList());

            return filteredResponsesGroupes;
        }

    }
}
