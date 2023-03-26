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
        CreateMap<Country, CountryDto>();
    }
}