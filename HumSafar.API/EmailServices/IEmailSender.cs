using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumSafar.API.EmailServices
{
    public interface IEmailSender
    {
        public bool SendEmail(string to, string subject, string body);
    }
}
