namespace RushHour.Web.Controllers
{
    using AutoMapper;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Models;
    using RushHour.Service.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AppointmentsController : BaseController<Appointment, AppointmentViewModel>
    {
        private readonly IActivityService activityService;
        private readonly IAppointmentService appointmentService;

        public AppointmentsController(IAppointmentService service, UserManager<User> userManager, IMapper mapper, IActivityService activityService) : base(service, userManager, mapper)
        {
            appointmentService = service;
            this.activityService = activityService;
            ItemName = "Appointment";
        }

        protected override string ItemName { get; set; }

        protected override async Task FillViewModelProps(IEnumerable<AppointmentViewModel> appointments)
        {
            foreach (AppointmentViewModel appointment in appointments)
            {
                appointment.OwnedActivities = await appointmentService.GetOwnedActivities(appointment.Id);
                var activitiesDuration = appointment.OwnedActivities.Sum(a => a.Duration);
                appointment.EndDateTime = appointment.StartDateTime.AddMinutes(activitiesDuration);
            }
        }

        private void UpdateActivities(AppointmentViewModel viewModel, Appointment appointment)
        {
            IEnumerable<Activity> activitiesToAdd = appointmentService.GetNotOwnedActivities(appointment)
                                                .Where(a => viewModel
                                                            .Activities
                                                            .Contains(a.Id));
            IEnumerable<Activity> activitiesToRemove = appointmentService.GetOwnedActivities(appointment)
                                    .Where(a => !viewModel
                                                .Activities
                                                .Contains(a.Id));

            foreach (Activity activityToAdd in activitiesToAdd)
            {
                appointment
                    .Activities
                    .Add(new ActivityAppointment
                    {
                        ActivityId = activityToAdd.Id,
                        AppointmentId = appointment.Id
                    });
            }

            foreach (Activity activityToRemove in activitiesToRemove)
            {
                ActivityAppointment mappingEntry = appointment.Activities.First(a => a.ActivityId == activityToRemove.Id);

                appointment
                    .Activities
                    .Remove(mappingEntry);
            }
        }       

        private async Task AddNewActivities(AppointmentViewModel viewModel, Appointment appointment)
        {
            foreach (int activityId in viewModel.Activities)
            {
                Activity newActivity = await activityService.GetByIdAsync(activityId);

                if (newActivity != null)
                {
                    appointment
                        .Activities
                        .Add(new ActivityAppointment
                        {
                            ActivityId = activityId,
                        });
                }
            }
        }

        protected override async Task<Appointment> GetEntityAsync(AppointmentViewModel viewModel, int id)
        {
            Appointment appointment = await service.GetByIdAsync(id);

            if (appointment == null)
            {
                appointment = new Appointment();
                appointment.UserId = GetCurrentUserAsync().Result.Id;
                await AddNewActivities(viewModel, appointment);
            }
            else
            {
                UpdateActivities(viewModel, appointment);
            }

            appointment.Title = viewModel.Title;
            appointment.StartDateTime = viewModel.StartDateTime;

            return appointment;
        }

        protected override AppointmentViewModel SendFormData(Appointment appointment, AppointmentViewModel viewModel)
        {
            viewModel = viewModel ?? new AppointmentViewModel();

            if (appointment == null)
            {
                viewModel.NotOwnedActivities = activityService.GetAll();
            }
            else
            {
                viewModel.NotOwnedActivities = appointmentService.GetNotOwnedActivities(appointment);

                viewModel.OwnedActivities = appointmentService.GetOwnedActivities(appointment);
            }

            return viewModel;
        }


    }
}
