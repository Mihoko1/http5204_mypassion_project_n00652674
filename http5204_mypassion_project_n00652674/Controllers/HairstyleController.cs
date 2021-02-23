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
    public class HairstyleController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static HairstyleController()
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

        // GET: Hairstyle
        public ActionResult List()
        {
            string url = "hairstyledata/gethairstyles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<HairstyleDto> SelectedHairStyles = response.Content.ReadAsAsync<IEnumerable<HairstyleDto>>().Result;
                return View(SelectedHairStyles);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Hairstyle/Details/5
        public ActionResult Details(int id)
        {
            ShowHairstyle ViewModel = new ShowHairstyle();
            string url = "hairstyledata/findhairstyle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into hairstyle data transfer object
                HairstyleDto SelectedHairStyle = response.Content.ReadAsAsync<HairstyleDto>().Result;
                ViewModel.hairstyle = SelectedHairStyle;


                url = "hairstyledata/findmemberforhairstyle/" + id;
                response = client.GetAsync(url).Result;
                MemberDto SelectedMember = response.Content.ReadAsAsync<MemberDto>().Result;
                ViewModel.member = SelectedMember;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Hairstyle/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Hairstyle/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Hairstyle HairstyleInfo)
        {
            Debug.WriteLine(HairstyleInfo.HairstyleID);
            string url = "hairstyledata/addhairstyle";
            Debug.WriteLine(jss.Serialize(HairstyleInfo));
            HttpContent content = new StringContent(jss.Serialize(HairstyleInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int hairstyleid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = hairstyleid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Hairstyle/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateHairstyle ViewModel = new UpdateHairstyle();

            string url = "hairstyledata/findhairstyle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
        
                HairstyleDto SelectedHairstyle = response.Content.ReadAsAsync<HairstyleDto>().Result;
                ViewModel.hairstyle = SelectedHairstyle;

                url = "memberdata/getmembers";
                response = client.GetAsync(url).Result;
                IEnumerable<MemberDto> PotentialMembers = response.Content.ReadAsAsync<IEnumerable<MemberDto>>().Result;
                ViewModel.allmembers = PotentialMembers;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Hairstyle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Hairstyle HairstyleInfo, HttpPostedFileBase HairstylePic)
        {
            Debug.WriteLine(HairstyleInfo.HairstyleID);
            string url = "hairstyledata/updatehairstyle/" + id;
            Debug.WriteLine(jss.Serialize(HairstyleInfo));
            HttpContent content = new StringContent(jss.Serialize(HairstyleInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                //Send over image data for hairstyle
                url = "hairstyledata/updatehairstylepic/" + id;
                //Debug.WriteLine("Received Hairstyle picture " + HairstylePic.FileName);
                if (HairstylePic != null)
                {
                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(HairstylePic.InputStream);
                    requestcontent.Add(imagecontent, "HairstylePic", HairstylePic.FileName);
                    response = client.PostAsync(url, requestcontent).Result;
                }
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Hairstyle/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "hairstyledata/findhairstyle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine("id:" + id);
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into hairstyle data transfer object
                HairstyleDto SelectedHairstyle = response.Content.ReadAsAsync<HairstyleDto>().Result;
                return View(SelectedHairstyle);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // POST: Hairstyle/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "hairstyledata/deletehairstyle/" + id;
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
