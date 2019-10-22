using System.Linq;
using AdPortalApi.Models;
using Sieve.Models;
using Sieve.Services;

namespace AdPortalApi.Extensions
{
    public static class SieveExtensions
    {
        // TODO check is "ref" really needed
        public static int ApplyAndCount<T>(this ISieveProcessor sieveProcessor, SieveModel sieveModel, ref IQueryable<T> entries) where T : IEntity
        {
            entries = sieveProcessor.Apply(sieveModel, entries, applyPagination: false);
            var count = entries.Count();
            entries = sieveProcessor.Apply(sieveModel, entries, applyFiltering: false, applySorting: false);
            return count;
        }
    }
}