using System.Collections.Generic;

namespace Dto.Contracts
{
    public class PagingResponse<T> : IResponse
    {
        public int Count { get; set; }
        public List<T> Items { get; set; }
    }
}