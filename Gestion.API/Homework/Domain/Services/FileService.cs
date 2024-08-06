namespace Gestion.API.Homework.Domain.Services;

using System;
using System.IO;

public class FileService
{
    public string SaveBase64Image(string base64Image, string directoryPath)
    {
        if (string.IsNullOrEmpty(base64Image))
            throw new ArgumentException("base64Image is null or empty");

        // Create directory if it does not exist
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        // Generate a unique file name
        var fileName = $"{Guid.NewGuid()}.jpg";
        var filePath = Path.Combine(directoryPath, fileName);

        // Remove base64 header if present
        if (base64Image.Contains(","))
            base64Image = base64Image.Split(',')[1];

        // Convert base64 string to byte array
        var imageBytes = Convert.FromBase64String(base64Image);

        // Save the byte array as an image file
        File.WriteAllBytes(filePath, imageBytes);

        return filePath;
    }
}
