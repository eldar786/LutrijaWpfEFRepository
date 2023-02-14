using LutrijaWpfEF.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LutrijaWpfEF.Model
{
        [MetadataType(typeof(IsplataMetadata))]
        public partial class DAIsplataOsnovnih
        {
            // No field here
        }

        public class IsplataMetadata
        {
            [Required(ErrorMessage = "Opis je potreban.")]
            [StringLength(5)]
            public string Opis { get; set; }
        }

    }
