using AutoMapper;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;


namespace SecureStore.Api.ApplicationLayer.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));


            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.Products.Select(p => p.Id).ToList()));


            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));


            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));


            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id).ToList()));


            CreateMap<Role, RoleDTO>();


            CreateMap<ShoppingCart, ShoppingCartDTO>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Items.Sum(item => item.TotalPrice)))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));



            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));


            CreateMap<ShoppingCartDTO, ShoppingCart>();

            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.FirstName == "Admin" ? 1 : 2));

        }
    }
}
