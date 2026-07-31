using givenAPI.Models;
using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public static class DataInitializer
    {
        // 1. Directors
        public static List<Director> Directors = new List<Director>
        {
            new Director { Id = 1, FullName = "Christopher Nolan", Male = true, Dob = new DateTime(1970, 7, 30), Nationality = "England", Description = "British-American filmmaker known for Inception and The Dark Knight trilogy." },
            new Director { Id = 2, FullName = "Steven Spielberg", Male = true, Dob = new DateTime(1946, 12, 18), Nationality = "USA", Description = "American director behind Jaws, E.T. and Jurassic Park." },
            new Director { Id = 3, FullName = "Bong Joon-ho", Male = true, Dob = new DateTime(1969, 9, 14), Nationality = "South Korea", Description = "South Korean director known for Parasite and Snowpiercer." },
            new Director { Id = 4, FullName = "Kathryn Bigelow", Male = false, Dob = new DateTime(1951, 11, 27), Nationality = "USA", Description = "American director known for The Hurt Locker and Zero Dark Thirty." },
            new Director { Id = 5, FullName = "Greta Gerwig", Male = false, Dob = new DateTime(1983, 8, 4), Nationality = "USA", Description = "American director known for Lady Bird and Barbie." }
        };

        // 2. Producers
        public static List<Producer> Producers = new List<Producer>
        {
            new Producer { Id = 1, Name = "Warner Bros. Pictures" },
            new Producer { Id = 2, Name = "Universal Pictures" },
            new Producer { Id = 3, Name = "CJ Entertainment" },
            new Producer { Id = 4, Name = "Legendary Pictures" },
            new Producer { Id = 5, Name = "Working Title Films" }
        };

        // 3. Genres
        public static List<Genre> Genres = new List<Genre>
        {
            new Genre { Id = 1, Title = "Sci-Fi" },
            new Genre { Id = 2, Title = "Action" },
            new Genre { Id = 3, Title = "Thriller" },
            new Genre { Id = 4, Title = "Drama" },
            new Genre { Id = 5, Title = "Comedy" }
        };

        // 4. Stars (added for realism, even though question 1 shows empty stars)
        public static List<Star> Stars = new List<Star>
        {
            new Star { Id = 1, FullName = "Leonardo DiCaprio", Male = true, Dob = new DateTime(1974, 11, 11), Description = "American actor known for Inception and Titanic.", Nationality = "USA" },
            new Star { Id = 2, FullName = "Sam Neill", Male = true, Dob = new DateTime(1947, 9, 14), Description = "New Zealand actor known for Jurassic Park.", Nationality = "New Zealand" }
        };

        // 5. Movies
        public static List<Movie> Movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Inception", ReleaseDate = new DateTime(2010, 7, 16), Description = "A thief who steals corporate secrets through dream-sharing technology.", Language = "English", ProducerId = 1, DirectorId = 1 },
            new Movie { Id = 2, Title = "The Dark Knight", ReleaseDate = new DateTime(2008, 7, 18), Description = "Batman faces the Joker in Gotham City.", Language = "English", ProducerId = 1, DirectorId = 1 },
            new Movie { Id = 3, Title = "Jurassic Park", ReleaseDate = new DateTime(1993, 6, 11), Description = "A theme park with cloned dinosaurs goes wrong.", Language = "English", ProducerId = 2, DirectorId = 2 },
            new Movie { Id = 4, Title = "E.T. the Extra-Terrestrial", ReleaseDate = new DateTime(1982, 6, 11), Description = "A boy befriends a stranded alien.", Language = "English", ProducerId = 2, DirectorId = 2 },
            new Movie { Id = 5, Title = "Parasite", ReleaseDate = new DateTime(2019, 5, 30), Description = "A poor family schemes to become employed by a wealthy family.", Language = "Korean", ProducerId = 3, DirectorId = 3 },
            new Movie { Id = 6, Title = "Zero Dark Thirty", ReleaseDate = new DateTime(2012, 12, 19), Description = "The decade-long hunt for Osama bin Laden.", Language = "English", ProducerId = 4, DirectorId = 4 },
            new Movie { Id = 7, Title = "Barbie", ReleaseDate = new DateTime(2023, 7, 21), Description = "Barbie and Ken venture from Barbieland to the real world.", Language = "English", ProducerId = 5, DirectorId = 5 }
        };

        // 6. MovieGenres (no associations in the seed data, so empty list)
        public static List<MovieGenre> MovieGenres = new List<MovieGenre>
        {
        };

        // 7. MovieStars (no associations in the seed data, so empty list)
        public static List<MovieStar> MovieStars = new List<MovieStar>
        {
        };
    }
}
