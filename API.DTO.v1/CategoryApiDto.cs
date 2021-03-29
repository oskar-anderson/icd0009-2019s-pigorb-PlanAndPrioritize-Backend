using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

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
        // public ICollection<FeatureApiDto>? Features { get; set; }
    }
}