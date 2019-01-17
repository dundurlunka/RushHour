namespace RushHour.Web.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class AppointmentViewModel : ViewModel, IValidatableObject, IMapFrom<Appointment>, IHaveCustomMapping
    {
        [Required]
        public string Title { get; set; }

        [Display(Name = "From")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "From")]
        public DateTime EndDateTime { get; set; }

        public IEnumerable<Activity> NotOwnedActivities { get; set; } = new List<Activity>();

        public IEnumerable<Activity> OwnedActivities { get; set; } = new List<Activity>();

        public IEnumerable<int> Activities { get; set; } = new List<int>();

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(a => a.NotOwnedActivities, cfg => cfg.Ignore())
                .ForMember(a => a.Activities, cfg => cfg.MapFrom(act => act
                                                                        .Activities
                                                                        .Select(m => m.ActivityId).ToList()));
        }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (StartDateTime < DateTime.UtcNow)
            {
                yield return new ValidationResult("Start date must be in the future");
            }
        }
    }
}
