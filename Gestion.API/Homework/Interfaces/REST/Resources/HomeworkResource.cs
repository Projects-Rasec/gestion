namespace Gestion.API.Homework.Interfaces.REST.Resources;

public class HomeworkResource
{
    public int Id { get; set; } // Agregar el campo Id
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public DateTime Fecha { get; set; }
    public int PriorityTask { get; set; }
    public string ImagenBase64 { get; set; } // Nueva propiedad

}