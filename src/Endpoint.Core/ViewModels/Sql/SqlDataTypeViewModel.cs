using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.ViewModels.Sql
{
    public class SqlDataTypeViewModel
    {

    }

    public static class SqlDataTypeModelExtensions
    {
        public static SqlDataTypeViewModel ToViewModel(this SqlDataTypeModel model)
        {
            var viewModel = new SqlDataTypeViewModel();

            return viewModel;
        }
    }
}
