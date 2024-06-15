using Microsoft.EntityFrameworkCore;

namespace AsistentZaUsteduNovca.Models
{
    public class ApplicationDbContext: DbContext // Ova klasa treba da nasledi DbContext-to, je nesto iz entityframework-a izguglati sta radi
    {

        // konstruktor                                       // base je base klasa ( a to je DbContext)
        public ApplicationDbContext(DbContextOptions options) : base(options) // izguglati sta je ovo
                                                                              // instanca ove aplikacije bice kreirana kroz dependency injection (to je uradjeno u program.cs)
        {
            // ovde treba da prosledimo db provajdera ( npr sql server ili mysql) 
            // takodje i connection string
        }

        public DbSet<Transakcija> Transactions { get; set; } // izguglati sta je dbSet
        public DbSet<Kategorija> Categories { get; set; }

        // prilikom migracije, tabele koje odgovaraju ovoj dvojici bice nazvane na isti nacin
        // koji smo definisali ovde
    }
}
