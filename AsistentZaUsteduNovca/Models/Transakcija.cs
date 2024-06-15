using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsistentZaUsteduNovca.Models
{
    public class Transakcija
    {
        [Key] //primary key atribut
        public int TransactionId { get; set; } // primary key

        [Range (1,int.MaxValue,ErrorMessage = "Molimo vas izaberite kategoriju")]
        // Ovde treba da sacuvamo i CategoryId da bi odredili sa kojim tipom transakcije radimo
        public int CategoryId { get; set; }
        // da bi sacuvali integritet podataka moramo da nabudzimo foreign key relaciju izmedju ove dve tabele
        // ovo znaci da ne mozemo da sacuvamo transakciju kada category id nije prisutan unutar base tabele (Category.cs)

        public Kategorija? Category { get; set; }
        // Foreign key 
        // Nesto je rekao "since this is a navigational property wehn we create the actual db through migration there will not be a column corresponding to this" ?!?!!?

        [Range(1, int.MaxValue, ErrorMessage = "Iznos bi trebao biti veći od 0")]
        public int Iznos { get; set; } // transaction amount

        [Column(TypeName = "nvarchar(75)")] // tip podataka u sql (za stringove)
        public string? Beleska { get; set; }
        // deskripcija transakcije - ovo nije obavezno zato je nullable (string?)

        public DateTime Datum { get; set; } = DateTime.Now;
        // predstavlja default datum koji je DateTime.now (znaci vrv trenutni) 

        [NotMapped]
        public string? CategoryTitleWithIcon { get
            {
                return Category == null ? "" : Category.Ikonica + " " + Category.Naziv;
            }
                }

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return ((Category == null || Category.Tip == "Trosak") ? "- " : "+ ") + Iznos.ToString("C0");
            }
        }
    }
}

// PRILIKOM DB MIGRACIJE OVI ENTITY MODELS KLASE CE BITI BITI KONVERTOVANE U ODGOVARAJUCE
// SQL SERVER TABELE - ovo ce se desiti samo ako postoji property koji odgovara Transaction
// i Category klasama u okviru ApplicationDbContext klase ( ona se nalazi u folderu Models)