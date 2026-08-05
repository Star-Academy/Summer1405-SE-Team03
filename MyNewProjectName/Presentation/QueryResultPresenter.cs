using System.Data;

namespace MyNewProjectName
{
    public interface QueryResultPresenter
    {
        void PresentResults(IDataReader reader);
    }
}