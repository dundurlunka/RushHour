namespace RushHour.Data.Models
{
    using Contracts;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Appointment : IEntity, IValidatableObject
    {
        private IList<ActivityAppointment> activities;

        public Appointment()
        {
        }

        private Appointment(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Display(Name = "Start time")]
        public DateTime StartDateTime { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public IList<ActivityAppointment> Activities
        {
            get => LazyLoader.Load(this, ref activities) ?? new List<ActivityAppointment>();
            set => Activities = value;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDateTime < DateTime.UtcNow)
            {
                yield return new ValidationResult("Start date must be in the future");
            }
        }
    }
}
