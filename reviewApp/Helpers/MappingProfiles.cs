using AutoMapper;
using reviewApp.Dto;
using reviewApp.Models;

namespace reviewApp.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Pokemon, PokemonDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Country, CountryDto>();
        CreateMap<CountryDto, Country>();
        CreateMap<Owner, OwnerDto>();
        CreateMap<OwnerDto, Owner>();
        CreateMap<Review, ReviewDto>();
        CreateMap<Reviewer, ReviewerDto>();
    }
}