using Entities.Model;

namespace Entities.Repository
{
    // To make RepoOrder got inheritated from main Repository
    public class RepoOrder : Repository<Order>, IRepoOrder  // Inherit RepoCustomer from DataContext
    {
        public RepoOrder(DataContext dataContext) : base(dataContext)
        {
        }
    }
}