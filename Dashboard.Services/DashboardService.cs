using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.API.API;
using Dashboard.API.Domain;
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
        
        //todo : performance
        public IEnumerable<Product> GetProducts()
        {
            return _unitOfWork.GetRepository<Product>()
                .Include(p => p.ProductViews)
               //.Include(p => p.ProductViews.Select(pv=> pv.DashboardView))
                .Include(p => p.Filter)
                .Get();
        }

        public Product GetProduct(long productId)
        {
            return _unitOfWork.GetRepository<Product>()
               .Include(p => p.ProductViews)
               .Include(p => p.ProductViews.Select(pv => pv.DashboardView))
               .Include(p => p.ProductViews.Select(pv => pv.DashboardView.ChildrenViews))
               .Include(p => p.ProductViews.Select(pv => pv.ViewSplits))
               .Include(p => p.Filter)
               .GetSingle();
        }

        public IEnumerable<ProductView> GetProductViews(long productId)
        {
            return _unitOfWork.GetRepository<ProductView>()
                .Include(p => p.Product)
                .Include(p => p.DashboardView)
                .Include(p => p.DashboardView.ChildrenViews)
                //.Include(p => p.ViewSplits)
                //.Include(p => p.ViewSplits.Select(vs=> vs.Filter))
                //.Include(p => p.DashboardView.Parent)
                .Get(pv=> pv.Product.Id == productId);
        }

        public ProductView GetProductView(long productViewId)
        {
            return _unitOfWork.GetRepository<ProductView>()
               .Include(p => p.DashboardView)
               .Include(p => p.Product)
               .Include(p => p.ViewSplits)
               .Include(p => p.ViewSplits.Select(vs => vs.Filter))
               .Include(p => p.DashboardView.Parent)
               .GetSingle(pv => pv.Id == productViewId);
        }
    }
}
