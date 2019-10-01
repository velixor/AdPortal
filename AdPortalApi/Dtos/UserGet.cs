using System;
using System.Collections.Generic;

namespace AdPortalApi.Dtos
{
    public class UserGet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> AdsId { get; set; }
    }
}