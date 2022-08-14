using Endpoint.Core.Models.Sql;

namespace Endpoint.Core.ViewModels.Sql
{

    public class ColumnViewModel
    {

    }

    public static class ColumnModelExtensions
    {
        public static ColumnViewModel ToViewModel(this ColumnModel model)
        {
            var viewModel = new ColumnViewModel();

            return viewModel;
        }
    }

}
