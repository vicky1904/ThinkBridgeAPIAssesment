using AutoMapper;
using ValueObjects;
using ShopBridgeDataModel;

namespace Services.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Products, VOProduct>();
            CreateMap<VOProduct, Products>();
        }
    }
}
