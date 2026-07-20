using Microsoft.EntityFrameworkCore;
using Q1_WebAPI.Data;
using Q1_WebAPI.Models;
using static Q1_WebAPI.Tests.TestDb;

namespace Q1_WebAPI.Tests;

public class DbContextConfigurationTests
{
    [Fact]
    public void AppDbContext_CanBeConstructed_WithoutThrowing()
    {
        // Arrange & Act
        using var context = CreateInMemoryDbContext();

        // Assert
        Assert.NotNull(context);
        Assert.NotNull(context.Model);
    }

    [Fact]
    public void AppDbContext_Model_CanBeValidated()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var model = context.Model;

        // Assert
        Assert.NotNull(model);
        // If model creation fails, exception is thrown here
    }

    [Fact]
    public void MovieGenre_HasCompositeKey_WithMovieIdAndGenreId()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieGenreEntityType = context.Model.FindEntityType(typeof(MovieGenre));
        var primaryKey = movieGenreEntityType?.FindPrimaryKey();
        var keyProperties = primaryKey?.Properties;

        // Assert
        Assert.NotNull(movieGenreEntityType);
        Assert.NotNull(primaryKey);
        Assert.NotNull(keyProperties);
        Assert.Equal(2, keyProperties.Count);

        var propertyNames = keyProperties.Select(p => p.Name).OrderBy(n => n).ToList();
        Assert.Contains("MovieId", propertyNames);
        Assert.Contains("GenreId", propertyNames);
    }

    [Fact]
    public void MovieStar_HasCompositeKey_WithMovieIdAndStarId()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieStarEntityType = context.Model.FindEntityType(typeof(MovieStar));
        var primaryKey = movieStarEntityType?.FindPrimaryKey();
        var keyProperties = primaryKey?.Properties;

        // Assert
        Assert.NotNull(movieStarEntityType);
        Assert.NotNull(primaryKey);
        Assert.NotNull(keyProperties);
        Assert.Equal(2, keyProperties.Count);

        var propertyNames = keyProperties.Select(p => p.Name).OrderBy(n => n).ToList();
        Assert.Contains("MovieId", propertyNames);
        Assert.Contains("StarId", propertyNames);
    }

    [Fact]
    public void MovieGenre_TableName_IsMappedToMovieGenreTable()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieGenreEntityType = context.Model.FindEntityType(typeof(MovieGenre));
        var tableName = movieGenreEntityType?.GetTableName();

        // Assert
        Assert.NotNull(movieGenreEntityType);
        Assert.Equal("Movie_Genre", tableName);
    }

    [Fact]
    public void MovieStar_TableName_IsMappedToMovieStarTable()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieStarEntityType = context.Model.FindEntityType(typeof(MovieStar));
        var tableName = movieStarEntityType?.GetTableName();

        // Assert
        Assert.NotNull(movieStarEntityType);
        Assert.Equal("Movie_Star", tableName);
    }

    [Fact]
    public void Movie_Director_Relationship_IsForeignKeyOptional()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieEntityType = context.Model.FindEntityType(typeof(Movie));
        var directorNavigation = movieEntityType?.FindNavigation(nameof(Movie.Director));
        var foreignKey = directorNavigation?.ForeignKey;

        // Assert
        Assert.NotNull(movieEntityType);
        Assert.NotNull(directorNavigation);
        Assert.NotNull(foreignKey);
        Assert.False(foreignKey.IsRequired, "Movie.Director relationship should be optional (IsRequired = false)");
        Assert.Single(foreignKey.Properties, property => property.Name == nameof(Movie.DirectorId));
    }

    [Fact]
    public void Movie_Producer_Relationship_IsForeignKeyOptional()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieEntityType = context.Model.FindEntityType(typeof(Movie));
        var producerNavigation = movieEntityType?.FindNavigation(nameof(Movie.Producer));
        var foreignKey = producerNavigation?.ForeignKey;

        // Assert
        Assert.NotNull(movieEntityType);
        Assert.NotNull(producerNavigation);
        Assert.NotNull(foreignKey);
        Assert.False(foreignKey.IsRequired, "Movie.Producer relationship should be optional (IsRequired = false)");
        Assert.Single(foreignKey.Properties, property => property.Name == nameof(Movie.ProducerId));
    }

    [Fact]
    public void Director_Movies_NavigationCollection_IsConfigured()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var directorEntityType = context.Model.FindEntityType(typeof(Director));
        var moviesNavigation = directorEntityType?.FindNavigation(nameof(Director.Movies));

        // Assert
        Assert.NotNull(directorEntityType);
        Assert.NotNull(moviesNavigation);
        Assert.True(moviesNavigation.IsCollection, "Director.Movies should be a collection navigation");
    }

    [Fact]
    public void DbContext_DbSets_AreAllDefined()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Assert - verify all DbSet properties exist and are not null
        Assert.NotNull(context.Directors);
        Assert.NotNull(context.Movies);
        Assert.NotNull(context.Producers);
        Assert.NotNull(context.Stars);
        Assert.NotNull(context.Genres);
        Assert.NotNull(context.MovieGenres);
        Assert.NotNull(context.MovieStars);
    }

    [Fact]
    public void Movie_DirectorId_ForeignKey_PointsToDirector()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieEntityType = context.Model.FindEntityType(typeof(Movie));
        var directorIdProperty = movieEntityType?.FindProperty(nameof(Movie.DirectorId));
        var foreignKeys = movieEntityType?.GetForeignKeys();
        var directorFk = foreignKeys?.FirstOrDefault(fk => fk.Properties.Any(p => p.Name == nameof(Movie.DirectorId)));

        // Assert
        Assert.NotNull(movieEntityType);
        Assert.NotNull(directorIdProperty);
        Assert.NotNull(directorFk);
        Assert.Equal(typeof(Director), directorFk.PrincipalEntityType.ClrType);
    }

    [Fact]
    public void Movie_ProducerId_ForeignKey_PointsToProducer()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieEntityType = context.Model.FindEntityType(typeof(Movie));
        var producerIdProperty = movieEntityType?.FindProperty(nameof(Movie.ProducerId));
        var foreignKeys = movieEntityType?.GetForeignKeys();
        var producerFk = foreignKeys?.FirstOrDefault(fk => fk.Properties.Any(p => p.Name == nameof(Movie.ProducerId)));

        // Assert
        Assert.NotNull(movieEntityType);
        Assert.NotNull(producerIdProperty);
        Assert.NotNull(producerFk);
        Assert.Equal(typeof(Producer), producerFk.PrincipalEntityType.ClrType);
    }

    [Fact]
    public void AllEntityTypes_AreDefined_InModel()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var model = context.Model;
        var entityTypes = model.GetEntityTypes().Select(et => et.ClrType).ToList();

        // Assert
        Assert.Contains(typeof(Director), entityTypes);
        Assert.Contains(typeof(Movie), entityTypes);
        Assert.Contains(typeof(Producer), entityTypes);
        Assert.Contains(typeof(Star), entityTypes);
        Assert.Contains(typeof(Genre), entityTypes);
        Assert.Contains(typeof(MovieGenre), entityTypes);
        Assert.Contains(typeof(MovieStar), entityTypes);
    }

    [Fact]
    public void Movie_Properties_HaveExpectedConfiguration()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var movieEntityType = context.Model.FindEntityType(typeof(Movie));

        // Assert
        Assert.NotNull(movieEntityType);

        // Verify all expected properties exist
        var propertyNames = movieEntityType.GetProperties().Select(p => p.Name).ToList();
        Assert.Contains(nameof(Movie.Id), propertyNames);
        Assert.Contains(nameof(Movie.Title), propertyNames);
        Assert.Contains(nameof(Movie.ReleaseDate), propertyNames);
        Assert.Contains(nameof(Movie.Description), propertyNames);
        Assert.Contains(nameof(Movie.Language), propertyNames);
        Assert.Contains(nameof(Movie.ProducerId), propertyNames);
        Assert.Contains(nameof(Movie.DirectorId), propertyNames);
    }

    [Fact]
    public void Director_Properties_HaveExpectedConfiguration()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();

        // Act
        var directorEntityType = context.Model.FindEntityType(typeof(Director));

        // Assert
        Assert.NotNull(directorEntityType);

        var propertyNames = directorEntityType.GetProperties().Select(p => p.Name).ToList();
        Assert.Contains(nameof(Director.Id), propertyNames);
        Assert.Contains(nameof(Director.FullName), propertyNames);
        Assert.Contains(nameof(Director.Male), propertyNames);
        Assert.Contains(nameof(Director.Dob), propertyNames);
        Assert.Contains(nameof(Director.Nationality), propertyNames);
        Assert.Contains(nameof(Director.Description), propertyNames);
    }
}
