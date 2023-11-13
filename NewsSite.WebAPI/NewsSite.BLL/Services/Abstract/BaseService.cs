using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.BLL.Services.Abstract
{
    public abstract class BaseService
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly IMapper _mapper;

        public BaseService(UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
    }
}
