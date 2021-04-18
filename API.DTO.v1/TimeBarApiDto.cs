using System;

namespace API.DTO.v1
{
    public class TimeBarApiDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Label { get; set; } = default!;
    }
}