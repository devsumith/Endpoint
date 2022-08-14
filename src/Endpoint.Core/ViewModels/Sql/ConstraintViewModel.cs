using Endpoint.Core.Models.Sql;

namespace Endpoint.Core.ViewModels.Sql
{
    public class ConstraintViewModel
    {

    }

    public static class ConstraintModelExtensions
    {
        public static ConstraintViewModel ToViewModel(this ConstraintModel model)
        {
            var viewModel = new ConstraintViewModel();

            return viewModel;
        }
    }
}
