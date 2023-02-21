using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PokemonReviewApi.Dto;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoryController : Controller
  {
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public IActionResult GetCategories()
    {
      List<CategoryDto> categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

      return !ModelState.IsValid ? BadRequest(ModelState) : Ok(categories);
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]
    public IActionResult GetCategory(int categoryId)
    {
      if (!_categoryRepository.CategoryExist(categoryId))
      {
        return NotFound();
      }

      CategoryDto category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

      return !ModelState.IsValid ? BadRequest() : Ok(category);
    }

    [HttpGet("pokemons/{categoryId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonsByCategory(int categoryId)
    {
      List<PokemonDto> pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));

      return !ModelState.IsValid ? BadRequest() : Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
    {
      if (categoryCreate == null)
      {
        return BadRequest(ModelState);
      }

      Category category = _categoryRepository.GetCategories()
        .FirstOrDefault(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper());

      if (category != null)
      {
        ModelState.AddModelError("", "Category already exist");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      Category categoryMap = _mapper.Map<Category>(categoryCreate);

      if (!_categoryRepository.CreateCategory(categoryMap))
      {
        ModelState.AddModelError("", "Something went wrong saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }
  }
}