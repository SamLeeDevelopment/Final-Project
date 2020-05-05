using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Final_Project.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore;
using Final_Project.Algorithm;
using Microsoft.AspNetCore.Authorization;

namespace Final_Project.Controllers
{
    public class SolverController : Controller
    {
        AnswerFinder _answerFinder;
        ApplicationDbContext dbContext;
        Log _logger;
        public SolverController(ApplicationDbContext applicationDbContext)
        {
            _answerFinder = new AnswerFinder();
            dbContext = applicationDbContext;
            _logger = new Log();
        }

        [Authorize]
        public IActionResult History()
        {
            List<Question> models = new List<Question>();
            List<string> data = _logger.getData(User.FindFirstValue(ClaimTypes.NameIdentifier));
            for (int i = data.Count - 1; i > 0; i -= 2)
            {
                models.Add(new Question(data[i], data[i - 1]));
            }

            return View(models);
        }

        [HttpPost]
        public IActionResult Query(UserEnterBindingModel model)
        {


            //Check if subject already exist in the database
            SubjectModel subjectModel = dbContext.Subjects.Where(s => s.Subject.Equals(model.Subject)).Include(x => x.MatchingQuerys).FirstOrDefault();

            if (subjectModel != null)
            {

                if (subjectModel.MatchingQuerys != null)
                {
                    Query foundQueryFromSubject = subjectModel.MatchingQuerys.Where(q => q.Question.Equals(model.Question)).FirstOrDefault();

                    //Found the question. Display it.

                    if (foundQueryFromSubject != null)
                    {
                        //log info to the user's file
                        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
                        {
                            _logger.StoreInfo(User.FindFirstValue(ClaimTypes.NameIdentifier), foundQueryFromSubject.Question, foundQueryFromSubject.Answer);
                        }


                        return View(foundQueryFromSubject);
                    }
                }


            }
            else
            {
                //Save the new subject in the database
                dbContext.InsertSubject(new SubjectModel { Subject = model.Subject });
            }

            //The subject exists. The query does not.

            Query query = new Query();

            SubjectModel subjectForQuery = dbContext.Subjects.Where(s => s.Subject.Equals(model.Subject)).First();


            //match the question asked to the user logged in
            query.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Add the subject id to the query
            //   query.SubjectModelId = subjectForQuery.ID;


            //get the response from algorithm
            List<QuestionInfo> res = _answerFinder.FindAnswers(model.Question, model.Subject);

            //Convert the response into a readable format
            StringBuilder stringBuilder = new StringBuilder();
            const String newLine = "<br>";
            for (int i = 0; i < res.Count; i++)
            {
                if (i == 1)
                {
                    query.IsMultipleAnswer = true;
                }

                stringBuilder.Append("<span class='topic'>"+res[i].Question+"</span>");
                stringBuilder.Append(newLine);
                stringBuilder.Append(res[i].Answer);
                if(i != res.Count-1) stringBuilder.Append(newLine+newLine);
            }

            query.Question = model.Question;
            query.Answer = stringBuilder.ToString();

            //log info to the user's file
            if(query.UserId != null) _logger.StoreInfo(query.UserId, query.Question, query.Answer.Length==0?"Nothing found, sorry.":query.Answer);


            subjectForQuery.MatchingQuerys.Add(query);

            dbContext.SaveChanges();

            return View(query);

        }

    }
}