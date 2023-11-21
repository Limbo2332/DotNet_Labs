using AutoMapper;
using NewsSite.DAL.DTO.Request;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;

namespace NewsSite.BLL.MappingProfiles
{
    public class RubricProfile : Profile
    {
        public RubricProfile()
        {
            CreateMap<Rubric, RubricResponse>();
            CreateMap<NewRubricRequest, Rubric>();
            CreateMap<UpdateRubricRequest, Rubric>();
        }
    }
}
