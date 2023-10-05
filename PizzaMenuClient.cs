namespace PizzaService.Clients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Models;

    public interface IPizzaMenuClient
    {
        Task<PizzaMenuItem?> GetMenuItem(int id);
    }

    public class PizzaMenuClient : IPizzaMenuClient
    {
        private readonly HttpClient httpClient;

        // a makeshift menu acting as a makebelieve cache
        private static Dictionary<int, PizzaMenuItem> menu =
            new()
            {
                [1] = new PizzaMenuItem("Margerita", "Tomato, Mozzarella", 6500),
                [2] = new PizzaMenuItem("Hawaii", "Tomato, Mozzarella, Ham, Pineapple", 7500),
                [3] = new PizzaMenuItem("Pepperoni", "Tomato, Mozzarella, Pepperoni", 8500),
                [4] = new PizzaMenuItem("Marinara", "Tomato, Garlic, Oregano", 5500),
                [5] = new PizzaMenuItem("Napoli", "Tomato, Mozzarella, Anchovies, Capers", 7500),
                [6] = new PizzaMenuItem("Diavola", "Tomato, Mozzarella, Spicy Salami", 8500),
                [7] = new PizzaMenuItem("Bufala", "Tomato, Mozzarella di Bufala", 9500),
                [8] = new PizzaMenuItem("Romana", "Tomato, Mozzarella, Anchovies", 7500),
                [9] = new PizzaMenuItem("Focaccia", "Olive Oil, Rosemary", 4500),
                [10] = new PizzaMenuItem("Calzone", "Tomato, Mozzarella, Ham, Mushrooms", 8500),
            };

        public PizzaMenuClient(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            this.httpClient = httpClient;
        }

        // this is really just added to show the intended design,
        // it does nothing right now as the url does not exist
        public async Task<PizzaMenuItem?> GetMenuItem(int id)
        {
            if (menu.TryGetValue(id, out var pizzaMenuItem))
            {
                return pizzaMenuItem;
            }

            try
            {
                var response = await this.httpClient.GetAsync($"https://pizzaapi.com/menu/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pizzaItem = JsonSerializer.Deserialize<PizzaMenuItem>(content);
                    if (pizzaItem != null)
                        menu.Add(id, pizzaItem);
                    return pizzaItem;
                }
            }
            catch (HttpRequestException)
            {
                // log exception
                Console.WriteLine("Error: Could not reach pizza menu service.");
            }
            return null;
        }
    }
}
