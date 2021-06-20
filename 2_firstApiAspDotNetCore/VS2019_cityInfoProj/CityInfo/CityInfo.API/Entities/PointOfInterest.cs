using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        //EF core automatically sets CityId as a foreign key here (convention based) but we can explicitly set it
        //to be clear - we explicity create the foreign key CityId and can name it whatever we want by including the ForeignKey decorator
        //and specifiying the name in parens.
        [ForeignKey("CityId")]
        public City City { get; set; }
        public int CityId { get; set; }
    }
}