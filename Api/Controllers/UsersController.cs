using System;
using System.Threading.Tasks;
using Core.Services;
using Data.Models;
using Dto.Contracts.UserContracts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsersController : EntityBaseController<User, UserRequest, UserResponse>
    {
        public UsersController(IUserService entityService) : base(entityService)
        {
        }

        public override Task<UserResponse> Post([FromBody] UserRequest request)
        {
            return base.Post(request);
        }

        public override Task<UserResponse> Put(Guid id, [FromBody] UserRequest request)
        {
            return base.Put(id, request);
        }
    }
}