using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonReviewApi.Models
{
  public class PokemonOwner
  {
    public int PokemonId { get; set; }
    public int OwnerId { get; set; }

    // Relationships
    public Pokemon Pokemon { get; set; }
    public Owner Owner { get; set; }
  }
}