﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParqueAPICentral.Entities;
using ParqueAPICentral.Models;
using ParqueAPICentral.Repositories;
using Microsoft.Extensions.Configuration;
using ParqueAPICentral.DTO;
using ParqueAPICentral.Data;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Drawing;
using QRCoder;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Web.Http;
using Microsoft.Extensions.DependencyInjection;


namespace ParqueAPICentral.Services
{
    public class ReservaService : ControllerBase
    {
        private readonly IReservaRepository _repo;
        private readonly ParquesService _service;
        private readonly ReservaCentralService _serviceR;
        private readonly ClienteService _serviceC;
        private readonly LugaresService _serviceL;
        private readonly FaturaService _serviceF;
        private readonly SubAluguerService _serviceS;

        public ReservaService(IReservaRepository repo, ParquesService serviceP, ReservaCentralService serviceR, ClienteService serviceC, LugaresService serviceL, FaturaService serviceF, SubAluguerService serviceS)
        {
            this._repo = repo;
            this._service = serviceP;
            this._serviceR = serviceR;
            this._serviceC = serviceC;
            this._serviceL = serviceL;
            this._serviceF = serviceF;
            this._serviceS = serviceS;
        }

        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _serviceR.GetAllReservasCentralAsync();
        }

        public async Task<ActionResult<Reserva>> GetReservaById(long id)
        {
            return await _serviceR.GetReservaById(id);
        }


        public async Task<ActionResult<IEnumerable<ReservaPrivateDTO>>> GetAllReservasByParque(long id)
        {
            var parque = await _service.GetParqueById(id);
            if (await _service.ParqueExist(id) == false)
            {
                return NotFound("Parque nao existe");
            }
            using var client = new HttpClient();
            UserInfo user = new UserInfo();
            StringContent contentUser = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            try
            {
                var responseLogin = await client.PostAsync(parque.Value.Url + "users/authenticate", contentUser);
                dynamic tokenresponsecontent = await responseLogin.Content.ReadAsAsync<object>();
                string rtoken = tokenresponsecontent.jwtToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", rtoken);
                string endpoint = parque.Value.Url + "Reservas/";
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var ListaReservas = await response.Content.ReadAsAsync<List<ReservaPrivateDTO>>();
                return ListaReservas;
            }
            catch (HttpRequestException)
            {
                return NotFound("API do Parque " + parque.Value.NomeParque + " não conectada.");
            }

        }        //========================================================================>>reservaAPI
        public async Task<ActionResult<Reserva>> CancelarReserva(long reservaID)
        {
            var reserv = _serviceR.GetReservaById(reservaID);
            var reservaAPIID = reserv.Result.Value.ReservaAPI;
            var parqueID = reserv.Result.Value.ParqueID;
            var parque = await _service.GetParqueById(parqueID);

            using HttpClient client = new HttpClient();
            string endpoint = parque.Value.Url + "reservas/cancelar/" + reservaAPIID;

            try
            {
                var ReservaCentral = _serviceR.GetAllReservasCentralAsync().Result.Value.
                    Where(r => r.ParqueID == parqueID).Where(rr => rr.ReservaAPI == reservaAPIID).FirstOrDefault();

                var cliente_ = await _serviceC.GetClienteById(ReservaCentral.ClienteID);

                var fatura = _serviceF.GetAllFaturas().Result.Value.Where(f => f.ReservaID == ReservaCentral.ReservaID).FirstOrDefault();

                if (fatura != null)

                    if (fatura != null)
                {
                    float precoFatura = fatura.PrecoFatura;

                    await _serviceC.UpdatePagamentoCliente(cliente_.Value.ClienteID, precoFatura);
                }

                var deleteTask = client.DeleteAsync(endpoint);

                var reservaCentral = await _serviceR.DeleteReservaCentral(reservaID);

                return NoContent();

            }
            catch (HttpRequestException)
            {
                return NotFound("API do Parque " + parque.Value.NomeParque + " não conectada.");
            }
        }


        public async Task<ActionResult<ReservaPrivateDTO>> PostReservaByData(ReservaPrivateDTO dto)
        {
            var parque = await _service.GetParqueById(dto.ParqueID);
            using var client = new HttpClient();
            try
            {
                var rtoken = await GetToken(parque.Value.Url + "users/authenticate");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", rtoken);
                //var i = _serviceL.GetLugaresDisponiveisComSubAlugueres(dto.DataInicio.ToString(), dto.DataFim.ToString(), dto.ParqueID).Result.Value.FirstOrDefault();
                //var reserva = new ReservaPrivateDTO(DateTime.Now, dto.DataInicio, dto.DataFim, i.LugarID);

                if (dto.DataInicio >= dto.DataFim)
                {
                    return NotFound("Data inválida");
                }
              
                    StringContent reserva_ = new StringContent(JsonConvert.
                        SerializeObject(dto), Encoding.UTF8, "application/json");
                    var response2 = await client.
                        PostAsync(parque.Value.Url + "reservas/", reserva_);
                    var UltimaReservaAPI = await GetUltimaReservaPrivate(dto.ParqueID);
                    var reservaCentral = new Reserva(dto.ParqueID, UltimaReservaAPI.Value.ReservaID, dto.ClienteID, dto.LugarID, dto.DataReserva, dto.DataInicio, dto.DataFim);
                    await _serviceR.CriarReservaCentral(reservaCentral);
                    var qrCode = GerarQRcode(UltimaReservaAPI.Value);
                    await EnviarEmail(qrCode.Value, dto.ClienteID, UltimaReservaAPI.Value.ReservaID);

                    return CreatedAtAction(nameof(PostReservaByData), new { id = dto.ReservaID }, dto);
            
            }
            catch (HttpRequestException)
            {
                return NotFound("API do Parque " + parque.Value.NomeParque + " não conectada.");
            }
        }

        //GET Lugares disponíveis de ParqueID by Data1 e Data2

        public async Task<ActionResult<ReservaPrivateDTO>> GetUltimaReservaPrivate(long parqueid)
        {
            var parque = await _service.GetParqueById(parqueid);
            ReservaPrivateDTO reserva;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var rtoken = await GetToken(parque.Value.Url + "users/authenticate");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", rtoken);

                    var response3 = await client.
                       GetAsync(parque.Value.Url + "reservas/");

                    List<ReservaPrivateDTO> ListaLugarUltimo = await response3.
                     Content.ReadAsAsync<List<ReservaPrivateDTO>>();

                    reserva = ListaLugarUltimo.
                    OrderByDescending(r => r.ReservaID).FirstOrDefault();


                    return reserva;
                }
                catch (HttpRequestException)
                {
                    return NotFound("API do Parque " + parque.Value.NomeParque + " não conectada.");
                }
            }
        }
        
        public async Task<string> GetToken(string apiBaseUrlPrivado)
        {
            using (HttpClient client = new HttpClient())
            {

                UserInfo user = new UserInfo();
                StringContent contentUser = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var responseLogin = await client.PostAsync(apiBaseUrlPrivado, contentUser);

                dynamic tokenresponsecontent = await responseLogin.Content.ReadAsAsync<object>();
                string rtoken = tokenresponsecontent.jwtToken;

                return rtoken;
            }
        }

        public ActionResult<byte[]> GerarQRcode(ReservaPrivateDTO reserva)
        {
            var qrInfo = "Reserva: " + reserva.ReservaID
                   + "\nLugar: " + reserva.LugarID
                   + "\nData de Inicio: " + reserva.DataInicio
                   + "\nData de Fim: " + reserva.DataFim;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrInfo, QRCodeGenerator.ECCLevel.Q);

            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            return BitmapToBytes(qrCodeImage);
        }


        private static byte[] BitmapToBytes(Bitmap img)
        {
            using MemoryStream stream = new MemoryStream();

            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

            return stream.ToArray();
        }

        public async Task EnviarEmail(byte[] qrCode, long ClienteID, long ReservaID)
        {
            var cliente = await _serviceC.GetClienteById(ClienteID);

            string remetente = "pseudocompany2020@gmail.com";

            string destinatario = cliente.Value.EmailCliente;

            var qrcode = new Attachment(new MemoryStream(qrCode), "QRCode", "image/png");

            using MailMessage mail = new MailMessage(remetente, destinatario)
            {
                Subject = "Confirmação da reserva nº " + ReservaID,
                Body = "A sua reserva está confirmada.\n\nO código QR relativo à sua reserva encontra-se em anexo."
            };

            mail.Attachments.Add(qrcode);
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                EnableSsl = true
            };

            NetworkCredential networkCredential = new NetworkCredential(remetente, "PseudoPark255");
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);
        }


    }
}
