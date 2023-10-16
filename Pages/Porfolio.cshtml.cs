using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using WebApplicationTest.DataInfrastructure;

namespace WebApplicationTest.Pages
{
    public class PorfolioModel : PageModel
    {
        public int count = 0;
        private readonly AppDb _appDb;
        public PorfolioModel(AppDb appDb)
        {
            _appDb = appDb;
        }
        public async void OnGet()
        {
            await _appDb.Connection.OpenAsync();

            using var command = new MySqlCommand("SELECT title from job_experience", _appDb.Connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var value = reader.GetValue(0);
                Console.WriteLine(value);
            }
            count++;
        }
    }
}
