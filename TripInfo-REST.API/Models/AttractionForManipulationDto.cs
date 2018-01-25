using System;
using System.ComponentModel.DataAnnotations;

namespace TripInfoREST.API.Models
{
    public class AttractionForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a name.")]
        [MaxLength(100, ErrorMessage = "The name shouldn't have more than 100 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "The description shouldn't have more than 500 characters.")]
        public virtual string Description { get; set; }
    }
}
