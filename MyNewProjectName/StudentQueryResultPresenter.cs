using System;
using System.Data;

namespace MyNewProjectName
{
    public class StudentQueryResultPresenter : QueryResultPresenter
    {
        public void PresentResults(IDataReader reader)
        {
            while (reader.Read())
            {
                Console.WriteLine($"Student Number: {reader["studentnumber"]}, Name: {reader["firstname"]}");
            }
        }
    }
}