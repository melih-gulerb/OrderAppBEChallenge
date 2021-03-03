
using Microsoft.Extensions.DependencyInjection;

namespace Entities.Repository
{
    public static class AddRepositoryServices
    {
        public static IServiceCollection AddRepositoryService(this IServiceCollection services) //Implement Repository methods to services.
        {
            return services
                .AddScoped<IRepoOrder, RepoOrder>().AddScoped<IRepoCustomer,RepoCustomer>()
                .AddScoped(typeof(IRepository<>), typeof(Repository<>));
        } 
    }
}