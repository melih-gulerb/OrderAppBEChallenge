using System;
using System.Linq;
using System.Threading.Tasks;
using Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace Entities.Repository
{
    //This is main Repository class that parent of all the methods in solution
    public class Repository<T> : IRepository<T> where T : Base   
    {
        private readonly DataContext _dataContext;  //This will make DataContext functionable (to implement CRUD operations)
        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IQueryable<T> GetAllRecords() // Invoke all the records from database by given T table
        {
            return _dataContext.Set<T>().AsNoTracking();
        }
        public async Task<T> GetFromId(Guid id)     // Bring a record from given Id
        {
            return await _dataContext.Set<T>().AsNoTracking()   // Return a record that output Id is equal as input Id
                .FirstOrDefaultAsync(record => record.Id == id);
        }
        public async Task Create(T table)  // Create a record and save it asynchronously
        {
            await _dataContext.Set<T>().AddAsync(table);
            await _dataContext.SaveChangesAsync();
        }
        public async Task Update(Guid id, T table) //Update a record from given Id, save the changes on database asynchronously
        {
            _dataContext.Set<T>().Update(table);
            await _dataContext.SaveChangesAsync();
        }
        public async Task Delete(Guid id)   // Delete record that had given Id 
        {
            _dataContext.Set<T>().Remove(await GetFromId(id));
            await _dataContext.SaveChangesAsync();  
        }
    }
}