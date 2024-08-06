using Gestion.API.Homework.Domain.Model.ValueObjects;

namespace Gestion.API.Homework.Domain.Model.Aggregates;

public class homework
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public DateTime Fecha { get; set; }
    public int PriorityTask { get; set; }
    public string ImagenBase64 { get; set; } 

}