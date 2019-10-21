using System;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Data;
using AdPortalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AdPortalApi.Services
{
    public abstract class EntityBaseService<T> : IEntityBaseService<T> where T : class, IEntity
    {
        protected readonly AdPortalContext Context;

        protected EntityBaseService(AdPortalContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual IQueryable<T> GetAll()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await Context.Set<T>().AsNoTracking().SingleAsync(x => x.Id == id);
        }

        public virtual async Task DeleteByIdAsync(Guid id)
        {
            Context.Set<T>().Remove(await GetByIdAsync(id));
        }

        public async Task<bool> IsEntryExistAsync(Guid id)
        {
            return await Context.Set<T>().AsNoTracking().AnyAsync(x => x.Id == id);
        }
    }
}