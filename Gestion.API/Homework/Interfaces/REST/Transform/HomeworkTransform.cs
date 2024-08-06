using Gestion.API.Homework.Domain.Model.ValueObjects;

namespace Gestion.API.Homework.Interfaces.REST.Transform;

using Gestion.API.Homework.Domain.Model.Aggregates;
using Gestion.API.Homework.Interfaces.REST.Resources;

public static class HomeworkTransform
{
    public static HomeworkResource ToResource(homework homework)
    {
        return new HomeworkResource
        {
            Id = homework.Id, // Agregar el ID
            Nombre = homework.Nombre,
            Descripcion = homework.Descripcion,
            Fecha = homework.Fecha,
            PriorityTask = (int)homework.PriorityTask,
            ImagenBase64 = homework.ImagenBase64 // Asignar la nueva propiedad

        };
    }

    public static homework FromResource(HomeworkResource resource)
    {
        return new homework
        {
            Id = resource.Id, // Agregar el ID
            Nombre = resource.Nombre,
            Descripcion = resource.Descripcion,
            Fecha = resource.Fecha,
            PriorityTask = (int)resource.PriorityTask,
            ImagenBase64 = resource.ImagenBase64 // Asignar la nueva propiedad

        };
    }
}