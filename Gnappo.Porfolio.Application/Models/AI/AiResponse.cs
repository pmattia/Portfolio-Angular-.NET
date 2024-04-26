using System.Collections.Generic;

namespace Gnappo.Portfolio.Domain.Models
{
    public class AiResponse
    {
        public string Text { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> DocumentIds { get; set; }
    }
}
