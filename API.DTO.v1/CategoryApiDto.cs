using System;

namespace API.DTO.v1
{
    public class CategoryCreateApiDto
    {
        public string Title { get; set; } = default!;
        
        public string? Description { get; set; }
    }
    
    public class CategoryEditApiDto : CategoryCreateApiDto
    {
        public Guid Id { get; set; }
    }
    
    public class CategoryApiDto : CategoryEditApiDto
    {
        public int Count { get; set; }
        
        public int InProgress { get; set; }
        
        public int Finished { get; set; }
    }
}