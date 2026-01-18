using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.Auth.Commands
{
    public class AuthCommandBase
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
