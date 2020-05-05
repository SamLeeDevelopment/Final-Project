using System;
using System.Collections.Generic;
namespace Final_Project.Models
{
    public class SubjectModel
    {
        public SubjectModel()
        {
        }

        public String Subject { get; set; }
        public List<Query> MatchingQuerys { get; set; } = new List<Query>();
        public int SubjectModelId { get; set; }

    }
}
