namespace PizzaService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using EventFeed;
    using Clients;

    [ApiController]
    [Route("orders")]
    public class PizzaController : ControllerBase
    {
        private static readonly List<Order> orders = new List<Order>();
        private readonly IEventStore eventStore;
        private readonly IPizzaMenuClient pizzaMenuClient;

        public PizzaController(IEventStore eventStore, IPizzaMenuClient pizzaMenuClient)
        {
            this.eventStore = eventStore;
            this.pizzaMenuClient = pizzaMenuClient;
        }

        [HttpGet]
        public IEnumerable<Order> GetOrders()
        {
            return orders;
        }

        [HttpPost]
        public ActionResult<Order> AddOrder(Order order)
        {
            var result = pizzaMenuClient.GetMenuItem(order.PizzaId);
            if (result == null || result.Result == null)
            {
                return BadRequest($"No pizza with id {order.PizzaId} exists.");
            }
            else
            {
                var pizza = result.Result;
                orders.Add(order);
                eventStore.Raise("pizza_ordered", new BackendOrder(order.TableNumber, pizza));
            }
            return order;
        }
    }
}
