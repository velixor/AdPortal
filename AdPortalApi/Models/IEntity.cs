using System;

namespace AdPortalApi.Models
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}