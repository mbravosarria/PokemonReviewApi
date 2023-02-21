using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonReviewApi.Models
{
  public class Review
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public decimal Rating { get; set; }

    // Relationships   
    public Reviewer Reviewer { get; set; }
    public Pokemon Pokemon { get; set; }
  }
}