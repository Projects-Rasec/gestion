using System;
using System.IO;
using System.Threading.Tasks;
using Gestion.API.Homework.Domain.Model.Commands;
using Gestion.API.Homework.Domain.Model.Aggregates;
using Gestion.API.Homework.Domain.Repositories;
using Gestion.API.Homework.Domain.Services;

namespace Gestion.API.Homework.Application.CommandServices
{
    public class HomeworkCommandService : IHomeworkCommandService
    {
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly string _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/homework"); // Directorio para guardar imágenes

        public HomeworkCommandService(IHomeworkRepository homeworkRepository)
        {
            _homeworkRepository = homeworkRepository;

            // Crear el directorio de imágenes si no existe
            if (!Directory.Exists(_imageDirectory))
            {
                Directory.CreateDirectory(_imageDirectory);
            }
        }

        public async Task CreateHomeworkTask(CreateHomeworkCommand command)
        {
            var imagePath = SaveImageFromBase64(command.ImagenBase64);

            var homeworkTask = new homework
            {
                Nombre = command.Nombre,
                Descripcion = command.Descripcion,
                Fecha = command.Fecha,
                PriorityTask = (int)command.PriorityTask,
                ImagenBase64 = imagePath // Guardar la ruta de la imagen en lugar del Base64
            };

            await _homeworkRepository.AddAsync(homeworkTask);
        }

        public async Task UpdateHomeworkTask(UpdateHomeworkCommand command)
        {
            var imagePath = SaveImageFromBase64(command.ImagenBase64);

            var homeworkTask = new homework
            {
                Id = command.Id,
                Nombre = command.Nombre,
                Descripcion = command.Descripcion,
                Fecha = command.Fecha,
                PriorityTask = command.Prioridad,
                ImagenBase64 = imagePath // Guardar la ruta de la imagen en lugar del Base64
            };

            await _homeworkRepository.UpdateAsync(homeworkTask);
        }

        private string SaveImageFromBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                // Decodificar la cadena Base64
                var imageBytes = Convert.FromBase64String(base64String);

                // Generar un nombre de archivo único
                var fileName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine(_imageDirectory, fileName);

                // Guardar la imagen en el disco
                File.WriteAllBytes(filePath, imageBytes);

                // Devolver la ruta relativa de la imagen
                return $"/images/homework/{fileName}";
            }
            catch (FormatException ex)
            {
                // Manejo de errores para cadenas Base64 inválidas
                Console.WriteLine($"Error al decodificar la cadena Base64: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteHomeworkTask(int id)
        {
            await _homeworkRepository.DeleteAsync(id);
        }
    }
}
