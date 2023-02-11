using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonReviewApi.Models
{
    public class Owner
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Gym {get; set;}

        // Relationships      
        public Country Country {get; set;}
        public ICollection<PokemonOwner> PokemonOwners {get; set;}
    }
}