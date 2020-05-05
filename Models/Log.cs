using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Log
    {
        public void StoreInfo(string id, string query, string answer)
        {
            string path = @"users/" + id + ".txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(answer + "\n" + query);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("\n" + answer + "\n" + query);
                }
            }
        }

        public List<string> getData(string id)
        {
            List<string> data = new List<string>();

            string path = @"users/" + id + ".txt";
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s != "") data.Add(s);
                    }
                }
            }

            return data;
        }
    }
}
