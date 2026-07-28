using givenAPI.Models;
using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public static class DataInitializer
    {
        // 1. Designers
        public static List<Designer> Designers = new List<Designer>
        {
            new Designer { Id = 1, FullName = "Jony Ive", Male = true, Dob = new DateTime(1967, 2, 27), Nationality = "England", Description = "Former Chief Design Officer at Apple, known for the iPhone and iMac." },
            new Designer { Id = 2, FullName = "Yves Behar", Male = true, Dob = new DateTime(1967, 8, 1), Nationality = "Switzerland", Description = "Founder of fuseproject, known for human-centered product design." },
            new Designer { Id = 3, FullName = "Marc Newson", Male = true, Dob = new DateTime(1963, 10, 20), Nationality = "Australia", Description = "Industrial designer known for futuristic furniture and consumer goods." },
            new Designer { Id = 4, FullName = "Zaha Hadid", Male = false, Dob = new DateTime(1950, 10, 31), Nationality = "Iraq", Description = "Architect and designer known for neo-futuristic forms." },
            new Designer { Id = 5, FullName = "Philippe Starck", Male = true, Dob = new DateTime(1949, 1, 18), Nationality = "France", Description = "French designer known for playful, iconic furniture pieces." }
        };

        // 2. Manufacturers
        public static List<Manufacturer> Manufacturers = new List<Manufacturer>
        {
            new Manufacturer { Id = 1, Name = "Apple Inc." },
            new Manufacturer { Id = 2, Name = "Herman Miller" },
            new Manufacturer { Id = 3, Name = "Knoll" },
            new Manufacturer { Id = 4, Name = "Vitra" },
            new Manufacturer { Id = 5, Name = "Kartell" }
        };

        // 3. Reviewers
        public static List<Reviewer> Reviewers = new List<Reviewer>
        {
            new Reviewer { Id = 1, FullName = "John Smith", Male = true, Dob = new DateTime(1980, 5, 15), Description = "Tech enthusiast and product reviewer.", Nationality = "USA" },
            new Reviewer { Id = 2, FullName = "Sarah Johnson", Male = false, Dob = new DateTime(1985, 7, 22), Description = "Design critic and furniture expert.", Nationality = "USA" },
            new Reviewer { Id = 3, FullName = "Michael Chen", Male = true, Dob = new DateTime(1975, 3, 10), Description = "Industrial design specialist.", Nationality = "China" },
            new Reviewer { Id = 4, FullName = "Emma Wilson", Male = false, Dob = new DateTime(1990, 11, 8), Description = "Consumer product analyst.", Nationality = "England" },
            new Reviewer { Id = 5, FullName = "James Brown", Male = true, Dob = new DateTime(1982, 9, 25), Description = "Material science expert.", Nationality = "USA" }
        };

        // 4. Tags
        public static List<Tag> Tags = new List<Tag>
        {
            new Tag { Id = 1, Title = "Tech      " },
            new Tag { Id = 2, Title = "Furniture " },
            new Tag { Id = 3, Title = "Lighting  " },
            new Tag { Id = 4, Title = "Iconic    " },
            new Tag { Id = 5, Title = "Minimal   " }
        };

        // 5. Products
        public static List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "iPhone 12", LaunchDate = new DateTime(2020, 10, 23), Description = "Smartphone with A14 Bionic chip.", Material = "Aluminum", ManufacturerId = 1, DesignerId = 1 },
            new Product { Id = 2, Name = "MacBook Air", LaunchDate = new DateTime(2008, 1, 15), Description = "Ultra-thin laptop.", Material = "Aluminum", ManufacturerId = 1, DesignerId = 1 },
            new Product { Id = 3, Name = "Leaf Lamp", LaunchDate = new DateTime(2007, 5, 1), Description = "LED desk lamp with touch dimming.", Material = "Plastic", ManufacturerId = 2, DesignerId = 2 },
            new Product { Id = 4, Name = "Lockheed Lounge", LaunchDate = new DateTime(1986, 1, 1), Description = "Riveted aluminum chaise longue.", Material = "Aluminum", ManufacturerId = 3, DesignerId = 3 },
            new Product { Id = 5, Name = "Aqua Table", LaunchDate = new DateTime(2005, 1, 1), Description = "Fluid-form fiberglass table.", Material = "Fiberglass", ManufacturerId = 4, DesignerId = 4 },
            new Product { Id = 6, Name = "Louis Ghost Chair", LaunchDate = new DateTime(2002, 1, 1), Description = "Transparent polycarbonate armchair.", Material = "Polycarbonate", ManufacturerId = 5, DesignerId = 5 }
        };

        // 6. ProductReviewers
        public static List<ProductReviewer> ProductReviewers = new List<ProductReviewer>
        {
            new ProductReviewer { ProductId = 1, ReviewerId = 1 },
            new ProductReviewer { ProductId = 1, ReviewerId = 3 },
            new ProductReviewer { ProductId = 2, ReviewerId = 1 },
            new ProductReviewer { ProductId = 3, ReviewerId = 2 },
            new ProductReviewer { ProductId = 3, ReviewerId = 4 },
            new ProductReviewer { ProductId = 4, ReviewerId = 2 },
            new ProductReviewer { ProductId = 5, ReviewerId = 4 },
            new ProductReviewer { ProductId = 6, ReviewerId = 2 },
            new ProductReviewer { ProductId = 6, ReviewerId = 5 }
        };

        // 7. ProductTags
        public static List<ProductTag> ProductTags = new List<ProductTag>
        {
            new ProductTag { ProductId = 1, TagId = 1 },
            new ProductTag { ProductId = 1, TagId = 4 },
            new ProductTag { ProductId = 2, TagId = 1 },
            new ProductTag { ProductId = 2, TagId = 5 },
            new ProductTag { ProductId = 3, TagId = 3 },
            new ProductTag { ProductId = 3, TagId = 5 },
            new ProductTag { ProductId = 4, TagId = 2 },
            new ProductTag { ProductId = 4, TagId = 4 },
            new ProductTag { ProductId = 5, TagId = 2 },
            new ProductTag { ProductId = 6, TagId = 2 },
            new ProductTag { ProductId = 6, TagId = 4 }
        };
    }
}
