using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PokemonReviewApi.Dto;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class ReviewerController : Controller
  {
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
    {
      _reviewerRepository = reviewerRepository;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
    public IActionResult GetReviewers()
    {
      var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

      return !ModelState.IsValid ? BadRequest(ModelState) : Ok(reviewers);
    }

    [HttpGet("{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewer(int reviewerId)
    {
      if (!_reviewerRepository.ReviewerExist(reviewerId))
        return NotFound();

      var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

      return !ModelState.IsValid ? BadRequest() : Ok(reviewer);
    }

    [HttpGet("{reviewerId}/reviews")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewersByReviewer(int reviewerId)
    {
      if (!_reviewerRepository.ReviewerExist(reviewerId))
        return NotFound();

      var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

      return !ModelState.IsValid ? BadRequest() : Ok(reviews);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
    {
      if (reviewerCreate == null)
      {
        return BadRequest(ModelState);
      }

      Reviewer reviewer = _reviewerRepository.GetReviewers()
        .FirstOrDefault(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper());

      if (reviewer != null)
      {
        ModelState.AddModelError("", "Reviewer already exist");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      Reviewer reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

      if (!_reviewerRepository.CreateReviewer(reviewerMap))
      {
        ModelState.AddModelError("", "Something went wrong saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }
  }
}