using System;

namespace Domain
{
    public class SubTask
    {
        public Guid Id { get; set; }

        public string Heading { get; set; } = default!;
        
        public string Description { get; set; } = default!;

        public Guid FeatureId { get; set; }
        public Feature? Feature { get; set; }
    }
}
