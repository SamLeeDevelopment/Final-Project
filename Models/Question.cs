using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Question
    {
        public Question(string q, string a)
        {
            this.query = q;
            this.answer = a;
        }

        public string query { get; set; }
        public string answer { get; set; }
    }
}
