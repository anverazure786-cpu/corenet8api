// Required namespaces for ASP.NET Core, Entity Framework, and  models.
using Microsoft.AspNetCore.Http; // For handling HTTP-specific information.
using Microsoft.AspNetCore.Mvc; // For building RESTful APIs and MVC applications.
using Microsoft.EntityFrameworkCore; // For interacting with the database using Entity Framework Core.
using Rest_API_Project.Models; // For accessing the `Movie` model and `MovieContext`.

namespace Rest_API_Project.Controllers // Declares the namespace for the controller.
{
    // Defines the API route as "api/Movies" and marks this class as an API controller.
    [Route("api/[controller]")]
    [ApiController] 
    public class MoviesController : ControllerBase // Inherits from ControllerBase, which is suitable for APIs.
    {
        // Dependency injection to access the database context.
        private readonly MovieContext _movieContext;

        // Constructor to initialize the database context.
        public MoviesController(MovieContext movieContext)
        {
            _movieContext = movieContext; // Assigns the injected context to the local variable.
        }

        // Handles GET requests to "api/Movies" to retrieve all movies.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            // Checks if the `Movies` table is null (e.g., the database is not initialized).
            if (_movieContext.Movies == null)
            {
                return NotFound(); // Returns a 404 status if no movies are found.
            }
            // Retrieves all movies from the database as a list asynchronously.
            return await _movieContext.Movies.ToListAsync();
        }

        // Handles GET requests to "api/Movies/{id}" to retrieve a specific movie by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            // Checks if the `Movies` table is null.
            if (_movieContext.Movies is null)
            {
                return NotFound(); // Returns a 404 status if no movies are found.
            }
            // Finds the movie by its ID asynchronously.
            var movie = await _movieContext.Movies.FindAsync(id);

            // Returns a 404 status if the movie is not found.
            if (movie is null)
            {
                return NotFound();
            }
            return movie; // Returns the movie if found.
        }

        // Handles POST requests to "api/Movies" to add multiple movies to the database.
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Movie>>> PostMovies(List<Movie> movies)
        {
            // Validates that the list of movies is not null or empty.
            if (movies == null || !movies.Any())
            {
                return BadRequest("The movies list cannot be empty."); // Returns a 400 status if invalid.
            }

            // Adds the movies to the database context in bulk.
            _movieContext.Movies.AddRange(movies);
            // Saves changes asynchronously to persist the movies in the database.
            await _movieContext.SaveChangesAsync();

            // Returns a 201 status with the created movies and a reference to the GET method.
            return CreatedAtAction(nameof(GetMovies), null, movies);
        }

        // Handles PUT requests to "api/Movies/{id}" to update a movie by ID.
        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> PutMovie(int id, Movie movie)
        {
            // Validates that the ID in the URL matches the ID in the request body.
            if (id != movie.Id)
            {
                return BadRequest(); // Returns a 400 status if the IDs do not match.
            }

            // Marks the movie entity as modified.
            _movieContext.Entry(movie).State = EntityState.Modified;

            try
            {
                // Saves changes asynchronously to update the movie in the database.
                await _movieContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Checks if the movie exists in the database.
                if (!MovieExists(id))
                {
                    return NotFound(); // Returns a 404 status if the movie does not exist.
                }
                else
                {
                    throw; // Re-throws the exception if another issue occurs.
                }
            }

            // Returns a 204 status indicating the update was successful.
            return NoContent();
        }

        // Helper method to check if a movie exists in the database by ID.
        private bool MovieExists(long id)
        {
            // Checks if any movie in the database matches the given ID.
            return (_movieContext.Movies?.Any(movie => movie.Id == id)).GetValueOrDefault();
        }

        // Handles DELETE requests to "api/Movies/{id}" to delete a movie by ID.
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            // Checks if the `Movies` table is null.
            if (_movieContext.Movies is null)
            {
                return NotFound(); // Returns a 404 status if no movies are found.
            }

            // Finds the movie by its ID asynchronously.
            var movie = await _movieContext.Movies.FindAsync(id);

            // Returns a 404 status if the movie is not found.
            if (movie is null)
            {
                return NotFound();
            }

            // Removes the movie from the database context.
            _movieContext.Movies.Remove(movie);
            // Saves changes asynchronously to delete the movie from the database.
            await _movieContext.SaveChangesAsync();

            // Returns a 204 status indicating the deletion was successful.
            return NoContent();
        }
    }
}
