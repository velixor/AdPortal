using System.Collections.Generic;
using System.Linq;
using Sieve.Models;

namespace Mvc.Extensions
{
    public static class SieveExtension
    {
        public static string GetQueryString(this SieveModel sieveModel)
        {
            var query = new List<string>();

            if (sieveModel.Sorts != null)
            {
                query.Add($"sorts={sieveModel.Sorts}");
            }

            if (sieveModel.Filters != null)
            {
                query.Add($"filters={sieveModel.Filters}");
            }

            if (sieveModel.Page != null)
            {
                query.Add($"page={sieveModel.Page.Value}");
            }

            if (sieveModel.PageSize != null)
            {
                query.Add($"pagesize={sieveModel.PageSize.Value}");
            }

            return string.Join('&', query);
        }
    }
}