using System.Data;

namespace MyNewProjectName.Presentation
{
    public class StudentQueryResultPresenter : IQueryResultPresenter
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