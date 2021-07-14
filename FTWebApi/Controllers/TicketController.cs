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
using System.Web;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace FTWebApi.Controllers
{
    public class TicketController : ApiController
    {
        TicketRepo dalticketrepo = new TicketRepo();

        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            string ticketno = httpRequest.Form["ticketno"];
            string filePath="";
            string FileName= "";
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    FileName = postedFile.FileName;
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            dalticketrepo.updateuploadfilepath(ticketno, filePath, FileName);

            return result;
        }

        
        public HttpResponseMessage Downloadnew(string filepath, string filname)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
          
            //Check whether File exists.
            if (!File.Exists(@filepath))
            {
                //Throw 404 (Not Found) exception if File not found.
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", filname);
                throw new HttpResponseException(response);
            }

            //Read the File into a Byte Array.
            byte[] bytes = File.ReadAllBytes(filepath);

            //Set the Response Content.
            response.Content = new ByteArrayContent(bytes);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = bytes.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = filname;

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filname));

            return response;
        }

        [HttpGet]
        public HttpResponseMessage Downloadexelfile(string fromdate,string todate,string customer,string user, string customertype,
              string region,string location,string hublocation,string cdpncm,string issuetype,string sla)
        {
           
            string html = dalticketrepo.GetReportData(fromdate, todate, customer, user, customertype,region,location,hublocation,cdpncm, issuetype, sla);
            byte[] bytes = Encoding.ASCII.GetBytes(html);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "TicketTracker.xls"
            };
            return result;
        }        


        [HttpGet]
        public IHttpActionResult DownloadFile(string filepath,string filneame)
        {
            var dataStream = new MemoryStream();

            try
            {
                if (File.Exists(@filepath))
                {
                    //converting Pdf file into bytes array  
                    var dataBytes = File.ReadAllBytes(@filepath);
                    //adding bytes to memory stream   
                    dataStream = new MemoryStream(dataBytes);
                }
                else
                {    
                    Console.WriteLine("wrong path");
                    return new eErrorResult(dataStream, Request, filneame);
                }
            }

            catch (Exception e)
            {
                string msg;
                msg = e.Message;
                Console.WriteLine(e.Message);

            }

            return new eBookResult(dataStream, Request, filneame);
        }

        public class eBookResult : IHttpActionResult
        {
            MemoryStream bookStuff;
            string PdfFileName;
            HttpRequestMessage httpRequestMessage;
            HttpResponseMessage httpResponseMessage;
            public eBookResult(MemoryStream data, HttpRequestMessage request, string filename)
            {
                bookStuff = data;
                httpRequestMessage = request;
                PdfFileName = filename;
            }
            public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
            {
                httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
                httpResponseMessage.Content = new StreamContent(bookStuff);
                //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
            }
        }

        public class eErrorResult : IHttpActionResult
        {
            MemoryStream bookStuff;
            string PdfFileName;
            HttpRequestMessage httpRequestMessage;
            HttpResponseMessage httpResponseMessage;
            public eErrorResult(MemoryStream data, HttpRequestMessage request, string filename)
            {
                bookStuff = data;
                httpRequestMessage = request;
                PdfFileName = filename;
            }
            public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
            {
                httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.NotFound);
                return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
            }
        }


        [HttpGet]
        public List<FTWebApi.Models.ticketcount> getdashboardcount(string userid, string dtticketdate)
        {
            DateTime dt = DateTime.Parse(dtticketdate);
            return dalticketrepo.getdashboardcount(userid, dt);
        }

        [HttpGet]
        public List<FTWebApi.Models.ticketcount> getadmindashboardcount(string userid, string dtticketdate)
        {
            DateTime dt = DateTime.Parse(dtticketdate);
            return dalticketrepo.getadmindashboardcount(userid, dt);
        }

        

        [HttpGet]
        public FTWebApi.Models.ticketuser ValidateUser(string ID)
        {
            return dalticketrepo.ValidateUser(ID);
        }

        [HttpGet]
        public FTWebApi.Models.ddlist getddlist(string querytype)
        {
            return dalticketrepo.getddlist(querytype);
        }

        [HttpGet]
        public FTWebApi.Models.ddlisttracker getddlisttracker()
        {
            return dalticketrepo.getddlisttracker();
        }

        [HttpGet]
        public string ProcessTAT(string datefilter)
        {
            string html;
            html = dalticketrepo.ProcessTat(datefilter);
            sendemailTAT(html);
            return "ok";
        }

        private void sendemailTAT(string html)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("rcmuatquery@cms.com");
            message.To.Add(new MailAddress("arvind.gavas@cms.com"));
            message.Subject = "RCMQuery TAT Eascalations";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = html;
            smtp.Port = 587;
            smtp.Host = "sampark.cms.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }

        [HttpGet]
        public string Sendacceptemail(string batchno)
        {
            string result;
            string fromemailid;
            result=dalticketrepo.Sendacceptemail(batchno);

            if (result=="ok")
            {
                fromemailid=dalticketrepo.getfromemailid(batchno);
                sendemailaccept(fromemailid);
            }
            return "ok";
        }

        private void sendemailaccept(string fromemailid)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("rcmuatquery@cms.com");
            ///message.To.Add(new MailAddress(fromemailid));
            //message.To.Add(new MailAddress("arvind.gavas@cms.com"));
            message.Subject = "CMS Ticket Accptance Email";
            //message.IsBodyHtml = true; //to make message body as html  
            message.Body = "CMS Ticket Accptance Email";
            smtp.Port = 587;
            smtp.Host = "sampark.cms.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }



        [HttpPost]
        public HttpResponseMessage sendlocationemail([FromBody] FTWebApi.Models.locemail objemail)
        {

            string sfilepath = objemail.filepath.Replace("Z:/", @"https://cmsliveuat.cms.com/rcmquery/api/ticket/DownloadFile?filepath=//172.19.100.157/nfsrmsuat/");
            string sfilename = "&filneame=EmailBody" + objemail.ticketno + ".html";

            //String sbody = @"https://cmsliveuat.cms.com/rcmquery/api/ticket/DownloadFile?filepath=\\172.19.100.157\nfsrmsuat\RCMEmail_Files\2021_06_25\EmailBody_2021-06-25_104528_799404.html&filneame=EmailBodyT000264.html";
            String sbody = sfilepath+ sfilename;

            StringBuilder msgbody= new StringBuilder();
            msgbody.Append("<html><head></head><body>Please click below link to download Email Body" + "<br />" + "<a href=" + sbody + ">EmailBody</a></body></html>");

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("rcmuatquery@cms.com");
                message.To.Add(new MailAddress(objemail.toemailid));   
                if (objemail.ccemailid != null && objemail.ccemailid != "")
                {
                    message.CC.Add(new MailAddress(objemail.ccemailid));
                }    
                message.Subject = objemail.emailsubject1;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = objemail.emailbody+ msgbody;
                smtp.Port = 587;
                smtp.Host = "sampark.cms.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                dalticketrepo.updatemailhistory(objemail);

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
        public rcmdetail getcrnno(string customer, string pickupcode, string clientcode)
        {
            return dalticketrepo.getcrnno(customer, pickupcode, clientcode);
        }

        [HttpGet]
        public rcmdetail getrcmdata(string crnno, string clientcode)
        {
            return dalticketrepo.getrcmdata(crnno, clientcode);
        }

        [HttpGet]
        public IQueryable<FTWebApi.Models.locemail>  GetEmailHistory(string ticketno)
        {
            return dalticketrepo.GetEmailHistory(ticketno);
        }

    [HttpGet]
        public IQueryable<FTWebApi.Models.Ticket> GetAllTickets(string userid,string userrole)
        {
            return dalticketrepo.GetAllTickets(userid,userrole);
        }

        [HttpGet]
        public IQueryable<FTWebApi.Models.Ticket> GetAllTicketsfordate(string datefilter, string userid, string userrole)
        {
            return dalticketrepo.GetAllTicketsfordate(datefilter, userid, userrole);
        }

        

        [HttpGet]
        public FTWebApi.Models.Ticket GetTicket(string ID)
        {
            return dalticketrepo.GetTicket(ID);
        }

        [HttpGet]
        public IQueryable<FTWebApi.Models.ticketdetails>  GetTicketDetails(string ID)
        {
            return dalticketrepo.GetTicketDetails(ID);
        }

        [HttpPut]
        public HttpResponseMessage UpdateTicketAccept([FromBody] FTWebApi.Models.Ticket[] objticketlst)
        {
            try
            {

                dalticketrepo.UpdateTicketAccept(objticketlst);

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

        [HttpPut]
        public HttpResponseMessage UpdateTicket([FromBody] FTWebApi.Models.Ticket objticket)
        {
            string html;
            try
            {
                dalticketrepo.UpdateTicket(objticket);

                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                if (objticket.status=="Close")
                {
                    html=dalticketrepo.GenerateHtmlResponse(objticket);
                    sendemailclose(html, objticket.emailfrom);
                }

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

        private void sendemailclose(string html,string toemail)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("rcmuatquery@cms.com");
            message.To.Add(new MailAddress(toemail));
            message.Subject = "Ticket Close Response";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = html;
            smtp.Port = 587;
            smtp.Host = "sampark.cms.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }

        [HttpPost]
        public HttpResponseMessage InsertTicketDetails([FromBody] FTWebApi.Models.ticketdetails[] objticketdetails)
        {
            try
            {

                dalticketrepo.InsertTicketDetails(objticketdetails);

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
        public HttpResponseMessage InsertTicket([FromBody] FTWebApi.Models.Ticket objticket)
        {
            try
            {

                dalticketrepo.InsertTicket(objticket);

                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                /*
                string result;
                string fromemailid;

                if (objticket.acceptstatus == "Accept")
                {
                    result = dalticketrepo.Sendacceptemail(objticket.batchno);

                    if (result == "ok")
                    {
                        fromemailid = dalticketrepo.getfromemailid(objticket.batchno);
                        sendemailaccept(fromemailid);
                    }
                }
                */
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
        public HttpResponseMessage InsertBatch([FromBody] FTWebApi.Models.batch objbatch)
        {
            try
            {

                string batchno=dalticketrepo.InsertBatch(objbatch);

                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                return Request.CreateResponse(HttpStatusCode.OK, batchno);

                //return response;

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


        [HttpGet]
        public string GetBatchno()
        {
            return dalticketrepo.getlastbatchno();
        }

        [HttpPost]
        
        public HttpResponseMessage InsertTicketList([FromBody] FTWebApi.Models.Ticket[] objticketlst)
        {
            try
            {

                dalticketrepo.InsertTicketList(objticketlst);

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

        public HttpResponseMessage InsertTicketBulk([FromBody] FTWebApi.Models.Ticketbulk[] objticketlst)
        {
            try
            {

                dalticketrepo.InsertTicketBulk(objticketlst);

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