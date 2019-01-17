namespace RushHour.Data.Implementations
{
    using AutoMapper;
    using Contracts;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(RushHourDbContext context) : base(context)
        {
        }
    }
}
