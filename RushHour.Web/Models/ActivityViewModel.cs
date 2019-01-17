namespace RushHour.Web.Models
{
    using Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ActivityViewModel : ViewModel, IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        public int Duration { get; set; }

        public decimal Price { get; set; }
        
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
