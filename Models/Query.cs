using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Final_Project.Models
{
    public class Query
    {
        public int Id { get; set; }
        public int SubjectModelId { get; set; }
        public SubjectModel Parent { get; set; }

        public string UserId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public bool IsMultipleAnswer { get; set; }

        public String Response { get; set; }

    }
}
