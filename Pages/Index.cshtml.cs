using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models.Cards;
using Models.RandomUsers;

namespace Server_HttpClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private HttpClient httpClient = new HttpClient();
        public RandomUsers users { get; set; }
        public Cards cards { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int? count)
        {

            HttpResponseMessage response = await httpClient.GetAsync("https://randomuser.me/api/?results=3");

            if (response.IsSuccessStatusCode)
            {
                var dataStream = await response.Content.ReadAsStreamAsync();

                users = await JsonSerializer.DeserializeAsync<RandomUsers>(
                    dataStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }


            response = await httpClient.GetAsync("https://api.scryfall.com/cards/search?q=usd%3E1500");

            if (response.IsSuccessStatusCode)
            {
                var dataStream = await response.Content.ReadAsStreamAsync();

                cards = await JsonSerializer.DeserializeAsync<Cards>(
                    dataStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }


            return Page();

        }
    }
}
