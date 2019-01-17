namespace RushHour.Service.Implementations
{
    using Contracts;
    using Data.Contracts;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using RushHour.Common;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AppointmentService : Service<Appointment>, IAppointmentService
    {
        private readonly UserManager<User> userManager;
        private readonly IActivityService activityService;

        public AppointmentService(IAppointmentRepository repository, UserManager<User> userManager, IActivityService activityService) : base(repository)
        {
            this.userManager = userManager;
            this.activityService = activityService;
        }

        public override async Task<IEnumerable<Appointment>> GetFilteredItemsAsync(User currentUser)
        {
            if (await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
                return GetAll();

            return GetAll(a => a.UserId == currentUser.Id);
        }

        public override async Task<bool> IsUserAuthorized(Appointment item, User currentUser)
        {
            if (await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
                return true;

            return item.UserId == currentUser.Id;
        }

        public override bool IsItemDuplicate(Appointment item)
        {
            return GetAll(a => a.Title.ToLower() == item.Title.ToLower()
                                && a.StartDateTime == item.StartDateTime    
                                && a.Id != item.Id)
                .Any();
        }

        public IEnumerable<Activity> GetNotOwnedActivities(Appointment appointment)
        {
            return activityService.GetAll(а => а
                            .Appointments
                            .All(a => a.AppointmentId != appointment.Id));
        }

        public IEnumerable<Activity> GetOwnedActivities(Appointment appointment)
        {
            return appointment.Activities
                            .Select(a => a.Activity)
                            .ToList();
        }

        public async Task<IEnumerable<Activity>> GetOwnedActivities(int appointmentId)
        {
            Appointment appointment = await GetByIdAsync(appointmentId);

            return appointment.Activities
                            .Select(a => a.Activity)
                            .ToList();
        }
    }
}
