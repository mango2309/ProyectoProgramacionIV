using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProyectoProgramacionIV.Services
{
   public class FileService
    {
       private readonly string _filePath;

       public FileService(string fileName)
       {
           _filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
       }

        public async Task SaveDataAsync<T>(List<T> data)
        {
            var json = JsonSerializer.Serialize(data);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<List<T>> LoadDataAsync<T>()
        {
           if (File.Exists(_filePath))
           {
               var json = await File.ReadAllTextAsync(_filePath);
               return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
           }
            return new List<T>();
        }

        public void DeleteFile()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }
}
