using System.Linq;
using Data.Models;
using Sieve.Models;
using Sieve.Services;

namespace Core.Extensions
{
    public static class SieveExtensions
    {
        public static int ApplyAndCount<T>(this ISieveProcessor sieveProcessor, SieveModel sieveModel,
            ref IQueryable<T> entries) where T : IEntity
        {
            entries = sieveProcessor.Apply(sieveModel, entries, applyPagination: false);
            var count = entries.Count();
            entries = sieveProcessor.Apply(sieveModel, entries, applyFiltering: false, applySorting: false);
            return count;
        }
    }
}
