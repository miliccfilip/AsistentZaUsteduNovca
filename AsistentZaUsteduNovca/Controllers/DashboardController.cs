using AsistentZaUsteduNovca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace AsistentZaUsteduNovca.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<ActionResult> Index()
        {

            // Zadnjih 7 dana


            DateTime StartDate = DateTime.Today.AddDays(-6); //six days from the current week
            DateTime EndDate = DateTime.Today;

            List<Transakcija> SelectedTransaction = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Datum>=StartDate && y.Datum<=EndDate)
                .ToListAsync();



            // Total income 
            int TotalIncome = SelectedTransaction
                .Where(i => i.Category.Tip == "Prihod")
                .Sum(j => j.Iznos);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");


            // Total expense 
            int TotalExpense = SelectedTransaction
                .Where(i => i.Category.Tip == "Trosak")
                .Sum(j => j.Iznos);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");


            //Balance
            int Balance = TotalIncome - TotalExpense;
            ViewBag.Balance = Balance.ToString("C0");
            string formattedBalance = Balance < 0 ? $"-{(-Balance).ToString("C0")}" : Balance.ToString("C0");
            ViewBag.Balance = formattedBalance;

           

            //Doughnut Chart - Expense by Category

            ViewBag.DoughnutChartData = SelectedTransaction
                .Where(i => i.Category.Tip == "Trosak")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    CategoryTitleWithIcon = k.First().Category.Ikonica + " " + k.First().Category.Naziv,
                    iznos = k.Sum(j => j.Iznos),
                    formatttedAmount = k.Sum(j => j.Iznos).ToString("C0"),
                })
                .OrderByDescending(l=>l.iznos)
                .ToList();

            //Spline Chart - Troskovi i Primanja
            //Primanja
            List<SplineChartData> IncomeSummary = SelectedTransaction
                .Where(i =>i.Category.Tip =="Prihod") 
                .GroupBy(j=>j.Datum)
                .Select(k=>new SplineChartData()
            {
                day = k.First().Datum.ToString("dd-MM"),
                 income = k.Sum(l=>l.Iznos)
            })
                .ToList();

            //Troskovi
            List<SplineChartData> ExpenseSummary = SelectedTransaction
                .Where(i => i.Category.Tip == "Trosak")
                .GroupBy(j => j.Datum)
                .Select(k => new SplineChartData()
            {
                day = k.First().Datum.ToString("dd-MM"),
                expense = k.Sum(l => l.Iznos)
            })
                .ToList();

            //Kombinovani troskovi i primanja

            string[] last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MM"))
            .ToArray();

            ViewBag.SplineChartData = from day in last7Days
                                      join income in IncomeSummary on day equals income.day
                                      into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense
                                      };


            ViewBag.RecentTransaction = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Datum)
                .Take(5)
                .ToListAsync();



            return View();
                

        }
    }
    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }

}
