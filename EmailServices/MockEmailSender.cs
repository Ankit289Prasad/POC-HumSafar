using HumSafar.API.EmailServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TrendyKart.API.Services
{
    public class MockEmailSender : IEmailSender
    {
        public bool SendEmail(string to, string subject, string body)
        {
            File.AppendAllText("Email.txt", $"Email-{to}, \n{subject}- {body}\n\n");
            return true;
        }
    }
}
