using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Models.Constants;
using Ninject;

namespace Dashboard.Bootstrapper
{
    public class CommonBootstrapper
    {
        public static IKernel  LoadFromCurrentAssembly()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            Common.Container = kernel;
            return kernel;
        }

        public static IKernel LoadFromCurrentAssembly(StandardKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly());
            Common.Container = kernel;
            return kernel;
        }


    }
}
