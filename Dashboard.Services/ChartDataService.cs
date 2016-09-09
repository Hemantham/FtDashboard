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


        public IEnumerable<ChartEntry> GetCharts(ChartSearchCriteria criteria)
        {
            GetChartValues(criteria)

        }

        //todo : performance
        public IEnumerable<DataChart> GetChartValues(ChartSearchCriteria criteria)
        {
            var productView = _dashboardService.GetProductView(criteria.ProductViewId);

            var filteredResponsesGroupes = FilterByProduct(productView);

            var splits_AllType = GetDistinctResponses(filteredResponsesGroupes);

            var splits_SelectiveTypeAnswers = criteria.SplitFilters;

            var splits_SelectiveTypeCode = productView.ViewSplits.FirstOrDefault(vs => vs.SplitType == SplitType.Mutiple);

            filteredResponsesGroupes = FilterByQuestions(filteredResponsesGroupes, criteria.SplitFilters);

            var split_carteasian = from split1 in splits_AllType
                                   from split2 in splits_SelectiveTypeAnswers
                                   select new
                                   {
                                       split1 = new { split1.Answer, split1.Question.Code },
                                       split2 = new { Answer = split2, Code = splits_SelectiveTypeCode }
                                   };

            var charts = new List<DataChart>();

            foreach (var filteredResponsesGroup in split_carteasian)
            {
                var chart = new DataChart();

                var datafields = filteredResponsesGroupes.Select(rg =>
                  new
                  {
                      Data = rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.FieldOfInterest)?.Answer, //select the field we are interested in for charting,
                      Id = rg.Key,
                      XAxisLable = rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.XAxislable)?.Answer,
                      XAxisId = rg.Any(r => r.Question.Code == productView.DashboardView.XAxisId) ? long.Parse(rg.First(r => r.Question.Code == productView.DashboardView.XAxisId).Answer) : 0
                  })
                  .GroupBy(d => d.XAxisId)
                  .ToList();

                chart.ChartValues = datafields.SelectMany(xg =>
                               xg.GroupBy(vg => vg.Data)
                                   .Select(vg => new ChartEntry
                                   {
                                       Value = vg.Count() * 100 / xg.Count(),
                                       XAxisLable = vg.Any() ? vg.First().XAxisLable : string.Empty,
                                       XAxisId = vg.Any() ? vg.First().XAxisId : 0,
                                       Series = vg.Key,
                                   })
                        )
                        .OrderBy(df => df.XAxisId)
                        .ToList();

                charts.Add(chart);

            }

            //var datafields = filteredResponsesGroupes.Select(rg =>
            //        new
            //        {
            //            Data = rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.FieldOfInterest)?.Answer, //select the field we are interested in for charting,
            //            Id = rg.Key,
            //            XAxisLable =  rg.FirstOrDefault(r => r.Question.Code == productView.DashboardView.XAxislable)?.Answer ,
            //            XAxisId = rg.Any(r => r.Question.Code == productView.DashboardView.XAxisId) ? long.Parse(rg.First(r => r.Question.Code == productView.DashboardView.XAxisId).Answer) : 0
            //        })
            //    .GroupBy(d => d.XAxisId).ToList();

            //return datafields.SelectMany(xg =>
            //            xg.GroupBy(vg => vg.Data)
            //              .Select(vg => new ChartEntry
            //              {
            //                  Value = vg.Count() * 100 / xg.Count(),
            //                  XAxisLable = vg.Any() ? vg.First().XAxisLable : string.Empty,
            //                  XAxisId = vg.Any() ? vg.First().XAxisId : 0,
            //                  Series = vg.Key,
            //              })
            //   )
            //   .OrderBy(df=>df.XAxisId)
            //   .ToList();
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

        private IEnumerable<IGrouping<string, Response>> FilteredResponsesGroupes(FieldSearchCriteria criteria, ProductView product)
        {

            var productFilters = product.Product?
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
                    .Where(rg =>
                        productFilters.All(q =>
                                rg.Any(r => r.Question.Code == q.Code && r.Answer == q.Answer)))
                    .ToList()
                : responsesGroupes;
            return filteredResponsesGroupes;
        }
    }
}
