using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetAppSqlDb.Models;using System.Diagnostics;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;


namespace DotNetAppSqlDb.Controllers
{
    public class TodosController : Controller
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: Todos
        public ActionResult Index()
        {            
            Trace.WriteLine("GET /Todos/Index");
            return View(db.Todoes.ToList());
        }
        public ActionResult UIndex()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

            //You get the user’s first and last name below:
            ViewBag.Name = userClaims?.FindFirst("name")?.Value;

            // The 'preferred_username' claim can be used for showing the username
            ViewBag.Username = userClaims?.FindFirst("preferred_username")?.Value;

            // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // TenantId is the unique Tenant Id - which represents an organization in Azure AD
            ViewBag.TenantId = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

            return View();
        }

        public ActionResult Home()
        {
            Trace.WriteLine("GET /Todos/Home");
            return View();
        }
        public ActionResult Filters()
        {
            Trace.WriteLine("GET /Todos/cal");
            return View();
        }
        public ActionResult YourList()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            string subject = ViewBag.Subject;
            Trace.WriteLine("GET /Todos/YourList");
            var model2 = from r in db.Todoes select r;
            model2 = from r in model2 where r.SUBJECTID == subject select r;
            return View(model2);
        }
        public ActionResult Pending()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            string subject = ViewBag.Subject;
            Trace.WriteLine("GET /Todos/Pending");
            var model2 = from r in db.Todoes where r.SUBJECTID == subject where r.requst == true where r.rply == false select r;
            return View(model2);
        }
        public ActionResult Requested()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            string subject = ViewBag.Subject;
            Trace.WriteLine("GET /Todos/Requested");
            var model2 = from r in db.Todoes where r.RSUBJECTID == subject where r.requst == true select r;
            return View(model2);
        }
        public ActionResult Delivery()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            string subject = ViewBag.Subject;
            Trace.WriteLine("GET /Todos/Delivery");
            var model2 = from r in db.Todoes where r.SUBJECTID == subject where r.requst == true where r.rply == true select r;
            return View(model2);
        }
        public ActionResult BRequest()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            string subject = ViewBag.Subject;
            Trace.WriteLine("GET /Todos/BRequest");
            var model2 = from r in db.Todoes where r.type == typ.REQUEST select r;
            return View(model2);
        }
        public ActionResult Reply(int? id)
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Trace.WriteLine("GET /Todos/Reply/" + id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            if (todo.SUBJECTID == null)
            {
                return RedirectToAction("Error");
            }
            if (todo.RSUBJECTID == null)
            {
                return RedirectToAction("Error");
            }
            return View(todo);
        }

        // POST: Todos/Reply
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply([Bind(Include = "id,Name,CreatedDate,DonatedDate,DonorGender,UserBirthDate,PhoneNumber,Blood_Group,Pola,Storage_Area,SUBJECTID,RSUBJECTID,requst,rply,type")] Todo todo)
        {
            Trace.WriteLine("POST /Todos/Reply/" + todo.ID);
            if (ModelState.IsValid)
            {
                if(todo.rply == false)
                    todo.RSUBJECTID = null;
                db.Entry(todo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Pending");
            }
            return View(todo);
        }
        public ActionResult Search(string ST,string BG,String PL,string TP)
        {
            Trace.WriteLine("GET /Todos/Search");
            var model1 = from r in db.Todoes select r;
            if (ST != "Any")
                model1 = from r in model1 where r.Storage_Area == ST select r;
            if (TP == "DONATION")
                model1 = from r in model1 where r.type == typ.DONATION select r;
            else
                model1 = from r in model1 where r.type == typ.REQUEST select r;
            switch(BG)
            {
                case "A" :
                    model1 = from r in model1 where r.Blood_Group == Groups.A select r;
                    break;
                case "B":
                    model1 = from r in model1 where r.Blood_Group == Groups.B select r;
                    break;
                case "AB":
                    model1 = from r in model1 where r.Blood_Group == Groups.AB select r;
                    break;
                case "O":
                    model1 = from r in model1 where r.Blood_Group == Groups.O select r;
                    break;
                default:
                    break;
            }
            switch(PL)
            {
                case "POSITIVE":
                    model1 = from r in model1 where r.Pola == pol.POSITIVE select r;
                    break;
                case "NEGATIVE":
                    model1 = from r in model1 where r.Pola == pol.NEGATIVE select r;
                    break;
                default:
                    break;
            }
            return View(model1);
        }
        public ActionResult Error()
        {
            Trace.WriteLine("GET /Shared/Error");
            return View();
        }

        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }


        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);
        }

        // GET: Todos/Details/5
        public ActionResult Details(int? id)
        {
            Trace.WriteLine("GET /Todos/Details/" + id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // GET: Todos/Create
        public ActionResult Create()
        {
            Trace.WriteLine("GET /Todos/Create");
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
           if (userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value == null)
            {
                return RedirectToAction("Error");
            }
            return View(new Todo { CreatedDate = DateTime.Now, DonatedDate = DateTime.Now, UserBirthDate = DateTime.Now, SUBJECTID = ViewBag.Subject});
        }

        // POST: Todos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,CreatedDate,DonatedDate,DonorGender,UserBirthDate,PhoneNumber,Blood_Group,Pola,Storage_Area,SUBJECTID,type")] Todo todo)
        {
            Trace.WriteLine("POST /Todos/Create");
            if (ModelState.IsValid)
            {
                todo.rply = false;
                db.Todoes.Add(todo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        // GET: Todos/Edit/5
        public ActionResult Edit(int? id)
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            Trace.WriteLine("GET /Todos/Edit/" + id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            if(todo.SUBJECTID != userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
            {
                return RedirectToAction("Error");
            }
            return View(todo);
        }

        // POST: Todos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,CreatedDate,DonatedDate,DonorGender,UserBirthDate,PhoneNumber,Blood_Group,Pola,Storage_Area,SUBJECTID,RSUBJECTID,requst,rply,type")] Todo todo)
        {
            Trace.WriteLine("POST /Todos/Edit/" + todo.ID);
            if (ModelState.IsValid)
            {
                db.Entry(todo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(todo);
        }

        // GET: Todos/Delete/5
        public ActionResult Delete(int? id)
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            Trace.WriteLine("GET /Todos/Delete/" + id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            if (todo.SUBJECTID != userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
            {
                return RedirectToAction("Error");
            }
            return View(todo);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trace.WriteLine("POST /Todos/Delete/" + id);
            Todo todo = db.Todoes.Find(id);
            db.Todoes.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteD(int? id)
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            Trace.WriteLine("GET /Todos/DeleteD/" + id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            if (todo.SUBJECTID != userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
            {
                return RedirectToAction("Error");
            }
            return View(todo);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("DeleteD")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDConfirmed(int id)
        {
            Trace.WriteLine("POST /Todos/DeleteD/" + id);
            Todo todo = db.Todoes.Find(id);
            db.Todoes.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Delivery");
        }
        public ActionResult Requests(int? id)
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Trace.WriteLine("GET /Todos/Requests/" + id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            if (ViewBag.Subject == null)
            {
                return RedirectToAction("Error");
            }
            if (todo.SUBJECTID == null)
            {
                return RedirectToAction("Error");
            }
            if (todo.RSUBJECTID != null)
            {
                return RedirectToAction("Error");
            }
            todo.RSUBJECTID = ViewBag.Subject;
            return View(todo);
        }

        // POST: Todos/Resquests
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Requests([Bind(Include = "id,Name,CreatedDate,DonatedDate,DonorGender,UserBirthDate,PhoneNumber,Blood_Group,Pola,Storage_Area,SUBJECTID,RSUBJECTID,requst,rply,type")] Todo todo)
        {
            Trace.WriteLine("POST /Todos/Requests/" + todo.ID);
            if (ModelState.IsValid)
            {
                db.Entry(todo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(todo);
        }

        public ActionResult Cancel(int? id)
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Trace.WriteLine("GET /Todos/Cancel/" + id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            if (todo.SUBJECTID == null)
            {
                return RedirectToAction("Error");
            }
            if (todo.RSUBJECTID != ViewBag.Subject)
            {
                return RedirectToAction("Error");
            }
            return View(todo);
        }

        // POST: Todos/cancel
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel([Bind(Include = "id,Name,CreatedDate,DonatedDate,DonorGender,UserBirthDate,PhoneNumber,Blood_Group,Pola,Storage_Area,SUBJECTID,RSUBJECTID,requst,rply,type")] Todo todo)
        {
            Trace.WriteLine("POST /Todos/Cancel/" + todo.ID);
            if (ModelState.IsValid)
            {
                if(todo.requst == false)
                {
                    todo.RSUBJECTID = null;
                    todo.rply = false;
                }

                db.Entry(todo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(todo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
