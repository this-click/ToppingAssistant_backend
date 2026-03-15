using AutoMapper;
using backend.Core.Dtos;
using backend.Core.Entities;

namespace backend.Core.AutoMapperConfig
{
    public class TopperProfile : Profile
    {
        public TopperProfile()
        {
            // Get Toppers, converts from Topper to TopperDto
            CreateMap<Topper, TopperDto>();

            // Create topper - del this, converts from Dto to Topper
            CreateMap<TopperDto, Topper>();

            // Buy toppers, converts from Dto to Topper
            CreateMap<BuyToppersDto, Topper>();

            // Feed topper, converts from Dto to Topper
            CreateMap<FeedTopperDto, Topper>();
        }
    }
}
