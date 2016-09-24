using System.Collections.Generic;
using System.Diagnostics;
using Dashboard.API.Domain;
using Dashboard.API.Enums;

namespace DataEf.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataEf.Context.DashboardContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private string GetChurn1(int random)
        {
            switch (random)
            {
                case 1: return "Network";
                case 2: return "Plans / pricing / inclusions";
                default:
                    return "Customer Service";
            }
        }

        private string GetRand(Random rand,params string[] values)
        {
            return values[rand.Next(0, values.Length)];
        }

        protected override void Seed(DataEf.Context.DashboardContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.


            var responses = new List<Response>();

            var questionGroup = new Question
            {
                Code = "GROUPS",  Text = "GROUPS"
            };

            var questionCHURNER_FLAG = new Question
            {
                Code = "CHURNER_FLAG",
                Text = "Is Churner"
            };

            var questionOLDPRODUCT = new Question
            {
                Code = "OLDPRODUCT",
                Text = "Competitor"
            };

            var questionCHRUN1 = new Question
            {
                Code = "CHURN1",
                Text = "Reason for leaving"
            };

            var questionCHRUN2a = new Question
            {
                Code = "CHURN2A",
                Text = "specific reason for leaving"
            };

            var questionNP1 = new Question
            {
                Code = "NP1",
                Text = "new provider"
            };

            var questionANALYSED_Week = new Question
            {
                Code = "ANALYSED_Week",
                Text = "Date Text"
            };

            var questionANALYSED_WeekNo = new Question
            {
                Code = "ANALYSED_Week_#",
                Text = "Date Number "
            };

            var questionTechType = new Question
            {
                Code = "TECHTYPE1",
                Text = "Technology Type"
            };

            var questionComp1 = new Question
            {
                Code = "COMP1",
                Text = "Reason for competitor"
            };

            var questionReturnIntent = new Question
            {
                Code = "RETURN1",
                Text = "will come back"
            };

            var questionsat1 = new Question
            {
                Code = "SAT1",Text = "satisfaction 1"
            };

            var questionsat2 = new Question
            {
                Code = "SAT2",
                Text = "satisfaction 2"
            };
            var questionsat3 = new Question
            {
                Code = "SAT3",
                Text = "satisfaction 3"
            };
            var questionsat4 = new Question
            {
                Code = "SAT4",
                Text = "satisfaction 4"
            };
            var questionsat5 = new Question
            {
                Code = "SAT5",
                Text = "satisfaction 5"
            };
            var questionOsat = new Question
            {
                Code = "OSAT",
                Text = "Overall satisfaction"
            };

            var random = new Random();

            for (var i = 1; i <= 100; i++)
            {
                var chrun1 = GetRand(random,"Network", "Plans / pricing / inclusions", "Customer Service");

                var chrun2a = GetRand(random, "flexibility", "range of options", "cost of plan");

                var techtype = GetRand(random,"ADSL", "NBN", "Cable", "Satalite");

                var competitor = GetRand(random,"Telstra", "Vodaphone", "Virgin", "TPG", "DoDo");

                var week = random.Next(1, 4);

                var return1 = random.Next(1, 9) + random.NextDouble();

                var completiondate = new DateTime(2016,8, random.Next(1, 30));

                responses.Add(
                    new Response
                    {
                        Question = questionGroup,
                        Answer = "CONSUMER",
                        ResponseId = i.ToString(),
                        InputId = i,
                        CompletionDate = completiondate,
                        ResponseType = ResponseType.Text,

                    });

                responses.Add(
                    new Response
                    {
                        Question = questionCHURNER_FLAG,
                        Answer = "CHURNER",
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.Text,
                        CompletionDate = completiondate
                    });


                responses.Add(
                    new Response
                    {
                        Question = questionOLDPRODUCT,
                        Answer = "Overall Fixed",
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.Text,
                        CompletionDate = completiondate
                    });

                responses.Add(
                    new Response
                    {
                        Question = questionCHRUN1,
                        Answer = chrun1,
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.Text,
                        CompletionDate = completiondate
                    });

                responses.Add(
                  new Response
                  {
                      Question = questionCHRUN2a,
                      Answer = chrun2a,
                      ResponseId = i.ToString(),
                      InputId = i,
                      ResponseType = ResponseType.Text,
                      CompletionDate = completiondate
                  });

                responses.Add(
                   new Response
                   {
                       Question = questionTechType,
                       Answer = techtype,
                       ResponseId = i.ToString(),
                       InputId = i,
                       ResponseType = ResponseType.Text,
                       CompletionDate = completiondate
                   });

                responses.Add(
                   new Response
                   {
                       Question = questionComp1,
                       Answer = competitor,
                       ResponseId = i.ToString(),
                       InputId = i,
                       ResponseType = ResponseType.Text,
                       CompletionDate = completiondate
                   });

                responses.Add(
                    new Response
                    {
                        Question = questionANALYSED_Week,
                        Answer = $"Week {week}",
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.Text,
                        CompletionDate = completiondate
                    });

                responses.Add(
                    new Response
                    {
                        Question = questionANALYSED_WeekNo,
                        Answer = $"{week}",
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.Text,
                        CompletionDate = completiondate
                    });

                responses.Add(
                  new Response
                  {
                      Question = questionReturnIntent,
                      Answer = $"{return1}",
                      ResponseId = i.ToString(),
                      InputId = i,
                      ResponseType = ResponseType.NumericRange,
                      CompletionDate = completiondate
                  });

                /////
                responses.Add(
                 new Response
                 {
                     Question = questionsat1,
                     Answer = $"{random.Next(1, 9)}.{random.Next(1,100)}",
                     ResponseId = i.ToString(),
                     InputId = i,
                     ResponseType = ResponseType.NumericRange,
                     CompletionDate = completiondate
                 });

                responses.Add(
                 new Response
                 {
                     Question = questionsat2,
                     Answer = $"{random.Next(1, 9)}.{random.Next(1,100)}",
                     ResponseId = i.ToString(),
                     InputId = i,
                     ResponseType = ResponseType.NumericRange,
                     CompletionDate = completiondate
                 });

                responses.Add(
                 new Response
                 {
                     Question = questionsat3,
                     Answer = $"{random.Next(1, 9)}.{random.Next(1,100)}",
                     ResponseId = i.ToString(),
                     InputId = i,
                     ResponseType = ResponseType.NumericRange,
                     CompletionDate = completiondate
                 });

                responses.Add(
                 new Response
                 {
                     Question = questionsat4,
                     Answer = $"{random.Next(1, 9)}.{random.Next(1,100)}",
                     ResponseId = i.ToString(),
                     InputId = i,
                     ResponseType = ResponseType.NumericRange,
                     CompletionDate = completiondate
                 });

                responses.Add(
                 new Response
                 {
                     Question = questionsat5,
                     Answer = $"{random.Next(1, 9)}.{random.Next(1,100)}",
                     ResponseId = i.ToString(),
                     InputId = i,
                     ResponseType = ResponseType.NumericRange,
                     CompletionDate = completiondate
                 });

                responses.Add(
                 new Response
                 {
                     Question = questionOsat,
                     Answer = $"{random.Next(1, 9)}.{random.Next(1,100)}",
                     ResponseId = i.ToString(),
                     InputId = i,
                     ResponseType = ResponseType.NumericRange,
                     CompletionDate = completiondate
                 });

            }

            foreach (var response in responses)
            {
                context.Set<Response>().Add(response);
                context.SaveChanges();
            }

            var churn1 =
                    new DashboardView
                    {
                        FieldOfInterest = new List<Question> { questionCHRUN1 },
                        Name = "Broad reason for Churn",
                        Code = "Broad reason for Churn",
          
                    };


            var CHURN2A =
                    new DashboardView
                    {
                        FieldOfInterest = new List<Question> { questionCHRUN2a } ,
                        Name = "Specific Reason for churn",
                        Code = "Specific Reason for churn",
                        Parent = churn1,
                    };

            var np1 = new DashboardView
                {
                    FieldOfInterest = new List<Question> { questionNP1 },
                    Name = "New Provider",
                    Code = "New Provider",
                  
            };

            var ExpAreaSat = new DashboardView
            {
                FieldOfInterest = new List<Question> { questionsat1, questionsat2 , questionsat3, questionsat4, questionsat5, questionOsat},
                Name = "Experience Area Satisfaction",
                Code = "Experience Area Satisfaction",
                DataAnlysisType = DataAnlysisType.avarage,
               
            };

            var IntentToReturn = new DashboardView
            {
                FieldOfInterest = new List<Question> { questionReturnIntent },
                Name = "Intent To Return",
                Code = "Intent To Return",
                DataAnlysisType = DataAnlysisType.percentageAndAverage,
                ChartRenderType = ChartRenderType.lineAndBar,
                ChartRanges = "Unlikely:0-5|Neutral:6-7|Likely:8-10"
            };

            
            foreach (var product in new[]{
                                                new {P = "SMB", F = @"[GROUPS] = 'SMALL BUSINESS' AND CHURNER_FLAG = 'CHURNER'"},
                                                new
                                                {
                                                    P = "Fixed",
                                                    F = @"[GROUPS] = 'CONSUMER' AND OLDPRODUCT = 'Overall Fixed' AND CHURNER_FLAG = 'CHURNER'"
                                                }
                                         })
            {

                var p = new Product
                {
                    Name = product.P,
                    Code = product.P,
                    Filter = new Filter
                    {
                        Name = $"{product.P} Filter",
                        FilterString = product.F
                    },
                   
                };

                context.Set<Product>().Add(p);
                context.SaveChanges();

                p.ProductViews = new List<ProductView>();

                p.ProductViews.Add(new ProductView
                {
                    Product = p,
                    DashboardView = churn1,
                    ViewSplits = new List<ViewSplit>
                        {
                            new ViewSplit
                            {
                                SplitName = "Tech Type (vic)",
                                Question = questionTechType,
                                SplitType = SplitType.All,
                                Filter = new Filter
                                {
                                    FilterString = "STATE='VIC'",
                                    Name = "State Filter"
                                }
                            },
                            new ViewSplit
                            {
                                SplitName = "Tech Type",
                                Question = questionTechType,
                                SplitType = SplitType.All,
                            }
                        }
                });

                p.ProductViews.Add(new ProductView
                {
                    Product = p,
                    DashboardView = CHURN2A,
                    ViewSplits = new List<ViewSplit>
                        {
                              new ViewSplit
                            {
                                SplitName = "Tech Type (vic)",
                                Question = questionTechType,
                                SplitType = SplitType.All,
                                Filter = new Filter
                                {
                                    FilterString = "STATE='VIC'",
                                    Name = "State Filter"
                                }
                            },
                            new ViewSplit
                            {
                                SplitName = "Tech Type",
                                Question = questionTechType,
                                SplitType = SplitType.All,
                            },
                            new ViewSplit
                            {
                                SplitType = SplitType.Multiple,
                                SplitName = "Select Broad Reasons",
                                Question = questionCHRUN1,
                            }
                        }
                });

                p.ProductViews.Add(new ProductView
                {
                    Product = p,
                    DashboardView = np1,
                    ViewSplits = new List<ViewSplit>
                    {
                        new ViewSplit
                        {
                            SplitType = SplitType.Multiple,
                            SplitName = "Select Competitor",
                            Question = questionComp1,
                        }
                    }
                });

                p.ProductViews.Add(new ProductView
                {
                    Product = p,
                    DashboardView = ExpAreaSat,
                    ViewSplits = new List<ViewSplit>
                    {
                        new ViewSplit
                        {
                            SplitType = SplitType.All,
                            SplitName = "Tech Type",
                            Question = questionTechType,
                        }
                    }
                });

                p.ProductViews.Add(new ProductView
                {
                    Product = p,
                    DashboardView = IntentToReturn,
                    ViewSplits = new List<ViewSplit>
                    {
                        new ViewSplit
                        {
                            SplitType = SplitType.All,
                            SplitName = "Tech Type",
                            Question = questionTechType,
                        }
                    }
                });

                context.SaveChanges();
            }

            foreach (RecencyTypes recencyType in Enum.GetValues(typeof(RecencyTypes)))
            {
                context.Set<RecencyType>().Add(new RecencyType
                {
                    RecencyTypes = recencyType,
                    Name = recencyType.ToString(),

                });
                context.SaveChanges();

            }
        }
    }
}
