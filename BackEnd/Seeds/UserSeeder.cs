using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using itsc_dotnet_practice.Data;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Utilities;

namespace itsc_dotnet_practice.Seeds
{
    public static class UserSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Skip if users already exist
            if (context.Users.Any())
            {
                Console.WriteLine("Users already exist, skipping seed.");
                return;
            }

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Seeds", "DataJson", "mock_user.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Seed file not found: {filePath}");
                return;
            }

            try
            {
                var json = File.ReadAllText(filePath);

                // Configure deserializer to ignore unknown fields and case sensitivity
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var users = JsonSerializer.Deserialize<List<User>>(json, options);

                if (users != null && users.Count > 0)
                {
                    foreach (User user in users)
                    {
                        user.Password = EncryptionUtility.HashPassword(user.Password);
                        context.Users.Add(user);
                    }
                    context.SaveChanges();
                    Console.WriteLine($"Seeded {users.Count} users successfully!");
                }
                else
                {
                    Console.WriteLine("No users found in seed file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to seed users: {ex.Message}");
            }
        }
    }
}
