﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace mvc.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
      return View();
    }

    public IActionResult Logout()
    {
      return SignOut("Cookies", "oidc");
    }

    public async Task<IActionResult> CallApi()
    {
      var accessToken = await HttpContext.GetTokenAsync("access_token");

      var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
      var content = await client.GetStringAsync("http://localhost:5001/api/identity");

      ViewBag.Json = JArray.Parse(content).ToString();
      return View("json");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
  }
}