using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Response;
using System.Linq.Expressions;
using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.BLL.Services.Abstract
{
    public abstract class BaseService
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly IMapper _mapper;

        protected BaseService(
            UserManager<IdentityUser> userManager, 
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
    }
}
