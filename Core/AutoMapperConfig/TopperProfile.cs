using AutoMapper;
using backend.Core.Dtos;
using backend.Core.Entities;

namespace backend.Core.AutoMapperConfig
{
    public class TopperProfile : Profile
    {
        public TopperProfile()
        {
            // Get Toppers
            CreateMap<Topper, TopperDto>();

            // Create topper - del this
            CreateMap<TopperDto, Topper>();

            // Update topper
            CreateMap<TopperUpdateDto, Topper>();
        }
    }
}
