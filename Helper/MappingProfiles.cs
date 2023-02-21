using AutoMapper;

using PokemonReviewApi.Dto;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Helper
{
  public static class Mapping
  {
    private static readonly Lazy<IMapper> Lazy = new(() =>
    {
      MapperConfiguration config = new(cfg =>
      {
        cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
        cfg.AddProfile<MappingProfiles>();
      });
      IMapper mapper = config.CreateMapper();
      return mapper;
    });

    public static IMapper Mapper => Lazy.Value;
  }

  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      _ = CreateMap<Pokemon, PokemonDto>();
      _ = CreateMap<PokemonDto, Pokemon>();
      _ = CreateMap<Category, CategoryDto>();
      _ = CreateMap<CategoryDto, Category>();
      _ = CreateMap<Country, CountryDto>();
      _ = CreateMap<CountryDto, Country>();
      _ = CreateMap<Owner, OwnerDto>();
      _ = CreateMap<OwnerDto, Owner>();
      _ = CreateMap<Review, ReviewDto>();
      _ = CreateMap<ReviewDto, Review>();
      _ = CreateMap<Reviewer, ReviewerDto>();
      _ = CreateMap<ReviewerDto, Reviewer>();
    }
  }
}