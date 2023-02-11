using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonReviewApi.Models
{
    public class Reviewer
    {
        public int Id {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}

      // Relationships
        public ICollection<Owner> Owners {get; set;}
    }
}