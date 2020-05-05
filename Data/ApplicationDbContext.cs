using System;
using System.Collections.Generic;
using System.Text;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Final_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Query> Querys { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }

        public void InsertSubject(SubjectModel subjectModel)
        {
            Subjects.Add(subjectModel);
            SaveChanges();
        }

        public void Update()
        {
            SaveChanges();
        }



    }
}