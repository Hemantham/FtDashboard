using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Dashboard.API.Domain;
using Dashboard.API.Enums;
using DataEf.Context;

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

        private const string Import_DB =
            @"data source=DAREDEVIL;initial catalog=OptusDataDEV;persist security info=True;user id=OptusDataDEV;password=f8cYI40Ux*CF;";

        private string GetRand(Random rand, params string[] values)
        {
            return values[rand.Next(0, values.Length)];
        }

        private bool readFromDb = true;


        protected override void Seed(DataEf.Context.DashboardContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //  return;
            //  var responses = new List<Response>();

            try
            {
                
                var importContext = readFromDb 
                     ? new RawQuery().CollectionFromSql(Import_DB, @"SELECT DISTINCT * FROM [dbo].[Landing_Optus_Data]", new Dictionary<string, object> { }).ToList()
                     : null;

                var questionGroup = AddQuestion(context, "GROUPS","GROUPS");

                var questionCHURNER_FLAG = AddQuestion(context, "CHURNER_FLAG","Is Churner");

                var questionOLDPRODUCT = AddQuestion(context, "OLDPRODUCT","Competitor");

                var questionCHRUN1 = AddQuestion(context, "CHURN1","Reason for leaving");

                var questionCHRUN2a = AddQuestion(context, "CHURN2A","specific reason for leaving");
                
                var questionNP1 = AddQuestion(context, "NP1", "New Provider");
              
                var questionANALYSED_Week = AddQuestion(context, "ANALYSED_Week", "Date Text");

                var questionANALYSED_WeekNo = AddQuestion(context, "ANALYSED_Week_#", "Date Number ");

                var sec_questionTechType = AddQuestion(context, "TECHTYPE1", "Technology Type");

                var questionNP7 = AddQuestion(context, "NP7", "Reason for competitor");

                var questionReturnIntent = AddQuestion(context, "RETURN1", "will come back");

                var questionsat1 = AddQuestion(context, "SAT1", "satisfaction 1");

                var questionsat2 = AddQuestion(context, "SAT2", "satisfaction 2");

                var questionsat3 = AddQuestion(context, "SAT3", "satisfaction 3");

                var questionsat4 = AddQuestion(context, "SAT4", "satisfaction 4");

                var questionsat5 = AddQuestion(context, "SAT5", "satisfaction 5");

                var questionOsat = AddQuestion(context, "OSAT", "Overall satisfaction");

                var random = new Random();

                for (var i = 1; i <= importContext.Count; i++)
                {
                    var response = importContext[i-1];

                    var chrun1 = GetRand(random, "Network", "Plans / pricing / inclusions", "Customer Service");

                    var chrun2a =  GetRand(random, "flexibility", "range of options", "cost of plan");

                    var techtype = GetRand(random, "ADSL", "NBN", "Cable", "Satalite");

                    var competitor =  GetRand(random, "Telstra", "Vodaphone", "Virgin", "TPG", "DoDo");

                    var reasonForCompetitor = GetRand(random, "Reputable network", "Better prices & plan options");

                    var groups = GetRand(random, "Small Business", "Consumer");

                    var week = random.Next(1, 4);

                    var return1 =  random.Next(1, 9) + random.NextDouble();

                    var completiondate = readFromDb ? DateTime.Parse(response.EndDate) : new DateTime(2016, 8, random.Next(1, 30));

                    var asyncContext = new DashboardContext();

                    AddAnswer(asyncContext, questionGroup, groups, i, completiondate,response, ResponseType.Text);
                    
                    AddAnswer(asyncContext, questionCHURNER_FLAG, "CHURNER", i, completiondate, response, ResponseType.Text);
                    
                    AddAnswer(asyncContext, questionOLDPRODUCT, "Overall Fixed", i, completiondate, response, ResponseType.Text);
                    
                    AddAnswer(asyncContext, questionCHRUN1, chrun1, i, completiondate, response, ResponseType.Text);
                    
                    AddAnswer(asyncContext, questionCHRUN2a, chrun2a, i, completiondate, response, ResponseType.Text);
                    
                    AddAnswer(asyncContext, sec_questionTechType, techtype, i, completiondate, response, ResponseType.Text);

                    AddAnswer(asyncContext, questionNP1, competitor, i, completiondate, response, ResponseType.Text);

                    AddAnswer(asyncContext, questionNP7, reasonForCompetitor, i, completiondate, response, ResponseType.Text);
                    
                    AddAnswer(asyncContext, questionANALYSED_Week, $"Week {week}", i, completiondate, response, ResponseType.Text);

                    AddAnswer(asyncContext, questionANALYSED_WeekNo, $"{week}", i, completiondate, response, ResponseType.Text);
                    
                    AddAnswer(asyncContext, questionReturnIntent, $"{return1}", i, completiondate, response, ResponseType.NumericRange);
                    
                    AddAnswer(asyncContext, questionsat1, $"{random.Next(1, 9)}.{random.Next(1, 100)}", i, completiondate, response, ResponseType.NumericRange);

                    AddAnswer(asyncContext, questionsat2, $"{random.Next(1, 9)}.{random.Next(1, 100)}", i, completiondate, response, ResponseType.NumericRange);
                    
                    AddAnswer(asyncContext, questionsat3, $"{random.Next(1, 9)}.{random.Next(1, 100)}", i, completiondate, response, ResponseType.NumericRange);
                    
                    AddAnswer(asyncContext, questionsat4, $"{random.Next(1, 9)}.{random.Next(1, 100)}", i, completiondate, response, ResponseType.NumericRange);
                    
                    AddAnswer(asyncContext, questionsat5, $"{random.Next(1, 9)}.{random.Next(1, 100)}", i, completiondate, response, ResponseType.NumericRange);
                    
                    AddAnswer(asyncContext, questionOsat, $"{random.Next(1, 9)}.{random.Next(1, 100)}", i, completiondate, response, ResponseType.NumericRange);
                    
                    asyncContext.SaveChangesAsync();
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
                            FieldOfInterest = new List<Question> { questionCHRUN2a },
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
                    FieldOfInterest = new List<Question> { questionsat1, questionsat2, questionsat3, questionsat4, questionsat5, questionOsat },
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
                                                new {P = "SMB", F = @"GROUPS = 'SMALL BUSINESS' AND CHURNER_FLAG = 'CHURNER'"},
                                                new
                                                {
                                                    P = "Fixed",
                                                    F = @"GROUPS = 'CONSUMER' AND OLDPRODUCT = 'Overall Fixed' AND CHURNER_FLAG = 'CHURNER'"
                                                }
                                         })
                {

                    var p = new Filter
                    {
                        Name = product.P,
                        Code = product.P,
                        FilterString = product.F,
                        Group = "Product",

                    };

                    context.Set<Filter>().Add(p);
                    context.SaveChanges();

                    p.FilteredDashboardViews = new List<FilteredDashboardView>();

                    p.FilteredDashboardViews.Add(new FilteredDashboardView
                    {
                        Filter = p,
                        DashboardView = churn1,
                        ViewSplits = new List<ViewSplit>
                        {
                            new ViewSplit
                            {
                                SplitName = "Tech Type (vic)",
                                Question = sec_questionTechType,
                                SplitType = SplitType.All,
                                Filter = new Filter
                                {
                                    FilterString = "STATE='VIC'",
                                    Name = "State Filter",
                                    Code = "State",
                                }
                            },
                            new ViewSplit
                            {
                                SplitName = "Tech Type",
                                Question = sec_questionTechType,
                                SplitType = SplitType.All,
                            }
                        }
                    });

                    p.FilteredDashboardViews.Add(new FilteredDashboardView
                    {
                        Filter = p,
                        DashboardView = CHURN2A,
                        ViewSplits = new List<ViewSplit>
                        {
                              new ViewSplit
                            {
                                SplitName = "Tech Type (vic)",
                                Question = sec_questionTechType,
                                SplitType = SplitType.All,
                                Filter = new Filter
                                {
                                    FilterString = "STATE='VIC'",
                                    Name = "State Filter",
                                    Code = "State"
                                }
                            },
                            new ViewSplit
                            {
                                SplitName = "Tech Type",
                                Question = sec_questionTechType,
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

                    p.FilteredDashboardViews.Add(new FilteredDashboardView
                    {
                        Filter = p,
                        DashboardView = np1,
                        ViewSplits = new List<ViewSplit>
                    {
                        new ViewSplit
                        {
                            SplitType = SplitType.Multiple,
                            SplitName = "Select Competitor",
                            Question = questionNP7,
                        }
                    }
                    });

                    p.FilteredDashboardViews.Add(new FilteredDashboardView
                    {
                        Filter = p,
                        DashboardView = ExpAreaSat,
                        ViewSplits = new List<ViewSplit>
                    {
                        new ViewSplit
                        {
                            SplitType = SplitType.All,
                            SplitName = "Tech Type",
                            Question = sec_questionTechType,
                        }
                    }
                    });

                    p.FilteredDashboardViews.Add(new FilteredDashboardView
                    {
                        Filter = p,
                        DashboardView = IntentToReturn,
                        ViewSplits = new List<ViewSplit>
                    {
                        new ViewSplit
                        {
                            SplitType = SplitType.All,
                            SplitName = "Tech Type",
                            Question = sec_questionTechType,
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
            catch (DbEntityValidationException ex)
            {

                // Console.WriteLine( string.Join("\r\n", ex.EntityValidationErrors.Select(e=> e.Entry.Entity.ToString())));

                var x = string.Join("\r\n",
                    ex.EntityValidationErrors.SelectMany(e => e.ValidationErrors)
                        .Select(ve => $"{ve.ErrorMessage} - {ve.PropertyName}"));
                Console.WriteLine();

                throw new Exception(x);

            }
        }

        private  void AddAnswer(DashboardContext asyncContext, Question question, string randomAnswer, int index,DateTime completiondate, dynamic row, ResponseType responseType)
        {
            object value = null;

            try
            {
                value = ((IDictionary<string, object>) row)[question.Code];
            }
            catch (KeyNotFoundException ex)
            {
                value = randomAnswer; // if the column not exists pick the random value
            }

            if (!readFromDb || (value != null && !string.IsNullOrWhiteSpace($"{value}")))
            {
                asyncContext.Set<Response>().Add(
                    new Response
                    {
                        Question = asyncContext.Set<Question>().Find(question.Id),
                        Answer = readFromDb ? $"{value}" : randomAnswer,
                        ResponseId = index.ToString(),
                        InputId = index,
                        CompletionDate = completiondate,
                        ResponseType = responseType,
                    });
            }
        }

        private static Question AddQuestion(DashboardContext context, string code, string text)
        {
            var questionNP1 = new Question
            {
                Code = code,
                Text = text
            };

            context.Set<Question>().Add(questionNP1);
            context.SaveChanges();
            return questionNP1;
        }
    }
}






