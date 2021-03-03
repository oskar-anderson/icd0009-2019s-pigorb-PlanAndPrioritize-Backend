using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Feature
    {
        public Guid Id { get; set; }

        [MaxLength(64)]
        public string Heading { get; set; } = default!;

        [MaxLength(256)]
        public string Description { get; set; } = default!;

        public ICollection<SubTask>? SubTasks { get; set; }
    }
}
