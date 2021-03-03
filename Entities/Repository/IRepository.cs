using System;
using System.Linq;
using System.Threading.Tasks;
using Entities.Model;

namespace Entities.Repository
{
    public interface IRepository<T> where T : Base  //Interface of Repository.cs, T : table of also inside in the database 
    {       
        IQueryable<T> GetAllRecords(); // Brings all the T records
        Task<T> GetFromId(Guid id); // Invokes the record that given its Id as input
        Task Create(T table);  // Creates a record
        Task Update(Guid id, T table); // Updates record that given its Id as input
        Task Delete(Guid id);  // Deletes record that given its Id as input
    }
}