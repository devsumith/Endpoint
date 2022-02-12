namespace Endpoint.Core.Managers
{
    //https://stackoverflow.com/questions/47637228/how-can-i-add-files-to-a-solution-folder
    public interface ISolutionFileManager
    {
        void AddSolutionItem(string path);
    }

    public class SolutionFileManager : ISolutionFileManager
    {
        public void AddSolutionItem(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
