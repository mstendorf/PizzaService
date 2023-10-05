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
        private static readonly List<BackendOrder> orders = new List<BackendOrder>();
        private readonly IEventStore eventStore;
        private readonly IPizzaMenuClient pizzaMenuClient;

        public PizzaController(IEventStore eventStore, IPizzaMenuClient pizzaMenuClient)
        {
            this.eventStore = eventStore;
            this.pizzaMenuClient = pizzaMenuClient;
        }

        [HttpGet]
        public IEnumerable<BackendOrder> GetOrders()
        {
            return orders;
        }

        [HttpPost]
        public ActionResult<BackendOrder> AddOrder(Order order)
        {
            var result = pizzaMenuClient.GetMenuItem(order.PizzaId);
            if (result == null || result.Result == null)
            {
                return BadRequest($"No pizza with id {order.PizzaId} exists.");
            }
            var pizza = result.Result;
            var backendOrder = new BackendOrder(order.TableNumber, pizza);
            orders.Add(backendOrder);
            eventStore.Raise("pizza_ordered", backendOrder);
            return backendOrder;
        }
    }
}
