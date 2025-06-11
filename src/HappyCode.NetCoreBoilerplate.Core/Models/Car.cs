using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyCode.NetCoreBoilerplate.Core.Models
{
    public partial class Car
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Plate { get; set; }

        [StringLength(50)]
        public string Model { get; set; }

        public Guid? OwnerId { get; set; }


        [ForeignKey("OwnerId")]
        [InverseProperty("Cars")]
        public virtual Owner Owner { get; set; }
    }
}
