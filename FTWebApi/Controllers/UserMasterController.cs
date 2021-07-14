using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FTWebApi.Models;
using FTWebApi.Repository;
using System.Net.Mail;


namespace FTWebApi.Controllers
{
    
    public class UserMasterController : ApiController
    {
        UserMasterRepo dalUserRepo = new UserMasterRepo();

        [HttpGet]
        public HttpResponseMessage sendemail()
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("agile@cms.com");
                message.To.Add(new MailAddress("arvindgawas@yahoo.com"));
                message.Subject = "Test";
                //message.IsBodyHtml = true; //to make message body as html  
                message.Body = "Test smtp email sending";
                smtp.Port = 587;
                smtp.Host = "sampark.cms.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("agile@cms.com", "Agil@2020");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception e)
            {
                string msg;
                msg = e.Message;
                Console.WriteLine(e.Message);
            }

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            return response;

        }

        [HttpGet]
        public HttpResponseMessage send365email()
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("ReconQuery@cms.com");
                message.To.Add(new MailAddress("arvindgawas@yahoo.com"));
                message.Subject = "Test";
                //message.IsBodyHtml = true; //to make message body as html  
                message.Body = "Test smtp email sending";
                smtp.Port = 587;
                smtp.Host = "outlook.office365.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("ReconQuery@cms.com", "Rec0n@2019");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception e)
            {
                string msg;
                msg = e.Message;
                Console.WriteLine(e.Message);
            }

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            return response;

        }

        [HttpGet]
        public HttpResponseMessage sendgmail()
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("sanjugawas@gmail.com");
                message.To.Add(new MailAddress("arvindgawas@yahoo.com"));
                message.Subject = "Test";
                //message.IsBodyHtml = true; //to make message body as html  
                message.Body = "Test smtp email sending";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("sanjugawas@gmail.com", "Sanju@123");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception e) {
                string msg;
                msg = e.Message;
                Console.WriteLine(e.Message);
            }

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            return response;

        }


        [HttpGet]
        public IQueryable<FTWebApi.Models.UserMaster> GetAllUser()
        {
            return dalUserRepo.GetAllUser();
        }

        [HttpGet]
        public Int32 ValidatePriorityUser(string bank,string querytype,string userid)
        {
            return dalUserRepo.ValidatePriorityUser(bank,querytype,userid);
        }

        [HttpGet]
        public IQueryable<FTWebApi.Models.userbankmap> GetUserBankMaster()
        {
            return dalUserRepo.GetUserBankMaster();
        }

        [HttpGet]
        public FTWebApi.Models.UserMaster GetUser(String ID)
        {
            return dalUserRepo.GetUser(ID);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteUser(string ID)
        {
            try
            {
                dalUserRepo.DeleteUser(ID);
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };
                return response;
            }
            catch (Exception)
            {
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError
                };
                return response;
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateUser([FromBody] FTWebApi.Models.UserMaster objUserMaster)
        {
            try
            {

                dalUserRepo.UpdatePlan(objUserMaster);

                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                return response;

            }
            catch (Exception)
            {
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError
                };
                return response;
            }
        }

       

        [HttpPost]
        public HttpResponseMessage SaveUser([FromBody]  FTWebApi.Models.UserMaster obum)
        {
            try
            {

                dalUserRepo.InsertPlan(obum);

                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                return response;

            }
            catch (Exception)
            {
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError
                };
                return response;
            }
        }


    }
}
