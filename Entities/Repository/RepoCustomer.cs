using Entities.Model;

namespace Entities.Repository
{   // To make RepoCustomer got inheritated from main Repository
    public class RepoCustomer : Repository<Customer>, IRepoCustomer // Inherit RepoCustomer from DataContext
    {
        public RepoCustomer(DataContext dataContext) : base(dataContext)   
        {
        }
    }
}