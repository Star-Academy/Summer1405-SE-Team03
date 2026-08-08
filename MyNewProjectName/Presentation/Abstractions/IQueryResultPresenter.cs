using System.Data;

namespace MyNewProjectName.Presentation.Abstractions
{
    public interface IQueryResultPresenter
    {
        void PresentResults(IDataReader reader);
    }
}