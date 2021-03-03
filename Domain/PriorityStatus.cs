using System;
using System.Collections.Generic;

namespace Domain
{
    public class PriorityStatus
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public ICollection<UsersFeaturePriority>? UsersFeaturePriorities { get; set; }
    }
}