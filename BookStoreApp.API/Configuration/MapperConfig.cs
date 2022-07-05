using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Configuration
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorDto, Author>().ReverseMap();
        }
    }
}
