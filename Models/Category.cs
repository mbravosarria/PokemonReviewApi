using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonReviewApi.Models
{
    public class Category
    {
      public int Id {get; set;}
      public string Name {get; set;}

      // Relationships
      public ICollection<PokemonCategory> PokemonCategories {get; set;}
    }
}