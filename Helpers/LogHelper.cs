using DogShelterMvc.Data;
using DogShelterMvc.Models;
using System.Text.Json;

namespace DogShelterMvc.Helpers
{
    public static class LogHelper
    {
        public static async Task LogAsync(DogShelterDbContext context, HttpContext httpContext, string tableName, string operation, object? oldValue = null, object? newValue = null)
        {
            try
            {
                var userName = httpContext.Session.GetString("Uname") ?? "Anonymous";
                
                var log = new Log
                {
                    CUser = userName,
                    EventTime = DateTime.Now,
                    TableName = tableName,
                    Operation = operation,
                    OldValue = oldValue != null ? JsonSerializer.Serialize(oldValue) : string.Empty,
                    NewValue = newValue != null ? JsonSerializer.Serialize(newValue) : string.Empty
                };

                context.Logs.Add(log);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Tichá chyba - nechceme, aby selhalo logování kvůli chybě v logování
                // V produkci bychom to mohli logovat do souboru nebo externího systému
            }
        }

        public static string SerializeEntity(object entity)
        {
            try
            {
                return JsonSerializer.Serialize(entity, new JsonSerializerOptions 
                { 
                    WriteIndented = false,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            }
            catch
            {
                return entity.ToString() ?? string.Empty;
            }
        }
    }
}

