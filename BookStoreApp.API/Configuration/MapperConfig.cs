using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorDto, Author>().ReverseMap();
            CreateMap<BookDto, Book>();
            CreateMap<Book, BookDto>().ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName}{map.Author.LastName}"))
                .ReverseMap();

            CreateMap<ApiUser, UserDto>().ReverseMap();
        }
    }
}
