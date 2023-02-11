using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonReviewApi.Models
{
    public class PokemonCategory
    {
        public int PokemonId {get; set;}
        public int CategoryId {get; set;}

      // Relationships
        public Pokemon Pokemon {get; set;}
        public Category Category {get; set;}
    }
}