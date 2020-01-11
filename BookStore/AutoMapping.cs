using AutoMapper;
using BookStore.Db.Entities;
using BookStore.Models;
using BookStore.Models.DTOs;
using LinqManager.Client.Db.Entities;

namespace BookStore
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, DropDown<int>>().ConvertUsing(value => new DropDown<int> { Key = value.Id, Value = value.Name });

            CreateMap<AuthorDto, Author>()
                .ForMember(m => m.CountryId, e => e.MapFrom(c => c.Country.Key))
                .ForMember(m => m.Country, e => e.Ignore())
                .ReverseMap();
            CreateMap<Author, DropDown<int>>()
                .ConvertUsing(value => new DropDown<int> { Key = value.Id, Value = $"{value.FirstName} {value.LastName}" });

            CreateMap<BookDto, Book>()
                .ForMember(m => m.AuthorId, e => e.MapFrom(src => src.Author.Key))
                .ForMember(m => m.Author, e => e.Ignore())
                .ReverseMap();
            CreateMap<Book, DropDown<int>>().ConvertUsing(value => new DropDown<int> { Key = value.Id, Value = value.Name });

            CreateMap<PublishingHouseDto, PublishingHouse>().ReverseMap();
            CreateMap<PublishingHouse, DropDown<int>>().ConvertUsing(value => new DropDown<int> { Key = value.Id, Value = value.Name });

            CreateMap<BookPublishingHouse, DropDown<int>>().ConvertUsing(value => new DropDown<int> { Key = value.Book.Id, Value = value.Book.Name });
            CreateMap<BookPublishingHouse, DropDown<int>>().ConvertUsing(value => new DropDown<int> { Key = value.PublishingHouse.Id, Value = value.PublishingHouse.Name });
        }
    }
}