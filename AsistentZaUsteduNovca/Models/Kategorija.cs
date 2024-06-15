using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsistentZaUsteduNovca.Models
{
    public class Kategorija
    {
        private Kategorija category;

        [Key] // primary atribut key 
        public int CategoryId { get; set; } // primary key


        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Naziv je neophodan.")]
        public string Naziv { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string Ikonica { get; set; } = ""; // default vrednost je empty string
        // svaka kategorija ce imati emoji ikonicu za laksu identifikaciju kategorije

        [Column(TypeName = "nvarchar(10)")]
        public string Tip { get; set; } = "Trosak";
        // tip kategorije - da li je income ili expense (prihodi ili troskovi)
        // stavicemo da je expense difoltna vrednost zato sto ce vecina transakcija u toku dana biti troskovi


        [NotMapped]
        public string? TitleWithIcon { 
            get
            {
                return this.Ikonica + " "+ this.Naziv;
            }
                }




    }
}

// za ove string property-e (Naziv, iconica, tip) moramo da specificiramo odgovarajuci 
// tip podataka u sql-u za to koristimo atribut [Column]