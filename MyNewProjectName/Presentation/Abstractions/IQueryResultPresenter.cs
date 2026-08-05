using System.Data;

namespace MyNewProjectName.Presentation
{
    public interface IQueryResultPresenter
    {
        void PresentResults(IDataReader reader);
    }
}