namespace RushHour.Web.Controllers
{
    using AutoMapper;
    using Common;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Models;
    using Service.Contracts;
    using System.Threading.Tasks;

    [Authorize(Roles = CommonConstants.AdministratorRole)]
    public class ActivitiesController : BaseController<Activity, ActivityViewModel>
    {
        public ActivitiesController(IActivityService service, UserManager<User> userManager, IMapper mapper) : base(service, userManager, mapper)
        {
            ItemName = "Activity";
        }

        protected override string ItemName { get; set; }

        protected override async Task<Activity> GetEntityAsync(ActivityViewModel viewModel, int id)
        {
            Activity activity = await service.GetByIdAsync(id);

            if (activity == null)
                activity = new Activity();

            activity.Name = viewModel.Name;
            activity.Duration = viewModel.Duration;
            activity.Price = viewModel.Price;

            return activity;
        }

        protected override ActivityViewModel SendFormData(Activity activity, ActivityViewModel viewModel)
        {
            return viewModel;
        }
    }
}