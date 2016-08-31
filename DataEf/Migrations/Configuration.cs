using System.Collections.Generic;
using Dashboard.API.Domain;

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
                Code = "CHRUN1",
            };

            var questionANALYSED_Week = new Question
            {
                Code = "ANALYSED_Week",
            };

            var questionANALYSED_WeekNo = new Question
            {
                Code = "ANALYSED_Week_#",
            };

            for (var i = 1; i <= 10000; i++)
            {

                var chrun1 = i % 2 == 0 ? "Network" : "Plans / pricing / inclusions";

                responses.Add(
                    new Response
                    {
                        Question = questionGroup,
                        Answer = "CONSUMER",
                        ResponseId = i.ToString(),
                        InputId = i,
                        CompletionDate = new DateTime(2016, 8, 8),
                        ResponseType = ResponseType.text,

                    });

                responses.Add(
                    new Response
                    {
                        Question = questionCHURNER_FLAG,
                        Answer = "CHURNER",
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.text,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });


                responses.Add(
                    new Response
                    {
                        Question = questionOLDPRODUCT,
                        Answer = "Overall Fixed",
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.text,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

                responses.Add(
                    new Response
                    {
                        Question = questionCHRUN1,
                        Answer = chrun1,
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.text,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

                responses.Add(
                    new Response
                    {
                        Question = questionANALYSED_Week,
                        Answer = "Week 1",
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.text,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

                responses.Add(
                    new Response
                    {
                        Question = questionANALYSED_WeekNo,
                        Answer = "1",
                        ResponseId = i.ToString(),
                        InputId = i,
                        ResponseType = ResponseType.text,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

            }

            foreach (var response in responses)
            {
                context.Set<Response>().Add(response);
                context.SaveChanges();
            }
        }
    }
}
