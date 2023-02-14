using LutrijaWpfEF.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LutrijaWpfEF.Model
{

    [MetadataType(typeof(PologMetadata))]
    public partial class DAPologPazara
    {
        // No field here
    }

   public class PologMetadata
    {
        [Required(ErrorMessage = "Opis je potreban.")]
        [StringLength(5)]
        public string Opis { get; set; }
    }

}