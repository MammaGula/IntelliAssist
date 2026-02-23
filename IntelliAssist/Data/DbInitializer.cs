using IntelliAssist.Models;
using Microsoft.EntityFrameworkCore;

namespace IntelliAssist.Data;


// Database initializer for seeding sample data

public static class DbInitializer
{
   
    public static async Task InitializeAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("Database ensured created");

            // Check if we already have movies
            if (await context.UserMovies.AnyAsync())
            {
                logger.LogInformation("Database already contains movie data. Skipping seed.");
                return;
            }

            // Seed sample movies
            logger.LogInformation("Seeding sample movie data...");
            await SeedMoviesAsync(context, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

  
    // Seed sample movie data
   
    private static async Task SeedMoviesAsync(AppDbContext context, ILogger logger)
    {
        var movies = new[]
        {
            new UserMovie { Title = "The Shawshank Redemption", Genre = "Drama", Rating = 5 },
            new UserMovie { Title = "The Godfather", Genre = "Crime", Rating = 5 },
            new UserMovie { Title = "The Dark Knight", Genre = "Action", Rating = 5 },
            new UserMovie { Title = "Pulp Fiction", Genre = "Crime", Rating = 5 },
            new UserMovie { Title = "Forrest Gump", Genre = "Drama", Rating = 4 },
            new UserMovie { Title = "Inception", Genre = "Sci-Fi", Rating = 5 },
            new UserMovie { Title = "The Matrix", Genre = "Sci-Fi", Rating = 5 },
            new UserMovie { Title = "Interstellar", Genre = "Sci-Fi", Rating = 4 },
            new UserMovie { Title = "Goodfellas", Genre = "Crime", Rating = 5 },
            new UserMovie { Title = "The Lord of the Rings: The Fellowship of the Ring", Genre = "Fantasy", Rating = 5 },
            new UserMovie { Title = "Star Wars: Episode V", Genre = "Sci-Fi", Rating = 5 },
            new UserMovie { Title = "Parasite", Genre = "Thriller", Rating = 5 },
            new UserMovie { Title = "Gladiator", Genre = "Action", Rating = 4 },
            new UserMovie { Title = "The Silence of the Lambs", Genre = "Thriller", Rating = 5 },
            new UserMovie { Title = "Saving Private Ryan", Genre = "War", Rating = 5 }
        };

        context.UserMovies.AddRange(movies);
        await context.SaveChangesAsync();

        logger.LogInformation("Successfully seeded {Count} movies to database", movies.Length);
    }

  

    // Clear all movie data (useful for testing)
    public static async Task ClearMoviesAsync(AppDbContext context, ILogger logger)
    {
        logger.LogWarning("Clearing all movie data from database");
        
        var movies = await context.UserMovies.ToListAsync();
        context.UserMovies.RemoveRange(movies);
        await context.SaveChangesAsync();
        
        logger.LogInformation("Cleared {Count} movies from database", movies.Count);
    }
}
