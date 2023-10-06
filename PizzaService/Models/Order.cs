namespace PizzaService.Models
{
    public record Order(int TableNumber, int PizzaId);

    public record BackendOrder(int TableNumber, PizzaMenuItem Pizza);
}
