﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using ParqueAPICentral.Data;

namespace CentralAPI.Services.Services
{
    public class EmailController : ControllerBase
    {
        private readonly APICentralContext _context;

        public EmailController(APICentralContext context)
        {
            _context = context;
        }

        public void EnviarEmail(byte[] qr, long clienteID, long reservaID)
        {
            var cliente = _context.Cliente.Where(c => c.ClienteID == clienteID).FirstOrDefault();

            string remetente = "pseudocompany@gmail.com";

            string destinatario = cliente.EmailCliente.ToString();

            var qrcode = new Attachment(new MemoryStream(qr), "QRCode", "image/png");

            using MailMessage mail = new MailMessage(remetente, destinatario)
            {
                Subject = "Comfirmação da reserva nº " + reservaID,
                Body = "Código QR em anexo."
            };

            mail.Attachments.Add(qrcode);
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                EnableSsl = true
            };

            NetworkCredential networkCredential = new NetworkCredential(remetente, "123456");
            smtp.Credentials = networkCredential;
            smtp.Port = 25;
            smtp.Send(mail);
        }
    }
}