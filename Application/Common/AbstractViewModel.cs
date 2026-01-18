using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.Common
{
    public class AbstractViewModel
    {
        public bool Success { get; set; }
        public Guid Id { get; set; }
        public string? Message { get; set; }
        public List<string>? LstErros { get; set; } = new List<string>();
        public List<string>? LstWarning { get; set; } = new List<string>();

    }
}
