using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.API;
using Dashboard.Services;
using DataEf.Context;
using Ninject.Modules;

namespace Dashboard.Bootstrapper
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IChartDataService>().To<ChartDataService>().InThreadScope();
            Bind<IDashboardService>().To<DashboardService>().InThreadScope();
            Bind<IUnitOfWork>().To<EfUnitOfWork>().InThreadScope();
        }
    }
}
