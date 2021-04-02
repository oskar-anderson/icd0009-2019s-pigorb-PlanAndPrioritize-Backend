using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class CategoryEditBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;
        
        public string? Description { get; set; }
    }
    
    public class CategoryBllDto : CategoryEditBllDto
    {
        public int Count { get; set; }
        
        public int InProgress { get; set; }
        
        public int Finished { get; set; }
    }
}