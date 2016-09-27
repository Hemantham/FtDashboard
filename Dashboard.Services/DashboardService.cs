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
        
      
        public IEnumerable<Product> GetProducts()
        {
            return _unitOfWork.GetRepository<Product>()
                .Include(p => p.ProductViews)
               // .Include(p => p.ProductViews.Select(pv=> pv.DashboardView))
               // .Include(p => p.Filter)
                .Get();
        }

        public Product GetProduct(long productId)
        {
            return _unitOfWork.GetRepository<Product>()
               .Include(p => p.ProductViews)
               .Include(p => p.ProductViews.Select(pv => pv.DashboardView))
               .Include(p => p.ProductViews.Select(pv => pv.DashboardView.FieldOfInterest))
               .Include(p => p.ProductViews.Select(pv => pv.DashboardView.ChildrenViews))
               .Include(p => p.ProductViews.Select(pv => pv.ViewSplits))
               .Include(p => p.Filter)
               .GetSingle(p=> p.Id == productId);
        }

        public IEnumerable<ProductViewModel> GetProductViewModels(long productId)
        {
            var models =  GetProductViews(productId)
                        .Select(MapProductViewModel).ToList();
            var children = models.SelectMany(m=> m.Children).ToList();
            return models.Where(m => !children.Any(c => c.Id == m.Id));
        }

        private static ProductViewModel MapProductViewModel(ProductView pv)
        {
            return new ProductViewModel
            {
                Id = pv.Id,
                DashboardView = pv.DashboardView,
                ProductId = pv.Product.Id,
                Children = pv.DashboardView.ChildrenViews.Select( cv=>  
                    new ProductViewModel
                    {
                        DashboardView = cv,
                        Id = cv.ProductViews.First(cpv=> cpv.Product != null && cpv.Product.Id == pv.Product.Id).Id,
                        ProductId = pv.Product.Id,
                    }).ToList()
            };
        }

        public IEnumerable<ProductView> GetProductViews(long productId)
        {
            return _unitOfWork.GetRepository<ProductView>()
                .Include(p => p.Product)
                .Include(p => p.DashboardView)
                .Include(p => p.DashboardView.ChildrenViews)
                .Include(p => p.DashboardView.ChildrenViews.Select(x=> x.ProductViews))
                //.Include(p => p.ViewSplits)
                //.Include(p => p.ViewSplits.Select(vs=> vs.Filter))
                //.Include(p => p.DashboardView.Parent)
                .Get(pv=> pv.Product.Id == productId);
        }

        public ProductView GetProductView(long productViewId)
        {
            return _unitOfWork.GetRepository<ProductView>()
               .Include(p => p.DashboardView)
               .Include(p => p.DashboardView.FieldOfInterest)
               .Include(p => p.Product)
               .Include(p => p.Product.Filter)
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
