using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using http5204_mypassion_project_n00652674.Models;
using http5204_mypassion_project_n00652674.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace http5204_mypassion_project_n00652674.Controllers
{
    public class SalonController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static SalonController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44328/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }


        // GET: Salon/List
        public ActionResult List()
        {
            string url = "salondata/getsalons";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<SalonDto> SelectedSalons = response.Content.ReadAsAsync<IEnumerable<SalonDto>>().Result;
                return View(SelectedSalons);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Salon/Details/5
        public ActionResult Details(int id)
        {
            ShowSalon ViewModel = new ShowSalon();
            string url = "salondata/findsalon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                SalonDto SelectedSalon = response.Content.ReadAsAsync<SalonDto>().Result;
                ViewModel.salon = SelectedSalon;

                url = "salondata/getmembersforsalon/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<MemberDto> SelectedMembers = response.Content.ReadAsAsync<IEnumerable<MemberDto>>().Result;
                ViewModel.salonmembers = SelectedMembers;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Salon/Create
        public ActionResult Create()
        {
            return View();
        }


        // Post: Salon/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Salon SalonInfo)
        {
            Debug.WriteLine(SalonInfo.SalonName);
            string url = "Salondata/addSalon";
            Debug.WriteLine(jss.Serialize(SalonInfo));
            HttpContent content = new StringContent(jss.Serialize(SalonInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int salonid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = salonid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }



        // GET: Salon/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "salondata/findsalon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                SalonDto SelectedSalon = response.Content.ReadAsAsync<SalonDto>().Result;
                return View(SelectedSalon);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Salon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Salon SalonInfo, HttpPostedFileBase SalonPic)
        {
            Debug.WriteLine(SalonInfo.SalonName);
            string url = "salondata/updatesalon/" + id;
            Debug.WriteLine(jss.Serialize(SalonInfo));
            HttpContent content = new StringContent(jss.Serialize(SalonInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //Send over image data for Salon
                url = "salondata/updatesalonpic/" + id;
                // Debug.WriteLine("Received Salon picture " + SalonPic.FileName);
                if (SalonPic != null) {
                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(SalonPic.InputStream);
                    requestcontent.Add(imagecontent, "SalonPic", SalonPic.FileName);
                    response = client.PostAsync(url, requestcontent).Result;
                }
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Salon/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "salondata/findsalon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                SalonDto SelectedSalon = response.Content.ReadAsAsync<SalonDto>().Result;
                return View(SelectedSalon);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // POST: Salon/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "salondata/deletesalon/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
