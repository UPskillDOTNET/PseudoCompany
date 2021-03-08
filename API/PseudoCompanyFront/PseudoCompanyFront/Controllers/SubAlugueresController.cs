﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PseudoCompanyFront.Models;

namespace PseudoCompanyFront.Controllers
{
    public class SubAlugueresController : Controller
    {
        private readonly IConfiguration _configure;
        private readonly string apiBaseUrl;

        public SubAlugueresController(IConfiguration configuration)
        {
            _configure = configuration;
            apiBaseUrl = _configure.GetValue<string>("WebAPIBaseUrl");
        }

        // GET: SubAlugueres
        public async Task<ActionResult> Index()
        {
            using HttpClient client = new();
            string endpoint = apiBaseUrl + "/SubAlugueres";
            var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsAsync<IEnumerable<SubAluguer>>();
                readTask.Wait();

                IEnumerable<SubAluguer> sub = readTask.Result;

                return View(sub);
            }
            else
            {
                return BadRequest("Server error. Please contact administrator.");
            }
        }

        // GET: SubAlugueres/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using HttpClient client = new();
            string endpoint = apiBaseUrl + "/SubAlugueres/" + id;
            var response = await client.GetAsync(endpoint);
            var sub = await response.Content.ReadAsAsync<SubAluguer>();

            if (sub == null)
            {
                return NotFound();
            }

            return View(sub);
        }


        // GET: SubAlugueres/Create
        public async Task<IActionResult> Create()
        {
            using (HttpClient client = new())
            {
                string endpoint = apiBaseUrl + "/SubAlugueres";
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var sub = await response.Content.ReadAsAsync<List<SubAluguer>>();
            }
            return View();
        }

        // POST: SubAlugueres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubAluguerID,Preco,Reservado,NovoCliente,ReservaID")] SubAluguer sub)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new())
                {
                    StringContent content = new (JsonConvert.SerializeObject(sub), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + "/Reservas";
                    var response = await client.PostAsync(endpoint, content);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sub);
        }


        // GET: SubAlugueres/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SubAluguer sub;
            using (HttpClient client = new())
            {
                string endpoint = apiBaseUrl + "/SubAluguers/" + id;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                sub = await response.Content.ReadAsAsync<SubAluguer>();
            }
            if (sub == null)
            {
                return NotFound();
            }
            return View(sub);
        }


        // POST: SubAlugueres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SubAluguerID,Preco,Reservado,NovoCliente,ReservaID")] SubAluguer sub)
        {
            if (id != sub.SubAluguerID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                using (HttpClient client = new())
                {
                    StringContent content = new (JsonConvert.SerializeObject(sub), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + "/SubAlugueres/" + id;
                    var response = await client.PutAsync(endpoint, content);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sub);
            }

        /*
        // GET: SubAlugueres/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SubAluguer sub;
            using (HttpClient client = new())
            {
                string endpoint = apiBaseUrl + "/SubAlugueres/" + id;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                sub = await response.Content.ReadAsAsync<SubAluguer>();
            }
            if (sub == null)
            {
                return NotFound();
            }
            return View(sub);
        }


        // POST: SubAlugueres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (HttpClient client = new())
            {
                string endpoint = apiBaseUrl + "/SubAlugueres/" + id;
                var response = await client.DeleteAsync(endpoint);
            }
            return RedirectToAction(nameof(Index));
        }*/
    }
}
