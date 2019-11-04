using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Data.Models;
using Dto.Contracts;
using Dto.Contracts.UserContracts;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace Core.Services
{
    public class UserService : EntityService<User>, IUserService
    {
        public UserService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }

        protected override bool IsAuthorized(Guid id, Guid userId)
        {
            if (id == Guid.Empty || userId == Guid.Empty) return false;
            return Entries.SingleOrDefault(x => x.Id == id)?.Id == userId;
        }

        public async Task<TResponse> RegisterNewAsync<TResponse>(IRequest request) where TResponse : IResponse
        {
            var newEntry = await Register(request);

            return MapToResponse<TResponse>(newEntry);
        }

        public async Task RegisterNewAsync(IRequest request)
        {
            await Register(request);
        }

        private async Task<User> Register(IRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var newEntry = MapFromRequest(request);
            Context.Users.Add(newEntry);
            await Context.SaveChangesAsync();
            return newEntry;
        }
        
        public async Task<TResponse> LoginAsync<TResponse>(IRequest request) where TResponse : IResponse
        {
            var user = await Login(request);

            return MapToResponse<TResponse>(user);
        }

        public async Task<bool> LoginAsync(IRequest request)
        {
            return await Login(request) != null;
        }

        private async Task<User> Login(IRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!(request is UserLoginRequest loginRequest)) throw new InvalidCastException(nameof(request));

            var user = await Context.Users.SingleOrDefaultAsync(x =>
                x.Email == loginRequest.Email && x.Password == loginRequest.Password);
            return user;
        }
    }
}