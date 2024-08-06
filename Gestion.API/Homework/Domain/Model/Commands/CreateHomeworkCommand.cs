using Gestion.API.Homework.Domain.Model.ValueObjects;

namespace Gestion.API.Homework.Domain.Model.Commands;

public class CreateHomeworkCommand
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public DateTime Fecha { get; set; }
    public PriorityTask PriorityTask { get; set; }
    public string ImagenBase64 { get; set; }

}