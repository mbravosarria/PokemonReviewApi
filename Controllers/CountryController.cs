using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PokemonReviewApi.Dto;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CountryController : Controller
  {
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
      _countryRepository = countryRepository;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
    public IActionResult GetCountries()
    {
      List<CountryDto> countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

      return !ModelState.IsValid ? BadRequest(ModelState) : Ok(countries);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountry(int countryId)
    {
      if (!_countryRepository.CountryExist(countryId))
      {
        return NotFound();
      }

      CountryDto country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

      return !ModelState.IsValid ? BadRequest() : Ok(country);
    }

    [HttpGet("owners/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountryOfAnOwner(int ownerId)
    {
      CountryDto country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

      return !ModelState.IsValid ? BadRequest() : Ok(country);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
    {
      if (countryCreate == null)
      {
        return BadRequest(ModelState);
      }

      Country country = _countryRepository.GetCountries()
        .FirstOrDefault(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper());

      if (country != null)
      {
        ModelState.AddModelError("", "Country already exist");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      Country countryMap = _mapper.Map<Country>(countryCreate);

      if (!_countryRepository.CreateCountry(countryMap))
      {
        ModelState.AddModelError("", "Something went wrong saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
    {
      if (updatedCountry == null)
      {
        return BadRequest(ModelState);
      }

      if (countryId != updatedCountry.Id)
      {
        return BadRequest(ModelState);
      }

      if (!_countryRepository.CountryExist(countryId))
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var countryMap = _mapper.Map<Country>(updatedCountry);

      if (!_countryRepository.UpdateCountry(countryMap))
      {
        ModelState.AddModelError("", "Something went wrong updating country");
        return StatusCode(500, ModelState);
      }

      return NoContent();
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCountry(int countryId)
    {
      if (!_countryRepository.CountryExist(countryId))
      {
        return NotFound();
      }

      var countryToDelete = _countryRepository.GetCountry(countryId);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (!_countryRepository.DeleteCountry(countryToDelete))
      {
        ModelState.AddModelError("", "Something went wrong deleting country");
      }

      return NoContent();
    }
  }
}