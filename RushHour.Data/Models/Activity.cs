namespace RushHour.Data.Models
{
    using Contracts;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Activity : IEntity, IValidatableObject
    {
        private IList<ActivityAppointment> appointments;

        public Activity()
        {
        }

        private Activity(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public int Duration { get; set; }

        public decimal Price { get; set; }

        public IList<ActivityAppointment> Appointments
        {
            get => LazyLoader.Load(this, ref appointments);
            set => appointments = value;
        } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Duration < 0)
            {
                yield return new ValidationResult("Duration cannot be less than 0");
            }

            if (Price < 0)
            {
                yield return new ValidationResult("Price cannot be less than 0");
            }
        }
    }
}
