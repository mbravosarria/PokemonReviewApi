using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PokemonReviewApi.Dto;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OwnerController : Controller
  {
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public OwnerController(
      IOwnerRepository ownerRepository,
      ICountryRepository countryRepository,
      IMapper mapper
    )
    {
      _ownerRepository = ownerRepository;
      _countryRepository = countryRepository;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    public IActionResult GetOwners()
    {
      List<OwnerDto> owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

      return !ModelState.IsValid ? BadRequest(ModelState) : Ok(owners);
    }

    [HttpGet("{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public IActionResult GetOwner(int ownerId)
    {
      if (!_ownerRepository.OwnerExist(ownerId))
      {
        return NotFound();
      }

      OwnerDto owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

      return !ModelState.IsValid ? BadRequest() : Ok(owner);
    }

    [HttpGet("{ownerId}/pokemons")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonsByOwner(int ownerId)
    {
      if (!_ownerRepository.OwnerExist(ownerId))
      {
        return NotFound();
      }

      List<PokemonDto> pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonsByOwner(ownerId));

      return !ModelState.IsValid ? BadRequest() : Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateOwner(
      [FromQuery] int countryId,
      [FromBody] OwnerDto ownerCreate
    )
    {
      if (ownerCreate == null)
      {
        return BadRequest(ModelState);
      }

      Owner owner = _ownerRepository.GetOwners()
        .FirstOrDefault(c => c.Name.Trim().ToUpper() == ownerCreate.Name.TrimEnd().ToUpper());

      if (owner != null)
      {
        ModelState.AddModelError("", "Owner already exist");
        return StatusCode(422, ModelState);
      }

      if (!_countryRepository.CountryExist(countryId))
      {
        ModelState.AddModelError("", "Country does't exist");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      Owner ownerMap = _mapper.Map<Owner>(ownerCreate);

      ownerMap.Country = _countryRepository.GetCountry(countryId);

      if (!_ownerRepository.CreateOwner(ownerMap))
      {
        ModelState.AddModelError("", "Something went wrong saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }
  }
}