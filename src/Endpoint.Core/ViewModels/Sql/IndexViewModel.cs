using Endpoint.Core.Models.Sql;

namespace Endpoint.Core.ViewModels.Sql
{
    public class IndexViewModel
    {

    }

    public static class IndexModelExtensions
    {
        public static IndexViewModel ToViewModel(this IndexModel model)
        {
            var viewModel = new IndexViewModel();

            return viewModel;
        }
    }
}
