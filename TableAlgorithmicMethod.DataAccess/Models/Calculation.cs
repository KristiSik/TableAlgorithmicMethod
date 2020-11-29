using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TableAlgorithmicMethod.DataAccess.Models
{
    public class Calculation
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int DataFormatId { get; set; }

        public int NumberOfElements { get; set; }

        public long ClassicMethodElapsedTicks { get; set; }

        public long TableAlgorithmicMethodElapsedTicks { get; set; }
    }
}
