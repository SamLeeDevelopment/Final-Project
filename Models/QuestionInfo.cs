using Final_Project.StringComparison;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class QuestionInfo
    {
        public String Question { get; private set; }
        public String Answer { get; private set; }

        private String UserEnterQuestion { get; set; }

        private FoundTypeWithPercent FoundTypeWithPercent { get; set; } = new FoundTypeWithPercent() { foundType = FoundType.NEVER_SET };

        public QuestionInfo(String question, String answer, String userEnterQuestion)
        {
            Question = question;
            Answer = answer;
            UserEnterQuestion = userEnterQuestion;
        }

        /// <summary>
        /// Checks if the user question matches the current question pulled from quizlet
        /// </summary>
        /// <returns></returns>
        public FoundTypeWithPercent Matches()
        {
            if (FoundTypeWithPercent.foundType == FoundType.NEVER_SET)
            {
                JaccardDistance jaccardDistance = new JaccardDistance();

                decimal d = jaccardDistance.Distance(Question, UserEnterQuestion);


                if (d == 100)
                {
                    FoundTypeWithPercent.foundType = FoundType.CORRECT;
                    FoundTypeWithPercent.Percent = 100;
                }
                else if (d >= 50)
                {
                    FoundTypeWithPercent.foundType = FoundType.POSSIBLE;
                    FoundTypeWithPercent.Percent = d;
                }
                else
                {
                    FoundTypeWithPercent.foundType = FoundType.NONE;
                }

            }


            return FoundTypeWithPercent;
        }

    }

    public enum FoundType
    {
        CORRECT, POSSIBLE, NONE, NEVER_SET
    }

    public class FoundTypeWithPercent
    {
        public FoundType foundType { get; set; }
        public decimal Percent { get; set; }
    }
}
