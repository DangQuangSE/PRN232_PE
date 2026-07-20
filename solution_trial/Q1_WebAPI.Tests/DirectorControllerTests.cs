using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_WebAPI.Controllers;
using Q1_WebAPI.Data;
using Q1_WebAPI.Models;
using Q1_WebAPI.Models.Dtos;
using static Q1_WebAPI.Tests.TestDb;

namespace Q1_WebAPI.Tests;

public class DirectorControllerTests
{
    /// <summary>
    /// Test subclass of AppDbContext that overrides SaveChanges to throw DbUpdateException.
    /// Used to test error handling in CreateDirector endpoint.
    /// </summary>
    private class ThrowingDbContext : AppDbContext
    {
        public ThrowingDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            throw new DbUpdateException("Simulated database error");
        }
    }

    #region GetDirectors Tests

    [Fact]
    public void GetDirectors_WithMaleDirectorsInUSA_ReturnsMaleUSADirectorsOnly()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director1 = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        var director2 = new Director
        {
            Id = 2,
            FullName = "Quentin Tarantino",
            Male = true,
            Dob = new DateTime(1963, 3, 27),
            Nationality = "USA",
            Description = "Indie filmmaker"
        };

        var director3 = new Director
        {
            Id = 3,
            FullName = "Greta Gerwig",
            Male = false,
            Dob = new DateTime(1983, 8, 4),
            Nationality = "USA",
            Description = "Contemporary filmmaker"
        };

        var director4 = new Director
        {
            Id = 4,
            FullName = "Christopher Nolan",
            Male = true,
            Dob = new DateTime(1970, 7, 30),
            Nationality = "UK",
            Description = "British filmmaker"
        };

        context.Directors.AddRange(director1, director2, director3, director4);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirectors("usa", "male");

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directors = Assert.IsType<List<DirectorDto>>(okResult.Value);

        Assert.Equal(2, directors.Count);
        Assert.True(directors.All(d => d.Nationality.ToLower() == "usa"));
        Assert.True(directors.All(d => d.Gender == "Male"));
        Assert.Contains(directors, d => d.FullName == "Steven Spielberg");
        Assert.Contains(directors, d => d.FullName == "Quentin Tarantino");
    }

    [Fact]
    public void GetDirectors_WithFemaleDirectors_ReturnsFemaleDirectorsOnly()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director1 = new Director
        {
            Id = 1,
            FullName = "Greta Gerwig",
            Male = false,
            Dob = new DateTime(1983, 8, 4),
            Nationality = "USA",
            Description = "Contemporary filmmaker"
        };

        var director2 = new Director
        {
            Id = 2,
            FullName = "Denis Villeneuve",
            Male = true,
            Dob = new DateTime(1967, 10, 3),
            Nationality = "Canada",
            Description = "Canadian filmmaker"
        };

        context.Directors.AddRange(director1, director2);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirectors("usa", "female");

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directors = Assert.IsType<List<DirectorDto>>(okResult.Value);

        Assert.Single(directors);
        Assert.Equal("Greta Gerwig", directors[0].FullName);
        Assert.Equal("Female", directors[0].Gender);
    }

    [Fact]
    public void GetDirectors_CaseInsensitiveNationality_ReturnsSameResultsForDifferentCases()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act - test with different case variations
        var resultLower = controller.GetDirectors("usa", "male");
        var resultUpper = controller.GetDirectors("USA", "male");
        var resultMixed = controller.GetDirectors("UsA", "male");

        // Assert
        var okLower = Assert.IsType<OkObjectResult>(resultLower);
        var directorsLower = Assert.IsType<List<DirectorDto>>(okLower.Value);
        Assert.Single(directorsLower);

        var okUpper = Assert.IsType<OkObjectResult>(resultUpper);
        var directorsUpper = Assert.IsType<List<DirectorDto>>(okUpper.Value);
        Assert.Single(directorsUpper);

        var okMixed = Assert.IsType<OkObjectResult>(resultMixed);
        var directorsMixed = Assert.IsType<List<DirectorDto>>(okMixed.Value);
        Assert.Single(directorsMixed);
    }

    [Fact]
    public void GetDirectors_CaseInsensitiveGender_ReturnsSameResultsForDifferentCases()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act - test with different case variations of gender
        var resultLower = controller.GetDirectors("usa", "male");
        var resultUpper = controller.GetDirectors("usa", "MALE");
        var resultMixed = controller.GetDirectors("usa", "Male");

        // Assert
        var okLower = Assert.IsType<OkObjectResult>(resultLower);
        var directorsLower = Assert.IsType<List<DirectorDto>>(okLower.Value);
        Assert.Single(directorsLower);

        var okUpper = Assert.IsType<OkObjectResult>(resultUpper);
        var directorsUpper = Assert.IsType<List<DirectorDto>>(okUpper.Value);
        Assert.Single(directorsUpper);

        var okMixed = Assert.IsType<OkObjectResult>(resultMixed);
        var directorsMixed = Assert.IsType<List<DirectorDto>>(okMixed.Value);
        Assert.Single(directorsMixed);
    }

    [Fact]
    public void GetDirectors_DobStringFormatIsCorrect_Uses_M_d_yyyy_Format()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Test with date 1975-04-09, expected dobString is "4/9/1975" (no leading zeros)
        var director = new Director
        {
            Id = 1,
            FullName = "Test Director",
            Male = true,
            Dob = new DateTime(1975, 4, 9),
            Nationality = "USA",
            Description = "Test"
        };

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirectors("usa", "male");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directors = Assert.IsType<List<DirectorDto>>(okResult.Value);
        var dto = directors[0];

        Assert.Equal("4/9/1975", dto.DobString);
        Assert.NotEqual("04/09/1975", dto.DobString); // Verify no leading zeros
    }

    [Fact]
    public void GetDirectors_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act - search for non-existent combination
        var result = controller.GetDirectors("Japan", "male");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directors = Assert.IsType<List<DirectorDto>>(okResult.Value);
        Assert.Empty(directors);
    }

    [Fact]
    public void GetDirectors_AllPropertiesArePopulated()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirectors("usa", "male");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directors = Assert.IsType<List<DirectorDto>>(okResult.Value);
        var dto = directors[0];

        Assert.Equal(1, dto.Id);
        Assert.Equal("Steven Spielberg", dto.FullName);
        Assert.Equal("Male", dto.Gender);
        Assert.Equal("USA", dto.Nationality);
        Assert.Equal("Legendary filmmaker", dto.Description);
        Assert.Equal(new DateTime(1946, 12, 18), dto.Dob);
        Assert.Equal("12/18/1946", dto.DobString);
    }

    #endregion

    #region GetDirector Tests

    [Fact]
    public void GetDirector_WithValidId_Returns200OkWithDirectorData()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Equal(1, directorDto.Id);
        Assert.Equal("Steven Spielberg", directorDto.FullName);
        Assert.Equal("Male", directorDto.Gender);
        Assert.Equal("USA", directorDto.Nationality);
        Assert.Equal("Legendary filmmaker", directorDto.Description);
        Assert.Equal(new DateTime(1946, 12, 18), directorDto.Dob);
        Assert.Equal("12/18/1946", directorDto.DobString);
    }

    [Fact]
    public void GetDirector_WithNonExistentId_Returns404NotFound()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(999);

        // Assert
        Assert.NotNull(result);
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public void GetDirector_WithDirectorHavingMultipleMovies_ReturnsAllMoviesInMoviesArray()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        var movie1 = new Movie
        {
            Id = 1,
            Title = "Jaws",
            ReleaseDate = new DateTime(1975, 6, 20),
            Description = "Thriller",
            Language = "English",
            DirectorId = 1
        };

        var movie2 = new Movie
        {
            Id = 2,
            Title = "E.T.",
            ReleaseDate = new DateTime(1982, 6, 11),
            Description = "Sci-Fi",
            Language = "English",
            DirectorId = 1
        };

        director.Movies.Add(movie1);
        director.Movies.Add(movie2);

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Equal(2, directorDto.Movies.Count);
        Assert.Contains(directorDto.Movies, m => m.Title == "Jaws");
        Assert.Contains(directorDto.Movies, m => m.Title == "E.T.");
    }

    [Fact]
    public void GetDirector_WithDirectorHavingNoMovies_ReturnsEmptyMoviesArray()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Empty(directorDto.Movies);
    }

    [Fact]
    public void GetDirector_MovieWithProducer_PopulatesProducerNameCorrectly()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var producer = new Producer
        {
            Id = 1,
            Name = "Jerry Bruckheimer"
        };

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        var movie = new Movie
        {
            Id = 1,
            Title = "Top Gun",
            ReleaseDate = new DateTime(1986, 5, 16),
            Description = "Action",
            Language = "English",
            DirectorId = 1,
            ProducerId = 1,
            Producer = producer
        };

        director.Movies.Add(movie);

        context.Producers.Add(producer);
        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Single(directorDto.Movies);
        var movieDto = directorDto.Movies[0];
        Assert.Equal("Jerry Bruckheimer", movieDto.ProducerName);
        Assert.Equal(1, movieDto.ProducerId);
    }

    [Fact]
    public void GetDirector_MovieWithoutProducer_ProducerNameIsNull()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        var movie = new Movie
        {
            Id = 1,
            Title = "Jaws",
            ReleaseDate = new DateTime(1975, 6, 20),
            Description = "Thriller",
            Language = "English",
            DirectorId = 1,
            ProducerId = null
        };

        director.Movies.Add(movie);

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Single(directorDto.Movies);
        var movieDto = directorDto.Movies[0];
        Assert.Null(movieDto.ProducerName);
    }

    [Fact]
    public void GetDirector_MovieReleaseYearIsComputedFromReleaseDate()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        var movie = new Movie
        {
            Id = 1,
            Title = "Jaws",
            ReleaseDate = new DateTime(1975, 6, 20),
            Description = "Thriller",
            Language = "English",
            DirectorId = 1
        };

        director.Movies.Add(movie);

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Single(directorDto.Movies);
        var movieDto = directorDto.Movies[0];
        Assert.Equal(1975, movieDto.ReleaseYear);
    }

    [Fact]
    public void GetDirector_MovieWithNullReleaseDate_ReleaseYearIsNull()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        var movie = new Movie
        {
            Id = 1,
            Title = "Unreleased Movie",
            ReleaseDate = null,
            Description = "Thriller",
            Language = "English",
            DirectorId = 1
        };

        director.Movies.Add(movie);

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Single(directorDto.Movies);
        var movieDto = directorDto.Movies[0];
        Assert.Null(movieDto.ReleaseYear);
    }

    [Fact]
    public void GetDirector_MovieGenresAndStarsAreAlwaysEmpty()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        var movie = new Movie
        {
            Id = 1,
            Title = "Jaws",
            ReleaseDate = new DateTime(1975, 6, 20),
            Description = "Thriller",
            Language = "English",
            DirectorId = 1
        };

        director.Movies.Add(movie);

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Single(directorDto.Movies);
        var movieDto = directorDto.Movies[0];

        // Genres and Stars should always be empty lists per spec
        Assert.NotNull(movieDto.Genres);
        Assert.Empty(movieDto.Genres);
        Assert.NotNull(movieDto.Stars);
        Assert.Empty(movieDto.Stars);
    }

    [Fact]
    public void GetDirector_MovieDirectorNameIsPopulatedFromDirector()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        var director = new Director
        {
            Id = 1,
            FullName = "Steven Spielberg",
            Male = true,
            Dob = new DateTime(1946, 12, 18),
            Nationality = "USA",
            Description = "Legendary filmmaker"
        };

        var movie = new Movie
        {
            Id = 1,
            Title = "Jaws",
            ReleaseDate = new DateTime(1975, 6, 20),
            Description = "Thriller",
            Language = "English",
            DirectorId = 1
        };

        director.Movies.Add(movie);

        context.Directors.Add(director);
        context.SaveChanges();

        var controller = new DirectorController(context);

        // Act
        var result = controller.GetDirector(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var directorDto = Assert.IsType<DirectorWithMoviesDto>(okResult.Value);

        Assert.Single(directorDto.Movies);
        var movieDto = directorDto.Movies[0];
        Assert.Equal("Steven Spielberg", movieDto.DirectorName);
    }

    #endregion

    #region CreateDirector Tests

    [Fact]
    public void CreateDirector_WithValidData_Returns200OkWithRecordCount()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new DirectorController(context);

        var request = new CreateDirectorRequest
        {
            FullName = "Martin Scorsese",
            Male = true,
            Dob = new DateTime(1942, 11, 17),
            Nationality = "USA",
            Description = "Great filmmaker"
        };

        // Act
        var result = controller.CreateDirector(request);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(1, okResult.Value);
    }

    [Fact]
    public void CreateDirector_WithValidData_PersistsDirectorInDatabase()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new DirectorController(context);

        var request = new CreateDirectorRequest
        {
            FullName = "Martin Scorsese",
            Male = true,
            Dob = new DateTime(1942, 11, 17),
            Nationality = "USA",
            Description = "Great filmmaker"
        };

        // Act
        controller.CreateDirector(request);

        // Assert
        var addedDirector = context.Directors.FirstOrDefault(d => d.FullName == "Martin Scorsese");
        Assert.NotNull(addedDirector);
        Assert.Equal("Martin Scorsese", addedDirector.FullName);
        Assert.True(addedDirector.Male);
        Assert.Equal(new DateTime(1942, 11, 17), addedDirector.Dob);
        Assert.Equal("USA", addedDirector.Nationality);
        Assert.Equal("Great filmmaker", addedDirector.Description);
    }

    [Fact]
    public void CreateDirector_WithMultipleValidRequests_ReturnsCorrectRecordCountsAndPersistsAll()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new DirectorController(context);

        var request1 = new CreateDirectorRequest
        {
            FullName = "Martin Scorsese",
            Male = true,
            Dob = new DateTime(1942, 11, 17),
            Nationality = "USA",
            Description = "Great filmmaker"
        };

        var request2 = new CreateDirectorRequest
        {
            FullName = "Christopher Nolan",
            Male = true,
            Dob = new DateTime(1970, 7, 30),
            Nationality = "UK",
            Description = "Innovative filmmaker"
        };

        // Act
        var result1 = controller.CreateDirector(request1);
        var result2 = controller.CreateDirector(request2);

        // Assert
        var okResult1 = Assert.IsType<OkObjectResult>(result1);
        Assert.Equal(1, okResult1.Value);

        var okResult2 = Assert.IsType<OkObjectResult>(result2);
        Assert.Equal(1, okResult2.Value);

        var allDirectors = context.Directors.ToList();
        Assert.Equal(2, allDirectors.Count);
    }

    [Fact]
    public void CreateDirector_WhenSaveChangesThrows_Returns409ConflictWithPlainTextContent()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var throwingContext = new ThrowingDbContext(options);
        var controller = new DirectorController(throwingContext);

        var request = new CreateDirectorRequest
        {
            FullName = "Test Director",
            Male = true,
            Dob = new DateTime(1975, 4, 9),
            Nationality = "USA",
            Description = "Test"
        };

        // Act
        var result = controller.CreateDirector(request);

        // Assert
        Assert.NotNull(result);
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal(409, contentResult.StatusCode);
        Assert.Equal("text/plain", contentResult.ContentType);
        Assert.Equal("There is an error while adding.", contentResult.Content);
    }

    [Fact]
    public void CreateDirector_ErrorResponseIsNotJsonEncoded()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var throwingContext = new ThrowingDbContext(options);
        var controller = new DirectorController(throwingContext);

        var request = new CreateDirectorRequest
        {
            FullName = "Test Director",
            Male = true,
            Dob = new DateTime(1975, 4, 9),
            Nationality = "USA",
            Description = "Test"
        };

        // Act
        var result = controller.CreateDirector(request);

        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);

        // The content should NOT contain escaped quotes or JSON encoding
        Assert.DoesNotContain("\\\"", contentResult.Content);
        Assert.DoesNotContain("\"There is an error", contentResult.Content);
        Assert.Equal("There is an error while adding.", contentResult.Content);
    }

    [Fact]
    public void CreateDirector_CanHandleDirectorWithFemaleGender()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new DirectorController(context);

        var request = new CreateDirectorRequest
        {
            FullName = "Greta Gerwig",
            Male = false,
            Dob = new DateTime(1983, 8, 4),
            Nationality = "USA",
            Description = "Contemporary filmmaker"
        };

        // Act
        var result = controller.CreateDirector(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1, okResult.Value);

        var addedDirector = context.Directors.First();
        Assert.False(addedDirector.Male);
    }

    #endregion
}
