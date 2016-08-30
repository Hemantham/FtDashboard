using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;


namespace DashboardMaster.Domain.DataContext
{
    /// <summary>
    /// implementation of UOW pattern 
    /// author:hemantha
    /// date:2014-3-22
    /// UPDATED : //2015:JAN CALLCENTRE DASHBOARD
    /// </summary>

    //Idea is to one repository for one type of object.

    public class UnitOfWork : IDisposable
    {
        private DbContext context;

        public UnitOfWork(string client)
        {
           
            } 

            Constants constants = new Constants(client);

            if (constants.DB_CONTEXT == "ModelAPIContext")  //  if (request.QueryString.AllKeys.Contains("modelentrycode") && !string.IsNullOrEmpty(request.QueryString.Get("modelentrycode")))
            {
                //News
                context = new ModelAPIContext(client);
            }
            else if (dashtype == Dashtype.brand)  
            {
                //Optus Brand 
                context = new BrandAPIContext(client);
            }
            else if (dashtype == Dashtype.callcentre)
            {
                //2015:JAN CALL CENTRE DASHBOARD
                context = new CallCentreContext(client);
            }
            else
            {
                //Unisuper and both NPS dashboards
                context = new APIContext(client);
            }            

            
         //   context.FSeriess.Include("Chart");
         //   context.FChartValues.Include("Series");         
           
        }

        private ForethoughtRepository<FChartValue> chartValueRepository;
        public ForethoughtRepository<FChartValue> ChartValueRepository
        {
            get
            {

                if (this.chartValueRepository == null)
                {
                    this.chartValueRepository = new ForethoughtRepository<FChartValue>(context);                    
                }
                return chartValueRepository;
            }
        }

        private ForethoughtRepository<ModelBasedChartValue> modelBasedChartValueRepository;
        public ForethoughtRepository<ModelBasedChartValue> ModelBasedChartValueRepository
        {
            get
            {

                if (this.modelBasedChartValueRepository == null)
                {
                    this.modelBasedChartValueRepository = new ForethoughtRepository<ModelBasedChartValue>(context);
                }
                return modelBasedChartValueRepository;
            }
        }

        private ForethoughtRepository<ChartModelEntry> chartModelEntryRepository;
        public ForethoughtRepository<ChartModelEntry> ChartModelEntryRepository
        {
            get
            {

                if (this.chartModelEntryRepository == null)
                {
                    this.chartModelEntryRepository = new ForethoughtRepository<ChartModelEntry>(context);
                }
                return chartModelEntryRepository;
            }
        }

        private ForethoughtRepository<Model> modelRepository;
        public ForethoughtRepository<Model> ModelRepository
        {
            get
            {

                if (this.modelRepository == null)
                {
                    this.modelRepository = new ForethoughtRepository<Model>(context);
                }
                return modelRepository;
            }
        }

        private ForethoughtRepository<ModelPerformance> modelPerformanceRepository;
        public ForethoughtRepository<ModelPerformance> ModelPerformanceRepository
        {
            get
            {

                if (this.modelPerformanceRepository == null)
                {
                    this.modelPerformanceRepository = new ForethoughtRepository<ModelPerformance>(context);
                }
                return modelPerformanceRepository;
            }
        }

        private ForethoughtRepository<ModelEntry> modeEntryRepository;
        public ForethoughtRepository<ModelEntry> ModeEntryRepository
        {
            get
            {

                if (this.modeEntryRepository == null)
                {
                    this.modeEntryRepository = new ForethoughtRepository<ModelEntry>(context);
                }
                return modeEntryRepository;
            }
        }

        private ForethoughtRepository<MastHead> mastHeadRepository;
        public ForethoughtRepository<MastHead> MastHeadRepository
        {
            get
            {

                if (this.mastHeadRepository == null)
                {
                    this.mastHeadRepository = new ForethoughtRepository<MastHead>(context);
                }
                return mastHeadRepository;
            }
        }

        private ForethoughtRepository<Channel> channelRepository;
        public ForethoughtRepository<Channel> ChannelRepository
        {
            get
            {

                if (this.channelRepository == null)
                {
                    this.channelRepository = new ForethoughtRepository<Channel>(context);
                }
                return channelRepository;
            }
        }


        private ForethoughtRepository<DayFigure> dayFigureRepository;
        public ForethoughtRepository<DayFigure> DayFigureRepository
        {
            get
            {

                if (this.dayFigureRepository == null)
                {
                    this.dayFigureRepository = new ForethoughtRepository<DayFigure>(context);
                }
                return dayFigureRepository;
            }
        }

        //private ForethoughtRepository<FChart> chartRepository;
        //public ForethoughtRepository<FChart> ChartRepository
        //{
        //    get
        //    {

        //        if (this.chartRepository == null)
        //        {
        //            this.chartRepository = new ForethoughtRepository<FChart>(context);
        //        }
        //        return chartRepository;
        //    }
        //}

        //private ForethoughtRepository<FSeries> seriesRepository;
        //public ForethoughtRepository<FSeries> SerieRepository
        //{
        //    get
        //    {

        //        if (this.seriesRepository == null)
        //        {
        //            this.seriesRepository = new ForethoughtRepository<FSeries>(context);
        //        }
        //        return seriesRepository;
        //    }
        //}

        private ForethoughtRepository<FChartOverride> chartOverrideRepository;
        public ForethoughtRepository<FChartOverride> ChartOverrideRepository
        {
            get
            {

                if (this.chartOverrideRepository == null)
                {
                    this.chartOverrideRepository = new ForethoughtRepository<FChartOverride>(context);
                }
                return chartOverrideRepository;
            }
        }

        private ForethoughtRepository<FSeriesOverride> seriesOverrideRepository;
        public ForethoughtRepository<FSeriesOverride> SeriesOverrideRepository
        {
            get
            {

                if (this.seriesOverrideRepository == null)
                {
                    this.seriesOverrideRepository = new ForethoughtRepository<FSeriesOverride>(context);
                }
                return seriesOverrideRepository;
            }
        }

        private ForethoughtRepository<Recency> recencyRepository;
        public ForethoughtRepository<Recency> RecencyRepository
        {
            get
            {

                if (this.recencyRepository == null)
                {
                    this.recencyRepository = new ForethoughtRepository<Recency>(context);
                }
                return recencyRepository;
            }
        }        

        private ForethoughtRepository<RecencyType> recencyTypeRepository;
        public ForethoughtRepository<RecencyType> RecencyTypeRepository
        {
            get
            {
                if (this.recencyTypeRepository == null)
                {
                    this.recencyTypeRepository = new ForethoughtRepository<RecencyType>(context);
                }
                return recencyTypeRepository;
            }
        }

        private ForethoughtRepository<CutGroup> cutGroupRepository;
        public ForethoughtRepository<CutGroup> CutGroupeRepository
        {
            get
            {
                if (this.cutGroupRepository == null)
                {
                    this.cutGroupRepository = new ForethoughtRepository<CutGroup>(context);
                }
                return cutGroupRepository;
            }
        }

        private ForethoughtRepository<Cut> cutRepository;
        public ForethoughtRepository<Cut> CutRepository
        {
            get
            {
                if (this.cutRepository == null)
                {
                    this.cutRepository = new ForethoughtRepository<Cut>(context);
                }
                return cutRepository;
            }
        }

        private ForethoughtRepository<RecencyMarket> recencyMarketRepository;
        public ForethoughtRepository<RecencyMarket> RecencyMarketRepository
        {
            get
            {
                if (this.recencyMarketRepository == null)
                {
                    this.recencyMarketRepository = new ForethoughtRepository<RecencyMarket>(context);
                }
                return recencyMarketRepository;
            }
        }

        private ForethoughtRepository<UserActivity> userActivityRepository;
        public ForethoughtRepository<UserActivity> UserActivityRepository
        {
            get
            {
                if (this.userActivityRepository == null)
                {
                    this.userActivityRepository = new ForethoughtRepository<UserActivity>(context);
                }
                return userActivityRepository;
            }
        }

        private ForethoughtRepository<UserAccess> userAccessRepository;
        public ForethoughtRepository<UserAccess> UserAccessRepository
        {
            get
            {
                if (this.userAccessRepository == null)
                {
                    this.userAccessRepository = new ForethoughtRepository<UserAccess>(context);
                }
                return userAccessRepository;
            }
        }

        private ForethoughtRepository<UserAccessAdmins> userAccessAdminsRepository;
        public ForethoughtRepository<UserAccessAdmins> UserAccessAdminsRepository
        {
            get
            {
                if (this.userAccessAdminsRepository == null)
                {
                    this.userAccessAdminsRepository = new ForethoughtRepository<UserAccessAdmins>(context);
                }
                return userAccessAdminsRepository;
            }
        }

        private ForethoughtRepository<Users> userRepository;
        public ForethoughtRepository<Users> UsersRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new ForethoughtRepository<Users>(context);
                }
                return userRepository;
            }
        }

        private ForethoughtRepository<QuestionMap> questionMapRepository;
        public ForethoughtRepository<QuestionMap> QuestionMapRepository
        {
            get
            {
                if (this.questionMapRepository == null)
                {
                    this.questionMapRepository = new ForethoughtRepository<QuestionMap>(context);
                }
                return questionMapRepository;
            }
        }

        //using this you dont have to create properties for each type as done above.
        public ForethoughtRepository<T> GetRepository<T>() where T : class
        {           
             return  new ForethoughtRepository<T>(context);              
        }   

        //After inserting you have to save if you are to insert values in.
        public void Save()
        {
            context.SaveChanges();
        }

        //It will be called automatically at the time of GC
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}