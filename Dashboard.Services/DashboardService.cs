using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.API.API;
using Dashboard.API.Domain;
using Dashboard.API.Models;
using Dashboard.Models;
using DataEf.Context;

namespace Dashboard.Services
{

    public class DashboardService : IDashboardService
    {
        private readonly EfUnitOfWork _unitOfWork;

        public DashboardService(EfUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
      
        public IEnumerable<Filter> GetFilters()
        {
            return _unitOfWork.GetRepository<Filter>()
                .Include(p => p.FilteredDashboardViews)
               // .Include(p => p.ProductViews.Select(pv=> pv.DashboardView))
               // .Include(p => p.Filter)
                .Get(p=> p.Group == "Product");
        }

        public Filter GetFilter(long productId)
        {
            return _unitOfWork.GetRepository<Filter>()
               .Include(p => p.FilteredDashboardViews)
               .Include(p => p.FilteredDashboardViews.Select(pv => pv.DashboardView))
               .Include(p => p.FilteredDashboardViews.Select(pv => pv.DashboardView.FieldOfInterest))
               .Include(p => p.FilteredDashboardViews.Select(pv => pv.DashboardView.ChildrenViews))
               .Include(p => p.FilteredDashboardViews.Select(pv => pv.ViewSplits))
               .GetSingle(p=> p.Id == productId);
        }

        public IEnumerable<ProductViewModel> GetProductViewModels(long productId)
        {
            var models =  GetFilteredViews(productId)
                        .Select(MapProductViewModel).ToList();
            var children = models.SelectMany(m=> m.Children).ToList();
            return models.Where(m => !children.Any(c => c.Id == m.Id));
        }

        public IEnumerable<DashboardView> GetDashboardViews()
        {
            return _unitOfWork.GetRepository<DashboardView>().Get();
        }

        private static ProductViewModel MapProductViewModel(FilteredDashboardView pv)
        {
            return new ProductViewModel
            {
                Id = pv.Id,
                DashboardView = pv.DashboardView,
                ProductId = pv.Filter.Id,
                Children = pv.DashboardView.ChildrenViews.Select( cv=>  
                    new ProductViewModel
                    {
                        DashboardView = cv,
                        Id = cv.ProductViews.First(cpv=> cpv.Filter != null && cpv.Filter.Id == pv.Filter.Id).Id,
                        ProductId = pv.Filter.Id,
                    }).ToList()
            };
        }

        public IEnumerable<FilteredDashboardView> GetFilteredViews(long productId)
        {
            return _unitOfWork.GetRepository<FilteredDashboardView>()
                .Include(p => p.Filter)
                .Include(p => p.DashboardView)
                .Include(p => p.DashboardView.ChildrenViews)
                .Include(p => p.DashboardView.ChildrenViews.Select(x=> x.ProductViews))
                //.Include(p => p.ViewSplits)
                //.Include(p => p.ViewSplits.Select(vs=> vs.Filter))
                //.Include(p => p.DashboardView.Parent)
                .Get(pv=> pv.Filter.Id == productId);
        }

        public FilteredDashboardView GetFilteredView(long productViewId)
        {
            return _unitOfWork.GetRepository<FilteredDashboardView>()
               .Include(p => p.DashboardView)
               .Include(p => p.DashboardView.FieldOfInterest)
               .Include(p => p.Filter)
               .Include(p => p.ViewSplits)
               .Include(p => p.ViewSplits.Select(vs => vs.Filter))
               .Include(p => p.ViewSplits.Select(vs => vs.Question))
               .Include(p => p.DashboardView.Parent)
               .GetSingle(pv => pv.Id == productViewId);
        }

        public ViewSplit GetViewSplit(long id)
        {
            return _unitOfWork.GetRepository<ViewSplit>()
               .Include(p => p.Filter)
               .GetSingle(pv => pv.Id == id);
        }
    }
}
