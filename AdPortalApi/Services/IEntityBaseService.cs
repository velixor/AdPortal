using System;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Models;

namespace AdPortalApi.Services
{
    public interface IEntityBaseService<T> where T : class, IEntity
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(Guid id);
        Task DeleteByIdAsync(Guid id);
        Task<bool> IsEntryExistAsync(Guid id);
    }
}