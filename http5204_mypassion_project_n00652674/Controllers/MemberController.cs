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
    public class MemberController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static MemberController()
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
        // GET: Member/List
        public ActionResult List()
        {

            string url = "memberdata/getmembers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<MemberDto> SelectedMembers = response.Content.ReadAsAsync<IEnumerable<MemberDto>>().Result;
                return View(SelectedMembers);

            }
            else
            {
                return RedirectToAction("Error");
            }
            //return View();
        }

        // GET: Member/Details/5
        public ActionResult Details(int id)
        {
            ShowMember ViewModel = new ShowMember();
            string url = "memberdata/findmember/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                MemberDto SelectedMember = response.Content.ReadAsAsync<MemberDto>().Result;
              ViewModel.member = SelectedMember;


            url = "memberdata/findsalonformember/" + id;
            response = client.GetAsync(url).Result;
            SalonDto SelectedSalon = response.Content.ReadAsAsync<SalonDto>().Result;
            ViewModel.salon = SelectedSalon;

            return View(ViewModel);
            }
            else
            {
              return RedirectToAction("Error");
            }
           
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
         public ActionResult Create(Member MemberInfo)
        {
            Debug.WriteLine(MemberInfo.FirstName);
            string url = "memberdata/addmember";
            Debug.WriteLine(jss.Serialize(MemberInfo));
            HttpContent content = new StringContent(jss.Serialize(MemberInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int memberid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = memberid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Member/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateMember ViewModel = new UpdateMember();

            string url = "memberdata/findmember/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                
                MemberDto SelectedMember= response.Content.ReadAsAsync<MemberDto>().Result;
                ViewModel.member = SelectedMember;

                url = "salondata/getsalons";
                response = client.GetAsync(url).Result;
                IEnumerable<SalonDto> PotentialSalons = response.Content.ReadAsAsync<IEnumerable<SalonDto>>().Result;
                ViewModel.allsalons = PotentialSalons;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

 

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Member MemberInfo, HttpPostedFileBase MemberPic)
        {
            Debug.WriteLine(MemberInfo.FirstName);
            string url = "memberdata/updatemember/" + id;
            Debug.WriteLine(jss.Serialize(MemberInfo));
            HttpContent content = new StringContent(jss.Serialize(MemberInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                //Send over image data for player
                url = "memberdata/updatememberpic/" + id;
                Debug.WriteLine("Received player picture " + MemberPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(MemberPic.InputStream);
                requestcontent.Add(imagecontent, "MemberPic", MemberPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Member/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "memberdata/deletemember/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "memberdata/findmember/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                
                MemberDto SelectedMember = response.Content.ReadAsAsync<MemberDto>().Result;
                return View(SelectedMember);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}
