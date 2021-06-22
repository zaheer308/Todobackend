using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MailKit;
using MimeKit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TodoBackend.Data;
using TodoBackend.Models;

namespace TodoBackend.Data
{
    public class EmailService : IEmailService
    {
        private string FromParameterName = "Todolist Verification";


        private string FromParameterAddressEx = "todolistapp442@outlook.com";



        public EmailService()
        {
        }


        public MimeMessage GetMessageObjForAuthLoginEmailEx(string emailID, string emailWording)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(FromParameterName, FromParameterAddressEx));
            message.To.Add(new MailboxAddress(emailID));
            message.Subject = "Todolist Verification";
            message.Body = new TextPart("Plain") { Text = emailWording };

            return message;
        }

        public MimeMessage GetMessageObjForEmailEx(string emailID)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(FromParameterName, FromParameterAddressEx));
            message.To.Add(new MailboxAddress(emailID));

            return message;
        }


    }
}