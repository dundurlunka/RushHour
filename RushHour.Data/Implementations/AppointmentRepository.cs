namespace RushHour.Data.Implementations
{
    using Contracts;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(RushHourDbContext context) : base(context)
        {
        }
    }
}
