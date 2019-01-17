namespace RushHour.Data.Models
{
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class ActivityAppointment
    {
        private Activity activity;

        public ActivityAppointment()
        {
        }

        private ActivityAppointment(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public Activity Activity
        {
            get => LazyLoader.Load(this, ref activity);
            set => activity = value;
        }

        public int ActivityId { get; set; }

        public Appointment Appointment { get; set; }

        public int AppointmentId { get; set; }
    }
}
