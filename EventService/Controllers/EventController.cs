using EventService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventStore eventStore;

        public EventController(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        /// <summary>
        /// Kaldes af en anden service, når den har brug for at offentliggøre (publishe) et event
        /// </summary>
        /// <param name="e">Information om den event, der er opstået</param>
        [HttpPost]
        public void RaiseEvent(Event e)
        {
            eventStore.RaiseEvent(e);
        }

        /// <summary>
        /// Henter events
        /// </summary>
        /// <param name="startIndex">Index på det første event der skal hentes</param>
        /// <param name="antal">Antallet af events der maksimalt skal hentes (der kan være færre)</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<StoredEvent> ListEvents(
            [FromQuery] int startIndex,
            [FromQuery] int antal = int.MaxValue
        )
        {
            // TODO Skriv din kode her. Du må gerne ændre returtype og parametre, hvis du vil. Koden her er bare tænkt som et udgangspunkt.

            return eventStore.GetEvents(startIndex, startIndex + antal);
        }
    }
}
