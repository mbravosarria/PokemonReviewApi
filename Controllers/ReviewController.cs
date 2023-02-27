using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PokemonReviewApi.Dto;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ReviewController : Controller
  {
    private readonly IReviewRepository _reviewRepository;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
    {
      _reviewRepository = reviewRepository;
      _pokemonRepository = pokemonRepository;
      _reviewerRepository = reviewerRepository;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    public IActionResult GetReviews()
    {
      List<ReviewDto> reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

      return !ModelState.IsValid ? BadRequest(ModelState) : Ok(reviews);
    }

    [HttpGet("{reviewId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetReview(int reviewId)
    {
      if (!_reviewRepository.ReviewExist(reviewId))
      {
        return NotFound();
      }

      ReviewDto review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

      return !ModelState.IsValid ? BadRequest() : Ok(review);
    }

    [HttpGet("pokemon/{pokemonId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewsOfAPokemon(int pokemonId)
    {
      List<ReviewDto> reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokemonId));

      return !ModelState.IsValid ? BadRequest() : Ok(reviews);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto reviewCreate)
    {
      if (reviewCreate == null)
      {
        return BadRequest(ModelState);
      }

      Review review = _reviewRepository.GetReviews()
        .FirstOrDefault(c => c.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper());

      if (review != null)
      {
        ModelState.AddModelError("", "Review already exist");
        return StatusCode(422, ModelState);
      }

      if (!_pokemonRepository.PokemonExist(pokemonId))
      {
        ModelState.AddModelError("", "Pokemon does't exist");
        return StatusCode(422, ModelState);
      }

      if (!_reviewerRepository.ReviewerExist(reviewerId))
      {
        ModelState.AddModelError("", "Reviewer does't exist");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      Review reviewMap = _mapper.Map<Review>(reviewCreate);

      reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
      reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

      if (!_reviewRepository.CreateReview(reviewMap))
      {
        ModelState.AddModelError("", "Something went wrong saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
    {
      if (updatedReview == null)
      {
        return BadRequest(ModelState);
      }

      if (reviewId != updatedReview.Id)
      {
        return BadRequest(ModelState);
      }

      if (!_reviewRepository.ReviewExist(reviewId))
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var reviewMap = _mapper.Map<Review>(updatedReview);

      if (!_reviewRepository.UpdateReview(reviewMap))
      {
        ModelState.AddModelError("", "Something went wrong updating review");
        return StatusCode(500, ModelState);
      }

      return NoContent();
    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReview(int reviewId)
    {
      if (!_reviewRepository.ReviewExist(reviewId))
      {
        return NotFound();
      }

      var reviewToDelete = _reviewRepository.GetReview(reviewId);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (!_reviewRepository.DeleteReview(reviewToDelete))
      {
        ModelState.AddModelError("", "Something went wrong deleting review");
      }

      return NoContent();
    }
  }
}