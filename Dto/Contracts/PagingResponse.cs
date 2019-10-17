using System.Collections.Generic;

namespace Dto.Contracts
{
    public class PagingResponse<T>
    {
        public int Count { get; set; }
        public List<T> Items { get; set; }
    }
}