using System;
using System.Threading.Tasks;
using Dto.Contracts;
using Dto.Contracts.AdContracts;
using Sieve.Models;

namespace Core.Services
{
    public interface IAdService : IEntityService<IAdRequest, IAdResponse>
    {}
}