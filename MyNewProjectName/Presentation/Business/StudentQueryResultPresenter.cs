using System.Data;
using MyNewProjectName.Presentation.Abstractions;

namespace MyNewProjectName.Presentation.Business
{
    internal sealed class StudentQueryResultPresenter : IQueryResultPresenter
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