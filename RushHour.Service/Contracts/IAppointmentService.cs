namespace RushHour.Service.Contracts
{
    using Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAppointmentService : IService<Appointment>
    {
        IEnumerable<Activity> GetNotOwnedActivities(Appointment appointment);

        IEnumerable<Activity> GetOwnedActivities(Appointment appointment);

        Task<IEnumerable<Activity>> GetOwnedActivities(int appointmentId);
    }
}
