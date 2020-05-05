using Final_Project.Models;
using HtmlAgilityPack;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Algorithm
{
    public class AnswerFinder
    {


        private HashSet<string> addedQuestions { get; set; } = new HashSet<string>();

        public List<QuestionInfo> FindAnswers(String question, String subject)
        {


            question = question.ToLower().Trim();

            List<QuestionInfo> possible = new List<QuestionInfo>();
            List<QuestionInfo> correctQuestion = new List<QuestionInfo>();

            // I dont think subjects are case sensetive on quizlet
            List<String> setList = GetSetUrl(subject);

            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 2;

            Parallel.ForEach(setList, parallelOptions, (setURL, state) =>
            {

                GetSetInfo(setURL, question, possible, correctQuestion, state);

            });

            return correctQuestion.Count() == 1 ? correctQuestion : possible;



        }


        /// <summary>
        /// Returns all links to each set that matches the subject
        /// </summary>
        /// <param name="subject">The name to use</param>
        /// <returns></returns>
        List<String> GetSetUrl(String subject)
        {

            //ScrapingBrowser browser = new ScrapingBrowser();
            String subjectUrl = CreateUrlFromSuject(subject);

            // WebPage page =  browser.NavigateToPage(new Uri(subjectUrl));

            HtmlDocument htmlDocument = GetHtmlDocument(subjectUrl);
            HtmlNode node = htmlDocument.DocumentNode;

            //    HtmlNode node = page.Html;


            var v = node.Descendants("a").Where(x => x.GetAttributeValue("class", "").Equals("UILink")).ToList();


            List<String> SetUrls = new List<string>();

            foreach (HtmlNode n in v)
            {

                String value = n.GetAttributeValue("href", "");

                if (value.Contains("https://"))
                {
                    SetUrls.Add(value);
                }


            }

            return SetUrls;

        }


        /// <summary>
        /// Loops over a set and returns the correct answer if found or any possibilities.
        /// </summary>
        /// <param name="url">The url of the set</param>
        /// <returns></returns>
        void GetSetInfo(String url, String userEnterQuestion, List<QuestionInfo> possible, List<QuestionInfo> correct, ParallelLoopState state)
        {
            // ScrapingBrowser scrapingBrowser = new ScrapingBrowser();

            List<QuestionInfo> setInfo = new List<QuestionInfo>();

            //  WebPage page = scrapingBrowser.NavigateToPage(new Uri(url));

            HtmlDocument htmlDocument = GetHtmlDocument(url);
            HtmlNode n = htmlDocument.DocumentNode;



            // HtmlNode n = page.Html;

            String name = n.Descendants("title").FirstOrDefault().InnerHtml;

            var stuff = n.Descendants("span").Where(x => x.GetAttributeValue("class", "").Contains("TermText")).ToList();

            for (int i = 0; i < stuff.Count() - 2; i += 2)
            {




                // Make sure all leading and trailing white spaces are gone. Lowercase the entire string
                String foundQuestion = stuff[i].InnerText.ToLower().Trim();
                String answer = stuff[i + 1].InnerText;


                QuestionInfo questionInfo = new QuestionInfo(foundQuestion, answer, userEnterQuestion);


                // Add the question info to the dictionary. Use the found question as the key.
                if (!addedQuestions.Contains(foundQuestion))
                {
                    addedQuestions.Add(foundQuestion);

                    //Check if we have found the exact answer
                    if (questionInfo.Matches().foundType == FoundType.CORRECT)
                    {


                        correct.Add(questionInfo);

                        state.Break();


                    }
                    // Add a possibility to the list of questions
                    else if (questionInfo.Matches().foundType == FoundType.POSSIBLE)
                    {
                        possible.Add(questionInfo);
                    }

                }

            }



        }






        HtmlDocument GetHtmlDocument(String subject)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            htmlWeb.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.2 Safari/605.1.15";
            return htmlWeb.Load(subject);
        }


        ///<summary>
        ///Creates the url that points to the collection of sets
        ///</summary>
        String CreateUrlFromSuject(String subject)
        {
            return String.Format("https://quizlet.com/subject/{0}/", subject);
        }
    }
}
