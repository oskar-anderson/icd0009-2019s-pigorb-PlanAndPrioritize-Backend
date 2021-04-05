using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1
{
    public class CategoryCreateApiDto
    {
        [MaxLength(512)] [MinLength(1)] public string Title { get; set; } = default!;
        
        [MaxLength(2048)] public string? Description { get; set; }
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