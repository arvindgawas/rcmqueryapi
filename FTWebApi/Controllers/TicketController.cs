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
using ExcelDataReader;
using System.Data;
using System.Net.Mime;
using System.Text.RegularExpressions;

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
                    FileName = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName) + ticketno + System.IO.Path.GetExtension(postedFile.FileName);
                    filePath = HttpContext.Current.Server.MapPath("~/uploadfiles/" + FileName);
                    //FileName = postedFile.FileName;
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

        [HttpPost]
        public HttpResponseMessage UploadFile1()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            string ticketno = httpRequest.Form["ticketno"];
            string filePath = "";
            string FileName = "";
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    FileName = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName) + ticketno + System.IO.Path.GetExtension(postedFile.FileName);
                    filePath = HttpContext.Current.Server.MapPath("~/uploadfiles/" + FileName);
                    //FileName = postedFile.FileName;
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            dalticketrepo.updateuploadfilepath1(ticketno, filePath, FileName);
           
            return result;
        }

        [HttpPost]
        public HttpResponseMessage UploadFile2()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            string ticketno = httpRequest.Form["ticketno"];
            string filePath = "";
            string FileName = "";
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    FileName = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName) + ticketno + System.IO.Path.GetExtension(postedFile.FileName);
                    filePath = HttpContext.Current.Server.MapPath("~/uploadfiles/" + FileName);
                    //FileName = postedFile.FileName;
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            dalticketrepo.updateuploadfilepath2(ticketno, filePath, FileName);

            return result;
        }


        [HttpPost]
        public HttpResponseMessage UploadExcelFile()
        {
            HttpResponseMessage result = null;
            
            var httpRequest = HttpContext.Current.Request;
            Stream FileStream = null;
            DataSet dsexcelRecords = new DataSet();
            IExcelDataReader reader = null;
            string message = "";
            List<FTWebApi.Models.Ticket> lstTicket = new List<FTWebApi.Models.Ticket>();

            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    FileStream = postedFile.InputStream;
                    if (postedFile != null)
                    {
                        if (postedFile.FileName.EndsWith(".xls"))
                        {
                            //reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                            reader = ExcelReaderFactory.CreateReader(FileStream);
                            dsexcelRecords = reader.AsDataSet();
                            reader.Close();
                        }
                        else if (postedFile.FileName.EndsWith(".xlsx"))
                        {
                            //reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                            reader = ExcelReaderFactory.CreateReader(FileStream);
                            
                            dsexcelRecords = reader.AsDataSet();
                            reader.Close();
                        }
                        else
                        {
                            message = "The file format is not supported.";
                        }

                       
                        if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                        {
                            DataTable dtStudentRecords = dsexcelRecords.Tables[0];
                            for (int i = 1; i < dtStudentRecords.Rows.Count; i++)
                            {
                                FTWebApi.Models.Ticket objticket = new FTWebApi.Models.Ticket();
                                objticket.crnno = Convert.ToString(dtStudentRecords.Rows[i][0]);
                                objticket.tickettype = Convert.ToString(dtStudentRecords.Rows[i][1]);
                                objticket.bank = Convert.ToString(dtStudentRecords.Rows[i][2]);
                                objticket.batchno = Convert.ToString(dtStudentRecords.Rows[i][3]);
                                if (dtStudentRecords.Rows[i][4]!=DBNull.Value)
                                {
                                    objticket.ticketdate = Convert.ToDateTime(dtStudentRecords.Rows[i][4]);
                                }
                                objticket.clientcode = Convert.ToString(dtStudentRecords.Rows[i][5]);
                                objticket.pickupcode = Convert.ToString(dtStudentRecords.Rows[i][6]);
                                //amount column
                                objticket.region = Convert.ToString(dtStudentRecords.Rows[i][7]);
                                //deposit slip column
                                objticket.location = Convert.ToString(dtStudentRecords.Rows[i][8]);
                                //solid column
                                objticket.hublocation = Convert.ToString(dtStudentRecords.Rows[i][9]);
                                objticket.emailfrom = Convert.ToString(dtStudentRecords.Rows[i][10]);
                                objticket.emailsubject = Convert.ToString(dtStudentRecords.Rows[i][11]);
                                objticket.emailcc = Convert.ToString(dtStudentRecords.Rows[i][12]);

                                lstTicket.Add(objticket);
                            }
                        }

                        dalticketrepo.AddExcelData(lstTicket);
                        result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                    }
                }    
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            FileStream.Close();
            return result;
        }


        [HttpPost]
        //public HttpResponseMessage UploadBulkTickeClose()
        public string UploadBulkTickeClose()
        {
            //HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            Stream FileStream = null;
            DataSet dsexcelRecords = new DataSet();
            IExcelDataReader reader = null;
            string message = "";
            List<FTWebApi.Models.Ticket> lstTicket = new List<FTWebApi.Models.Ticket>();
            List<FTWebApi.Models.ticketdetails> lsttd = new List<FTWebApi.Models.ticketdetails>();

            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    FileStream = postedFile.InputStream;

                    if (postedFile != null)
                    {
                        if (postedFile.FileName.EndsWith(".xls"))
                        {
                            //reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                            reader = ExcelReaderFactory.CreateReader(FileStream);
                            dsexcelRecords = reader.AsDataSet();
                            reader.Close();

                        }
                        else if (postedFile.FileName.EndsWith(".xlsx"))
                        {
                            //reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                            reader = ExcelReaderFactory.CreateReader(FileStream);
                            dsexcelRecords = reader.AsDataSet();
                            reader.Close();
                        }
                        else
                        {
                            message = "The file format is not supported.";
                        }

                        
                        if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                        {
                            DataTable dtStudentRecords = dsexcelRecords.Tables[0];
                            for (int i = 1; i < dtStudentRecords.Rows.Count; i++)
                            {
                                FTWebApi.Models.Ticket objticket = new FTWebApi.Models.Ticket();
                                FTWebApi.Models.ticketdetails objtd = new FTWebApi.Models.ticketdetails();

                                objticket.batchno = Convert.ToString(dtStudentRecords.Rows[i][0]);
                                objticket.ticketno = Convert.ToString(dtStudentRecords.Rows[i][1]);
                                objticket.crnno = Convert.ToString(dtStudentRecords.Rows[i][2]);
                                objticket.mistakedoneby = Convert.ToString(dtStudentRecords.Rows[i][3]);
                                objticket.errortype = Convert.ToString(dtStudentRecords.Rows[i][4]);
                                objticket.problem  = Convert.ToString(dtStudentRecords.Rows[i][8]);

                                objtd.ticketno = Convert.ToString(dtStudentRecords.Rows[i][1]);
                                objtd.crnno = Convert.ToString(dtStudentRecords.Rows[i][2]);
                                objtd.pickupdate = DateTime.Parse((dtStudentRecords.Rows[i][5].ToString()));
                                objtd.actualhcin = Convert.ToString(dtStudentRecords.Rows[i][6]);
                                objtd.actualamt = Decimal.Parse(dtStudentRecords.Rows[i][7].ToString());
                                
                                lstTicket.Add(objticket);
                                lsttd.Add(objtd);
                            }
                        }

                        message = dalticketrepo.AddExcelCloseData(lstTicket, lsttd);

                        if (message != "Close" && message != "Batch")
                        {
                            sendcloseemail(lstTicket);
                        }


                        //result = Request.CreateResponse(HttpStatusCode.Created, message);
                    }
                }
            }
            else
            {
                //result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            FileStream.Close();
            //throw new HttpResponseException(response);
            return message;
        }

        public void sendcloseemail(List<FTWebApi.Models.Ticket> lstTicket)
        {
            //arvind
            string sbatchid="";
            string html = "";
            FTWebApi.Models.Ticket objti;
            foreach (var objticket in lstTicket)
            {
                if (sbatchid!= objticket.batchno)
                {
                    html = dalticketrepo.GenerateHtmlResponse(objticket);

                    if (html != "notallclose")
                    {
                        objti = dalticketrepo.GetTicket(objticket.ticketno);

                        sendemailcloseexcel(html, objti.emailfrom, objti.emailsubject, objti.batchno, objti.emailcc, objti.filename);
                    }
                }
                sbatchid = objticket.batchno;
            }
        }

        public HttpResponseMessage Downloadnew(string filepath, string filname)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
          
          
            if (!File.Exists(@filepath))
            {
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
        public HttpResponseMessage getbulkclosedata()
        {
            string html = dalticketrepo.getbulkclosedata();
            byte[] bytes = Encoding.ASCII.GetBytes(html);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ticketclosure.xls"
            };

            return result;
        }

        [HttpGet]
        public HttpResponseMessage Downloadexelfile(string fromdate,string todate,string customer,string user, string customertype,
              string region,string location,string hublocation,string cdpncm,string issuetype,string sla)
        {
                string html = dalticketrepo.GetReportData(fromdate, todate, customer, user, customertype, region, location, hublocation, cdpncm, issuetype, sla);
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
        public List<FTWebApi.Models.ticketcount> getdashboardcount(string userid, string dtticketdate,string todate)
        {
            DateTime dt, dttodate;
             dt = DateTime.Parse(dtticketdate);
             dttodate = DateTime.Parse(todate);
            return dalticketrepo.getdashboardcount(userid, dt, dttodate);
        }

        [HttpGet]
        public List<FTWebApi.Models.ticketcount> getadmindashboardcount(string userid, string dtticketdate, string todate)
        {
            DateTime dt = DateTime.Parse(dtticketdate);
            DateTime dttodate = DateTime.Parse(todate);
            return dalticketrepo.getadmindashboardcount(userid, dt, dttodate);
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
            //message.From = new MailAddress("rcmuatquery@cms.com");
            message.From = new MailAddress("rcm.opsquery@cms.com ");
            message.To.Add(new MailAddress("arvind.gavas@cms.com"));
            message.Subject = "RCMQuery TAT Eascalations";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = html;
            smtp.Port = 587;
            smtp.Host = "sampark.cms.com"; //for gmail host  
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.Credentials = new NetworkCredential("rcm.opsquery@cms.com", "Rcm@opq2021");
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

        [HttpGet]
        public string getticketclosecount(string batchno)
        {
            string result="";
            result = dalticketrepo.getticketclosecount(batchno);
            return result;
        }

        [HttpGet]
        public string checkticketdata(string ticketno)
        {
            string result = "";
            result = dalticketrepo.checkticketdata(ticketno);
            return result;
        }

        private void sendemailaccept(string fromemailid)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            //message.From = new MailAddress("rcmuatquery@cms.com");
            message.From = new MailAddress("rcm.opsquery@cms.com");
            ///message.To.Add(new MailAddress(fromemailid));
            //message.To.Add(new MailAddress("arvind.gavas@cms.com"));
            message.Subject = "CMS Ticket Accptance Email";
            //message.IsBodyHtml = true; //to make message body as html  
            message.Body = "CMS Ticket Accptance Email";
            smtp.Port = 587;
            smtp.Host = "sampark.cms.com"; //for gmail host  
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.Credentials = new NetworkCredential("rcm.opsquery@cms.com", "Rcm@opq2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }



        [HttpPost]
        public HttpResponseMessage sendlocationemail([FromBody] FTWebApi.Models.locemail objemail)
        {

            //string sfilepath = objemail.filepath.Replace("Z:/", @"https://cmsliveuat.cms.com/rcmquery/api/ticket/DownloadFile?filepath=//172.19.100.157/nfsrmsuat/");
            string sfilepath = objemail.filepath.Replace("Z:/", @"http://rcmquerylive.cms.com/rcmquery/api/ticket/DownloadFile?filepath=//rcmticketfsstoacc.file.core.windows.net/rcmticketfileshare/");
            string sfilename = "&filneame=EmailBody" + objemail.ticketno + ".html";

            //String sbody = @"https://cmsliveuat.cms.com/rcmquery/api/ticket/DownloadFile?filepath=\\172.19.100.157\nfsrmsuat\RCMEmail_Files\2021_06_25\EmailBody_2021-06-25_104528_799404.html&filneame=EmailBodyT000264.html";
            String sbody = sfilepath+ sfilename;

            StringBuilder msgbody= new StringBuilder();
            msgbody.Append("<html><head></head><body>Please click below link to download Email Body" + "<br />" + "<a href=" + sbody + ">EmailBody</a></body></html>");


            if (objemail.emailattachment != "" && objemail.emailattachment != null)
            {
                //sfilepath = objemail.emailattachment.Replace("Z:/", @"https://cmsliveuat.cms.com/rcmquery/api/ticket/DownloadFile?filepath=//172.19.100.157/nfsrmsuat/");
                sfilepath = objemail.emailattachment.Replace("Z:/", @"http://rcmquerylive.cms.com/rcmquery/api/ticket/DownloadFile?filepath=//rcmticketfsstoacc.file.core.windows.net/rcmticketfileshare/");
                sfilename = "&filneame=Emailattachment" + objemail.ticketno + ".zip";
                sbody = sfilepath + sfilename;
                msgbody.Append("<html><head></head><body>Please click below link to download Email Attachemnt" + "<br />" + "<a href=" + sbody + ">Email Attachment</a></body></html>");
            }
            


            try
            {
                if (objemail.location)
                {

                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    //message.From = new MailAddress("rcmuatquery@cms.com");
                    message.From = new MailAddress("rcm.opsquery@cms.com");
                    message.To.Add(new MailAddress(objemail.toemailid));
                    if (objemail.ccemailid != null && objemail.ccemailid != "")
                    {
                        string[] CCId = objemail.ccemailid.Split(',');

                        foreach (string CCEmail in CCId)
                        {
                            message.CC.Add(new MailAddress(CCEmail));
                        }
                     
                    }

                    message.Subject = objemail.emailsubject1;
                    message.Body = objemail.emailbody + msgbody;
                    message.IsBodyHtml = true; //to make message body as html
                   
                    /*
                    if (objemail.emailattachment != "" && objemail.emailattachment!=null)
                    {
                        sfilepath = "";
                        sfilepath = objemail.emailattachment.Replace("Z:/", @"//172.19.100.157/nfsrmsuat/");

                        //sfilepath = objemail.emailattachment.Replace("Z:/", @"Z:/");

                        //string file = HttpContext.Current.Server.MapPath(sfilepath);
                        message.Attachments.Add(new Attachment(sfilepath));
                    }
                    */
                    smtp.Port = 587;
                    smtp.Host = "sampark.cms.com"; //for gmail host  
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = false;
                    //smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
                    smtp.Credentials = new NetworkCredential("rcm.opsquery@cms.com", "Rcm@opq2021");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);

                    dalticketrepo.updatemailhistory(objemail);
                }
                else
                {
                    dalticketrepo.UpdateTicketStatusClose(objemail.ticketno);
                    sendmanualcloseresponse(objemail);
                }
                //sendemailcloseexcel(html, objticket.emailfrom, objticket.emailsubject, objticket.batchno, objticket.emailcc, objticket.filename);

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

        public void sendmanualcloseresponse(FTWebApi.Models.locemail objemail)
        {
            string html;
            FTWebApi.Models.Ticket objticket;
            objticket= dalticketrepo.GetTicket(objemail.ticketno);

            if (objticket.status == "Close" )
            {
                html = dalticketrepo.GenerateHtmlResponse(objticket);

                if (html != "notallclose")
                {
                    sendemailcloseexcelmanual(html, objemail.toemailid, objemail.emailsubject1, objticket.batchno, objemail.ccemailid, objticket.filename, objemail.emailbody,objemail.filepath);
                }
            }
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
        public rcmdetail getrcmdatamanual(string crnno, string clientcode)
        {
            return dalticketrepo.getrcmdatamanual(crnno, clientcode);
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
        public IQueryable<FTWebApi.Models.Ticket> GetAllTicketsfordate(string datefilter,string todate, string userid, string userrole, string filter)
        {
            return dalticketrepo.GetAllTicketsfordate(datefilter,todate, userid, userrole,filter);
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
        public HttpResponseMessage DeleteTicketAll([FromBody] FTWebApi.Models.Ticket[] objticketlst)
        {
            try
            {

                dalticketrepo.DeleteTicketAll(objticketlst);

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

        [HttpGet]
        public HttpResponseMessage UpdateTicketStatus(string ticketno)
        {
            try
            {

                dalticketrepo.UpdateTicketStatus(ticketno);

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


        [HttpDelete]
        public HttpResponseMessage DeleteTicket(string ticketno)
        {
            try
            {
                dalticketrepo.DeleteTicket(ticketno);
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
        public HttpResponseMessage SendCloseResponse([FromBody] FTWebApi.Models.Ticket objticket)
        {
            string html;
            if (objticket.status == "Close" && objticket.SendAutoCloseResponse == "Y")
            {
                html = dalticketrepo.GenerateHtmlResponse(objticket);

                if (html != "notallclose")
                {
                    sendemailcloseexcel(html, objticket.emailfrom, objticket.emailsubject, objticket.batchno,objticket.emailcc,objticket.filename);
                }

            }

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            return response;
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

                if (objticket.status=="Close" && objticket.SendAutoCloseResponse=="Y")
                {
                    html=dalticketrepo.GenerateHtmlResponse(objticket);

                    if (html != "notallclose")
                    {
                        sendemailcloseexcel(html, objticket.emailfrom,objticket.emailsubject,objticket.batchno,objticket.emailcc,objticket.filename);
                    }
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
            //message.From = new MailAddress("rcmuatquery@cms.com");
            message.From = new MailAddress("rcm.opsquery@cms.com");
            message.To.Add(new MailAddress(toemail));
            message.Subject = "Ticket Close Response";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = html;
            smtp.Port = 587;
            smtp.Host = "sampark.cms.com"; //for gmail host  
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.Credentials = new NetworkCredential("rcm.opsquery@cms.com", "Rcm@opq2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }


        private void sendemailcloseexcelmanual(string html, string toemail, string emailsubject, string batchno, string emailcc, string filename,string emailbody,string filepath)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(html);
            var stream = new MemoryStream(bytes);

            string msgsubject = "";
            string msgbody = "";

            msgsubject =  emailsubject;

            //string sfilepath = @"F:\arvind\test\EmailBody_2021-06-25_104528_799404.html";
            //string sfilepath = filepath.Replace("Z:/", @"//172.19.100.157/nfsrmsuat/");
            
            string sfilepath = filepath.Replace("Z:/", @"//rcmticketfsstoacc.file.core.windows.net/rcmticketfileshare/");
            
            string htmlText = System.IO.File.ReadAllText(sfilepath);

            msgbody = emailbody;

            MailMessage message = new MailMessage();

            SmtpClient smtp = new SmtpClient();
            //message.From = new MailAddress("rcmuatquery@cms.com");
            
            message.From = new MailAddress("rcm.opsquery@cms.com");

            //message.To.Add(new MailAddress(toemail));
            if (toemail != "" && toemail != null)
            {
                string[] CCId = toemail.Split(',');

                foreach (string CCEmail in CCId)
                {
                    if (CCEmail != "")
                    {
                        message.To.Add(new MailAddress(CCEmail));
                    }
                }
            }

            if (emailcc != "" && emailcc != null)
            {
                string[] CCId = emailcc.Split(',');

                foreach (string CCEmail in CCId)
                {
                    message.CC.Add(new MailAddress(CCEmail));   
                }
            }
          
            message.Subject = msgsubject;
            message.IsBodyHtml = true; //to make message body as html  
            

            if (htmlText != null)
            {
                msgbody=msgbody.Replace("\n\n", "\n").Replace("\r\n", "\n").Replace("\n", "<br/><br/>");
                msgbody = msgbody + "<br><br><hr>" +htmlText;
            }

            message.Body = msgbody;
            
            message.Attachments.Add(new Attachment(stream, "ExcelResponse.xls", "application/x-msexcel"));
           
            List<FTWebApi.Models.Ticket> tiicketlist = dalticketrepo.getticketsforbatch(batchno);

           
            foreach (var objticket in tiicketlist)
            {
                if (objticket.filename != "" && objticket.filename != null)
                {
                    string file = HttpContext.Current.Server.MapPath("~/uploadfiles/" + objticket.filename);
                    message.Attachments.Add(new Attachment(file));
                }
                if (objticket.filename1 != "" && objticket.filename1 != null)
                {
                    string file = HttpContext.Current.Server.MapPath("~/uploadfiles/" + objticket.filename1);
                    message.Attachments.Add(new Attachment(file));
                }
                if (objticket.filename2 != "" && objticket.filename2 != null)
                {
                    string file = HttpContext.Current.Server.MapPath("~/uploadfiles/" + objticket.filename2);
                    message.Attachments.Add(new Attachment(file));
                }
            }
           

            smtp.Port = 587;
            smtp.Host = "sampark.cms.com"; //for gmail host  
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.Credentials = new NetworkCredential("rcm.opsquery@cms.com", "Rcm@opq2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
            stream.Close();
        }


        private void sendemailcloseexcel(string html, string toemail,string emailsubject,string batchno,string emailcc,string filename)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(html);
            var stream = new MemoryStream(bytes);
          
            string msgsubject = "";
            string msgbody = "";
          
            msgsubject = "Closed-"+batchno + " " + emailsubject;

            msgbody = "<table><tr><td>Dear Sir/Madam,</tr></td><tr><td></tr></td><tr><td>Your query has been closed vide batch no. " + batchno +
                       " and detailed report attached for your quick reference. </td></tr><tr><td></tr></td><tr><td>Please note the interaction number for future reference.</tr></td><tr><td></tr></td><tr><td>This is an auto acknowledgement mail. Kindly do not reply to this mail.</td></tr><tr><td></tr></td><tr><td>Regards,</td></tr><tr><td></tr></td><tr><td>CMS Customer Service Team</td></tr></table>";

            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            //message.From = new MailAddress("rcmuatquery@cms.com");
            message.From = new MailAddress("rcm.opsquery@cms.com");
            message.To.Add(new MailAddress(toemail));

            if (emailcc != "" && emailcc != null)
            {
                string[] CCId = emailcc.Split(',');

                foreach (string CCEmail in CCId)
                {
                    message.CC.Add(new MailAddress(CCEmail));
                }
            }

            message.Subject = msgsubject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = msgbody;
            message.Attachments.Add(new Attachment(stream, "ExcelResponse.xls", "application/x-msexcel"));

            List<FTWebApi.Models.Ticket> tiicketlist = dalticketrepo.getticketsforbatch(batchno);

            foreach (var objticket in tiicketlist)
            {
                if (objticket.filename != "" && objticket.filename != null)
                {
                    string file = HttpContext.Current.Server.MapPath("~/uploadfiles/" + objticket.filename);
                    message.Attachments.Add(new Attachment(file));
                }
                if (objticket.filename1 != "" && objticket.filename1 != null)
                {
                    string file = HttpContext.Current.Server.MapPath("~/uploadfiles/" + objticket.filename1);
                    message.Attachments.Add(new Attachment(file));
                }
                if (objticket.filename2 != "" && objticket.filename2 != null)
                {
                    string file = HttpContext.Current.Server.MapPath("~/uploadfiles/" + objticket.filename2);
                    message.Attachments.Add(new Attachment(file));
                }
            }

            smtp.Port = 587;
            smtp.Host = "sampark.cms.com"; //for gmail host  
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.Credentials = new NetworkCredential("rcm.opsquery@cms.com", "Rcm@opq2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
            stream.Close();
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