namespace RushHour.Data.Models
{
    using Contracts;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser<int>, IEntity
    {
        public override int Id { get; set; }

        public IEnumerable<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
