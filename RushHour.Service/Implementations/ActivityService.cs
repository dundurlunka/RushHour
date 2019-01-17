namespace RushHour.Service.Implementations
{
    using Contracts;
    using Data.Contracts;
    using Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ActivityService : Service<Activity>, IActivityService
    {
        public ActivityService(IActivityRepository repository) : base(repository)
        {
        }

        public override async Task<IEnumerable<Activity>> GetFilteredItemsAsync(User currentUser)
        {
            return GetAll();
        }

        public override async Task<bool> IsUserAuthorized(Activity item, User currentUser)
        {
            return true;
        }

        public override bool IsItemDuplicate(Activity item)
        {
            return GetAll(a => a.Name.ToLower() == item.Name.ToLower() && item.Id != a.Id).Any();
        }
    }
}
