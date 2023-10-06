namespace EventService.Models
{
    /// <summary>
    /// Information om den event, der er opstået
    /// Tænk over, hvilke data det er relevant at event'et indeholder
    /// </summary>
    public class Event
    {
        public string Name { get; }

        // jeg har valgt at gøre eventet så generisk som muligt, da det for mig ikke giver mening at lave
        // seperat sevice til at håndtere en andens service specifikke events.
        // i leder muligvis efter en mere direkte implementation ift pizzaId og bordnummer,
        // men på denne måde kan servicen anvendes som en generel event service.
        // Jeg ville dog aldrig gøre det på denne måde of i stedet udstille events direkte i servicen som også
        // efter hvad bogen ligger op til, eller bruge rabbitMQ eller lignende til at håndtere events.
        public Dictionary<string, object> Content { get; }

        // public Object Content { get; }

        public Event(string name, Dictionary<string, object> content)
        {
            this.Name = name;
            this.Content = content;
        }
    }

    // jeg har lavet en wrapper klasse for at gemme ekstra meta data om et event der er håndteret af denne service.
    // det giver mulighed for at have styr på tiden denne service har set events i og have et fortløbende id
    // der gør event replay og debugging nemmere.
    public class StoredEvent
    {
        public long Id { get; }
        public string Name { get; }
        public object Content { get; }
        public DateTime OccuredAt { get; }

        public StoredEvent(long id, Event @event)
        {
            this.Id = id;
            this.Name = @event.Name;
            this.Content = @event.Content;
            this.OccuredAt = DateTime.Now;
        }
    }
}
