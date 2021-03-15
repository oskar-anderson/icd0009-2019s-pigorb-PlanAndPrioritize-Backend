using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.Identity
{
    public class MessageDto
    {
        [Required]
        public IList<string> Messages { get; set; } = new List<string>();
        
        public MessageDto()
        {
        }

        public MessageDto(params string[] messages)
        {
            Messages = messages;
        }

    }
}
