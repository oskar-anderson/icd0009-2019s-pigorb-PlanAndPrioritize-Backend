using System;
using System.Collections.Generic;

namespace Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;
        
        public string? Description { get; set; }

        public ICollection<Feature>? Features { get; set; }
    }
}
