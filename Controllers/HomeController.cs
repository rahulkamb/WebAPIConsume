﻿using APIConsume.Models;
using APIConsume.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace APIConsume.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _userKey;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _userKey = configuration["Authentication:userKey"];
        }

        [HttpGet]
        public IActionResult Get(int pg = 1,int pageSize=10, string SearchText="")
        {
            List<EmployeeVM> employees = new List<EmployeeVM>();
            string empAPI = _configuration["EmpAPI"];
            //string user_key = _configuration["Authentication:userKey"];
            if (pg <= 0)
            {
                pg = 1;
            }
            using(var client =  new HttpClient())
            {
                client.BaseAddress = new Uri(empAPI);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("user_key", _userKey);

                HttpResponseMessage response = client.GetAsync("get/Get-AllEmployee").Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    employees = JsonConvert.DeserializeObject<List<EmployeeVM>>(jsonData);
                    int totalItems = employees.Count();
                    if (!String.IsNullOrEmpty(SearchText))
                    {
                        
                        var emp = employees.Where(str => str.FirstName.Contains(SearchText)).ToList();
                        employees.Clear();
                        employees = emp;
                    }
                    var pagination = new Pagination(totalItems,pg, pageSize);
                    var recSkip = ((pg - 1) * pageSize);
                    var data = employees.Skip(recSkip).Take(pageSize).ToList();
                    ViewBag.pager = pagination;
                    return View(data);
                }
                else
                {
                    return NotFound();
                }
            }
            
        }


        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateEmployee(EmployeeVM employee)
        {
            List<EmployeeVM> employees = new List<EmployeeVM>();
            string empAPI = _configuration["EmpAPI"];
            //string userKey = _configuration["Authentication:userKey"];

            if (ModelState.IsValid)
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(empAPI);
                    client.DefaultRequestHeaders.Add("user_key", _userKey);
                    var json = JsonConvert.SerializeObject(employee);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync("Create/Add-NewUser",content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Get");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create product.");
                    }
                }
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            EmployeeVM employees = new EmployeeVM();
            string empAPI = _configuration["EmpAPI"];
            //string userKey = _configuration["Authentication:userKey"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(empAPI);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("user_key", _userKey);

                HttpResponseMessage response = client.GetAsync("get/Get-EmployeeById/" + id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    employees = JsonConvert.DeserializeObject<EmployeeVM>(jsonData);
                    return View(employees);
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            EmployeeVM employees = new EmployeeVM();
            string empAPI = _configuration["EmpAPI"];
            //string userKey = _configuration["Authentication:userKey"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(empAPI);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //https://localhost:7066/api/Home/Delete/Delete-EmployeeById/5
                client.DefaultRequestHeaders.Add("user_key", _userKey);
                HttpResponseMessage response = client.DeleteAsync("Delete/Delete-EmployeeById/" + id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    //var jsonData = response.Content.ReadAsStringAsync().Result;
                    //employees = JsonConvert.DeserializeObject<EmployeeVM>(jsonData);
                    return RedirectToAction("Get");
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}