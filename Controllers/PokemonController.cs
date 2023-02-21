using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PokemonReviewApi.Dto;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PokemonController : Controller
  {
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public PokemonController(
      IPokemonRepository pokemonRepository,
      IOwnerRepository ownerRepository,
      ICategoryRepository categoryRepository,
      IMapper mapper
    )
    {
      _pokemonRepository = pokemonRepository;
      _ownerRepository = ownerRepository;
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public IActionResult GetPokemons()
    {
      List<PokemonDto> pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

      return !ModelState.IsValid ? BadRequest(ModelState) : Ok(pokemons);
    }

    [HttpGet("{pokemonId}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(int pokemonId)
    {
      if (!_pokemonRepository.PokemonExist(pokemonId))
      {
        return NotFound();
      }

      PokemonDto pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokemonId));

      return !ModelState.IsValid ? BadRequest() : Ok(pokemon);
    }

    [HttpGet("{pokemonId}/rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonRating(int pokemonId)
    {
      if (!_pokemonRepository.PokemonExist(pokemonId))
      {
        return NotFound();
      }

      decimal rating = _pokemonRepository.GetPokemonRating(pokemonId);

      return !ModelState.IsValid ? BadRequest() : Ok(rating);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreatePokemon(
      [FromQuery] int ownerId,
      [FromQuery] int categoryId,
      [FromBody] PokemonDto pokemonCreate
    )
    {
      if (pokemonCreate == null)
      {
        return BadRequest(ModelState);
      }

      Pokemon pokemon = _pokemonRepository.GetPokemons()
        .FirstOrDefault(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper());

      if (pokemon != null)
      {
        ModelState.AddModelError("", "Pokemon already exist");
        return StatusCode(422, ModelState);
      }

      if (!_ownerRepository.OwnerExist(ownerId))
      {
        ModelState.AddModelError("", "Owner does't exist");
        return StatusCode(422, ModelState);
      }

      if (!_categoryRepository.CategoryExist(categoryId))
      {
        ModelState.AddModelError("", "Category does't exist");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      Pokemon pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

      if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
      {
        ModelState.AddModelError("", "Something went wrong saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }
  }
}