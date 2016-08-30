using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Models;
using Microsoft.Win32;

namespace Domain
{
    public class Question
    {
        public string Code { get; set; }
        public string Text { get; set; }
        public QuestionGroup QuestionGroup { get; set; }

        public IEnumerable<ChartEntry> Get()
        {
            var responses = new List<Response>();

            for (var i = 0; i < 1000000; i++)
            {

                var chrun1 = i%2 == 0 ? "Network" : "Plans / pricing / inclusions";

                responses.Add(
                    new Response
                    {
                        Question = new Question
                        {
                            Code = "GROUPS",
                        },
                        Answer = "CONSUMER",
                        ResponseId = i.ToString(),
                        InputId = i,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

                responses.Add(
                    new Response
                    {
                        Question = new Question
                        {
                            Code = "CHURNER_FLAG",
                        },
                        Answer = "CHURNER",
                        ResponseId = i.ToString(),
                        InputId = i,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });


                responses.Add(
                    new Response
                    {
                        Question = new Question
                        {
                            Code = "OLDPRODUCT",
                        },
                        Answer = "Overall Fixed",
                        ResponseId = i.ToString(),
                        InputId = i,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

                responses.Add(
                    new Response
                    {
                        Question = new Question
                        {
                            Code = "CHRUN1",
                        },
                        Answer = chrun1,
                        ResponseId = i.ToString(),
                        InputId = i,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

                responses.Add(
                    new Response
                    {
                        Question = new Question
                        {
                            Code = "ANALYSED_Week",
                        },
                        Answer = "Week 1",
                        ResponseId = i.ToString(),
                        InputId = i,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

                responses.Add(
                    new Response
                    {
                        Question = new Question
                        {
                            Code = "ANALYSED_Week_#",
                        },
                        Answer = "1",
                        ResponseId = i.ToString(),
                        InputId = i,
                        CompletionDate = new DateTime(2016, 8, 8)
                    });

            }

            var responsesGroupes = responses.GroupBy(r => r.ResponseId); // create a group for each response 

            var filteredResponsesGroupes = responsesGroupes
                .Where(
                    rg => rg.Any(r => r.Question.Code == "GROUPS" && r.Answer == "CONSUMER") &&
                          rg.Any(r => r.Question.Code == "CHURNER_FLAG" && r.Answer == "CHURNER") &&
                          rg.Any(r => r.Question.Code == "OLDPRODUCT" && r.Answer == "Overall Fixed")); // filter by the cuts/filters , etc

            var datafields = filteredResponsesGroupes.Select(rg =>
                    new
                    {
                        Data = rg.Any() ? rg.First(r => r.Question.Code == "CHRUN1").Answer : string.Empty, //select the field we are interested in for charting,
                        Id = rg.Key,
                        XAxisLable = rg.Any(r => r.Question.Code == "ANALYSED_Week") ?  rg.First(r => r.Question.Code == "ANALYSED_Week").Answer : string.Empty,
                        XAxisId =  rg.Any(r => r.Question.Code == "ANALYSED_Week_#") ? long.Parse(rg.First(r => r.Question.Code == "ANALYSED_Week_#").Answer) : 0
                    })
                .GroupBy(d => d.XAxisId);

             return  datafields.SelectMany(xg => 
                          xg.GroupBy(vg => vg.Data)
                            .Select( vg=> new ChartEntry
                                {
                                    Value = vg.Count() * 100 /xg.Count(),
                                    XAxisLable = vg.Any() ? vg.First().XAxisLable : string.Empty,
                                    XAxisId = vg.Any() ? vg.First().XAxisId : 0,
                                    Series = vg.Key,
                                })
                ).ToList();
          
        }
    }
}


//[GROUPS] = 'CONSUMER' AND OLDPRODUCT = 'Overall Fixed' AND CHURNER_FLAG = 'CHURNER'