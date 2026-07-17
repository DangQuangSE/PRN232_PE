// Models/Responses/ProductResponse.cs
using System;
using System.Collections.Generic;

namespace givenAPI.Models.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? LaunchDate { get; set; }
        public string? Description { get; set; }
        public string Material { get; set; } = null!;
        public int? DesignerId { get; set; }

        public DesignerInfo? Designer { get; set; }

        public List<ReviewerInfo> Reviewers { get; set; } = new();

        public List<TagInfo> Tags { get; set; } = new();

        public class DesignerInfo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public bool Male { get; set; }
            public DateTime Dob { get; set; }
            public string Nationality { get; set; } = null!;
            public string Description { get; set; } = null!;
        }

        public class ReviewerInfo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public bool? Male { get; set; }
            public DateTime? Dob { get; set; }
            public string? Description { get; set; }
            public string? Nationality { get; set; }
        }

        public class TagInfo
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
        }
    }
}
