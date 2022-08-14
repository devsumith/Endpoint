using Endpoint.Core.Models.Sql;

namespace Endpoint.Core.ViewModels.Sql
{
    public class TableViewModel
    {

    }

    public static class TableModelExtensions
    {
        public static TableViewModel ToViewModel(this TableModel model)
        {
            var viewModel = new TableViewModel();

            return viewModel;
        }
    }
}
