using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Solstice.CodingChallenge.Domain.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
