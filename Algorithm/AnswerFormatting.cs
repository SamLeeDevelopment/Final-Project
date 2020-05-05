using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Final_Project.Algorithm
{
    /// <summary>
    /// Returns an xml representation of Questions
    /// </summary>
    public class AnswerFormatting
    {
        private XElement root;

        public AnswerFormatting(String question)
        {
            root = new XElement("Set", new XAttribute("UserEnterQuestion", question));
        }

        /// <summary>
        /// Adds the current question info to the xml
        /// </summary>
        /// <param name="questionInfo"></param>
        public void AddQuestionInfo(QuestionInfo questionInfo)
        {
            FoundType foundType = questionInfo.Matches().foundType;

            XElement questionAnswer = new XElement("QuestionAnswer", new XAttribute("CorrectMatch", foundType.ToString()));
            questionAnswer.Add(new XElement("Question", questionInfo.Question));
            questionAnswer.Add(new XElement("Answer", questionInfo.Answer));
            root.Add(questionAnswer);

        }

        /// <summary>
        /// Returns the xml representation of questions
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return root.ToString();
        }

    }
}
