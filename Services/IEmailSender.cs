using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;


namespace Arfler.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        
    }
}
