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
                Code = "GROUPS",
            };

            var questionCHURNER_FLAG = new Question
            {
                Code = "CHURNER_FLAG",
            };

            var questionOLDPRODUCT = new Question
            {
                Code = "OLDPRODUCT",
            };

            var questionCHRUN1 = new Question
            {
                Code = "CHURN1",
            };

            var questionCHRUN2a = new Question
            {
                Code = "CHURN2A",
            };

            var questionNP1 = new Question
            {
                Code = "NP1",
            };

            var questionANALYSED_Week = new Question
            {
                Code = "ANALYSED_Week",
            };

            var questionANALYSED_WeekNo = new Question
            {
                Code = "ANALYSED_Week_#",
            };

            var questionTechType = new Question
            {
                Code = "TECHTYPE1",
            };

            var questionComp1 = new Question
            {
                Code = "COMP1",
            };

            var random = new Random();

            for (var i = 1; i <= 100; i++)
            {
                var chrun1 = GetRand(random,"Network", "Plans / pricing / inclusions", "Customer Service");

                var chrun2a = GetRand(random, "flexibility", "range of options", "cost of plan");

                var techtype = GetRand(random,"ADSL", "NBN", "Cable", "Satalite");

                var competitor = GetRand(random,"Telstra", "Vodaphone", "Virgin", "TPG", "DoDo");

                var week = random.Next(1, 4);

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
                       // XAxisId = "ANALYSED_Week_#",
                       // XAxislable = "ANALYSED_Week"
                    };

            context.Set<DashboardView>().Add(churn1);
            context.SaveChanges();


            var CHURN2A =
                    new DashboardView
                    {
                        FieldOfInterest = new List<Question> { questionCHRUN2a } ,
                        Name = "Specific Reason for churn",
                        Code = "Specific Reason for churn",
                        Parent = churn1,
                      //  XAxisId = "ANALYSED_Week_#",
                      //  XAxislable = "ANALYSED_Week"
                    };


            context.Set<DashboardView>().Add(CHURN2A);
            context.SaveChanges();


            var np1 = new DashboardView
                {
                    FieldOfInterest = new List<Question> { questionNP1 },
                    Name = "New Provider",
                    Code = "New Provider",
                   // XAxisId = "ANALYSED_Week_#",
                   // XAxislable = "ANALYSED_Week"
            };

            context.Set<DashboardView>().Add(np1);
            context.SaveChanges();

            foreach (var product in new[]
                                        {
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

                context.SaveChanges();
            }
        }
    }
}
