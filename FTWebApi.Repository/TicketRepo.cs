using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTWebApi.Models;
using FTWebApi.Interface;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.UI;
using System.Data.Entity.SqlServer;
using System.Data.Entity;

namespace FTWebApi.Repository
{
    public class TicketRepo : ITicketRepo
    {
        TMSEntitiesnew DataContext = new TMSEntitiesnew();
        rcmentities rcmDataContext = new rcmentities();

        public List<FTWebApi.Models.ticketcount> getadmindashboardcountnew(string userid, DateTime dtticketdate)
        {

            FTWebApi.Models.ticketcount objticketcount;
            Int32 acceptcount, duplicatecount, rejectcount, opencount, closecount, totalcount;

            List<FTWebApi.Models.ticketcount> objticketcountlst = new List<FTWebApi.Models.ticketcount>();

            var ticketgroup = (from t in DataContext.Tickets
                               where  t.assignedto != null
                               group t by new
                               {
                                   t.customer,
                                   t.assignedto
                               }
                               into customergroup
                               select new
                               {
                                   Bankkey = customergroup.Key.customer,
                                   Userkey = customergroup.Key.assignedto
                                   // grouplist = customergroup.ToList()
                               });


            foreach (var tgroup in ticketgroup)
            {
                objticketcount = new ticketcount();
                acceptcount = 0;
                duplicatecount = 0;
                rejectcount = 0;
                opencount = 0;
                closecount = 0;
                totalcount = 0;

                acceptcount = (from ts in DataContext.Tickets
                               where ts.assignedto == tgroup.Userkey && ts.acceptstatus == "Accept"
                               && ts.customer == tgroup.Bankkey && !ts.querystatus.Contains("Close")
                               select ts.TicketID).Count();
                duplicatecount = (from ts in DataContext.Tickets
                                  where  ts.assignedto == tgroup.Userkey && ts.acceptstatus == "Duplicate"
                                  && ts.customer == tgroup.Bankkey && !ts.querystatus.Contains("Close")
                                  select ts.TicketID).Count();
                rejectcount = (from ts in DataContext.Tickets
                               where  ts.assignedto == tgroup.Userkey && ts.acceptstatus == "Reject"
                               && ts.customer == tgroup.Bankkey && !ts.querystatus.Contains("Close")
                               select ts.TicketID).Count();
                opencount = (from ts in DataContext.Tickets
                             where  ts.assignedto == tgroup.Userkey && ts.querystatus == "Open"
                             && !ts.acceptstatus.Contains("Reject") && !ts.acceptstatus.Contains("Duplicate") && !ts.querystatus.Contains("Close")
                             && ts.customer == tgroup.Bankkey
                             select ts.TicketID).Count();
                closecount = (from ts in DataContext.Tickets
                              where  ts.assignedto == tgroup.Userkey && ts.querystatus == "Close"
                              && ts.customer == tgroup.Bankkey
                              select ts.TicketID).Count();
                totalcount = (from ts in DataContext.Tickets
                              where  ts.assignedto == tgroup.Userkey
                              && !ts.acceptstatus.Contains("Reject") && !ts.acceptstatus.Contains("Duplicate") && !ts.querystatus.Contains("Close")
                              && ts.customer == tgroup.Bankkey
                              select ts.TicketID).Count();

                objticketcount.bank = tgroup.Bankkey;
                objticketcount.userid = tgroup.Userkey;
                objticketcount.acceptcount = acceptcount;
                objticketcount.duplicatecount = duplicatecount;
                objticketcount.rejectcount = rejectcount;
                objticketcount.opencount = opencount;
                objticketcount.closecount = closecount;
                objticketcount.totalcount = totalcount;
                objticketcount.pendingcount = totalcount;

                objticketcountlst.Add(objticketcount);

            }

            return objticketcountlst;
         
        }

        public List<FTWebApi.Models.ticketcount> getadmindashboardcount(string userid, DateTime dtticketdate, DateTime todate)
        {

            FTWebApi.Models.ticketcount objticketcount;
            Int32 acceptcount, duplicatecount, rejectcount, opencount, closecount, totalcount;

            List<FTWebApi.Models.ticketcount> objticketcountlst = new List<FTWebApi.Models.ticketcount>();

            var ticketgroup = (from t in DataContext.Tickets
                               where t.ticketdate >= dtticketdate && t.ticketdate <= todate && t.assignedto != null
                               group t by new
                               {
                                   t.customer,
                                   t.assignedto
                               }
                               into customergroup
                               select new
                               {
                                   Bankkey = customergroup.Key.customer,
                                   Userkey = customergroup.Key.assignedto
                                   // grouplist = customergroup.ToList()
                               });


            foreach (var tgroup in ticketgroup)
            {
                objticketcount = new ticketcount();
                acceptcount = 0;
                duplicatecount = 0;
                rejectcount = 0;
                opencount = 0;
                closecount = 0;
                totalcount = 0;

                acceptcount = (from ts in DataContext.Tickets
                               where ts.ticketdate >= dtticketdate && ts.ticketdate <= todate && ts.assignedto == tgroup.Userkey && ts.acceptstatus == "Accept"
                               && ts.customer == tgroup.Bankkey
                               select ts.TicketID).Count();
                duplicatecount = (from ts in DataContext.Tickets
                                  where ts.ticketdate >= dtticketdate && ts.ticketdate <= todate && ts.assignedto == tgroup.Userkey && ts.acceptstatus == "Duplicate"
                                  && ts.customer == tgroup.Bankkey
                                  select ts.TicketID).Count();
                rejectcount = (from ts in DataContext.Tickets
                               where ts.ticketdate >= dtticketdate && ts.ticketdate <= todate && ts.assignedto == tgroup.Userkey && ts.acceptstatus == "Reject"
                               && ts.customer == tgroup.Bankkey
                               select ts.TicketID).Count();
                opencount = (from ts in DataContext.Tickets
                             where ts.ticketdate >= dtticketdate && ts.ticketdate <= todate && ts.assignedto == tgroup.Userkey && ts.querystatus == "Open"
                             && ts.customer == tgroup.Bankkey
                             select ts.TicketID).Count();
                closecount = (from ts in DataContext.Tickets
                              where ts.ticketdate >= dtticketdate && ts.ticketdate <= todate && ts.assignedto == tgroup.Userkey && ts.querystatus == "Close"
                              && ts.customer == tgroup.Bankkey
                              select ts.TicketID).Count();
                totalcount = (from ts in DataContext.Tickets
                              where ts.ticketdate >= dtticketdate && ts.ticketdate <= todate  && ts.assignedto == tgroup.Userkey
                              && ts.customer == tgroup.Bankkey
                              select ts.TicketID).Count();

                objticketcount.bank = tgroup.Bankkey;
                objticketcount.userid = tgroup.Userkey;
                objticketcount.acceptcount = acceptcount;
                objticketcount.duplicatecount = duplicatecount;
                objticketcount.rejectcount = rejectcount;
                objticketcount.opencount = opencount;
                objticketcount.closecount = closecount;
                objticketcount.totalcount = totalcount;
                objticketcount.pendingcount = totalcount - closecount;

                objticketcountlst.Add(objticketcount);

            }

            return objticketcountlst;


        }


        public List<FTWebApi.Models.ticketcount> getdashboardcount(string userid,DateTime dtticketdate, DateTime todate)
        {

            FTWebApi.Models.ticketcount objticketcount;
            Int32 acceptcount, duplicatecount, rejectcount, opencount, closecount, totalcount;

            List<FTWebApi.Models.ticketcount> objticketcountlst = new List<FTWebApi.Models.ticketcount>();

            var ticketgroup = (from t in DataContext.Tickets
                               where t.assignedto == userid
                               group t by t.customer into customergroup
                               select new
                               {
                                   Key = customergroup.Key
                                  // grouplist = customergroup.ToList()
                               });


            foreach (var tgroup in ticketgroup)
            {
                objticketcount = new ticketcount();
                acceptcount = 0;
                duplicatecount = 0;
                rejectcount = 0;
                opencount = 0;
                closecount = 0;
                totalcount = 0;

                acceptcount = (from ts in DataContext.Tickets
                                   where  ts.assignedto == userid && ts.acceptstatus == "Accept"
                                   && !ts.acceptstatus.Contains("Reject") && ts.acceptstatus != "Duplicate" && !ts.querystatus.Contains("Close")
                                   && ts.customer == tgroup.Key
                                   select ts.TicketID).Count();
                duplicatecount = (from ts in DataContext.Tickets
                                   where  ts.assignedto == userid && ts.acceptstatus == "Duplicate"
                                   && ts.customer == tgroup.Key
                                   select ts.TicketID).Count();
                rejectcount = (from ts in DataContext.Tickets
                                      where  ts.assignedto == userid && ts.acceptstatus == "Reject"
                                      && ts.customer == tgroup.Key
                                      select ts.TicketID).Count();
                opencount = (from ts in DataContext.Tickets
                                   where  ts.assignedto == userid && ts.querystatus == "Open"
                                   && !ts.acceptstatus.Contains("Reject") && ts.acceptstatus != "Duplicate" && !ts.querystatus.Contains("Close")
                                   && ts.customer == tgroup.Key
                                   select ts.TicketID).Count();
                closecount = (from ts in DataContext.Tickets
                                 where ts.assignedto == userid && ts.querystatus == "Close"
                                 && ts.customer == tgroup.Key
                                 select ts.TicketID).Count();
                 totalcount = (from ts in DataContext.Tickets
                                  where ts.assignedto == userid
                                  && !ts.acceptstatus.Contains("Reject") && ts.acceptstatus != "Duplicate" && !ts.querystatus.Contains("Close")
                                  && ts.customer == tgroup.Key
                                  select ts.TicketID).Count();

                objticketcount.bank = tgroup.Key;
                objticketcount.userid = userid;
                objticketcount.acceptcount = acceptcount;
                objticketcount.duplicatecount = duplicatecount;
                objticketcount.rejectcount = rejectcount;
                objticketcount.opencount = opencount;
                objticketcount.closecount = closecount;
                objticketcount.totalcount = totalcount;
                objticketcount.pendingcount = totalcount ;
                objticketcountlst.Add(objticketcount);

                }

            return objticketcountlst;

            
        }

        public List<FTWebApi.Models.ticketcount> getdashboardcountold(string userid, DateTime dtticketdate)
        {

            FTWebApi.Models.ticketcount objticketcount;
            Int32 acceptcount, duplicatecount, rejectcount, opencount, closecount, totalcount;

            List<FTWebApi.Models.ticketcount> objticketcountlst = new List<FTWebApi.Models.ticketcount>();

            var ticketgroup = (from t in DataContext.Tickets
                               where t.ticketdate == dtticketdate && t.assignedto == userid
                               group t by t.customer into customergroup
                               select new
                               {
                                   Key = customergroup.Key
                                   // grouplist = customergroup.ToList()
                               });


            foreach (var tgroup in ticketgroup)
            {
                objticketcount = new ticketcount();
                acceptcount = 0;
                duplicatecount = 0;
                rejectcount = 0;
                opencount = 0;
                closecount = 0;
                totalcount = 0;

                acceptcount = (from ts in DataContext.Tickets
                               where ts.ticketdate == dtticketdate && ts.assignedto == userid && ts.acceptstatus == "Accept"
                               && ts.customer == tgroup.Key
                               select ts.TicketID).Count();
                duplicatecount = (from ts in DataContext.Tickets
                                  where ts.ticketdate == dtticketdate && ts.assignedto == userid && ts.acceptstatus == "Duplicate"
                                  && ts.customer == tgroup.Key
                                  select ts.TicketID).Count();
                rejectcount = (from ts in DataContext.Tickets
                               where ts.ticketdate == dtticketdate && ts.assignedto == userid && ts.acceptstatus == "Reject"
                               && ts.customer == tgroup.Key
                               select ts.TicketID).Count();
                opencount = (from ts in DataContext.Tickets
                             where ts.ticketdate == dtticketdate && ts.assignedto == userid && ts.querystatus == "Open"
                             && ts.customer == tgroup.Key
                             select ts.TicketID).Count();
                closecount = (from ts in DataContext.Tickets
                              where ts.ticketdate == dtticketdate && ts.assignedto == userid && ts.querystatus == "Close"
                              && ts.customer == tgroup.Key
                              select ts.TicketID).Count();
                totalcount = (from ts in DataContext.Tickets
                              where ts.ticketdate == dtticketdate && ts.assignedto == userid
                              && ts.customer == tgroup.Key
                              select ts.TicketID).Count();

                objticketcount.bank = tgroup.Key;
                objticketcount.userid = userid;
                objticketcount.acceptcount = acceptcount;
                objticketcount.duplicatecount = duplicatecount;
                objticketcount.rejectcount = rejectcount;
                objticketcount.opencount = opencount;
                objticketcount.closecount = closecount;
                objticketcount.totalcount = totalcount;
                objticketcount.pendingcount = totalcount - closecount;
                objticketcountlst.Add(objticketcount);

            }

            return objticketcountlst;


        }
        public FTWebApi.Models.ddlisttracker getddlisttracker()
        {
            ddlisttracker ddl = new ddlisttracker();

            var customerlst = (from e in DataContext.BankMasters
                                 select new customer
                                 {
                                     ID = e.ID,
                                     Name = e.Bank
                                 }).AsQueryable();

            var userlist = (from a in DataContext.usermasters
                            select new Userddl
                            {
                                ID = a.Userid,
                                UserName = a.UserName
                            }).AsQueryable();

            //where a.UserPriority == "1"

            ddl.customerlst = customerlst.AsQueryable();
            ddl.Userlst = userlist.AsQueryable();

            return ddl;
        }

        public FTWebApi.Models.ddlist getddlist(string querytype)
        {
            ddlist ddl = new ddlist();
            var errortypeslst = (from e in DataContext.ErrorTypes
                                 where e.querytype == querytype
                                 select new errortype
                                 {
                                     ID = e.ID,
                                     ErrorName = e.ErrorType1
                                 }).AsQueryable();

            var userlist = (from a in DataContext.usermasters
                            select new Userdd
                            {
                                ID = a.Userid,
                                UserName = a.UserName
                            }).AsQueryable();

            //where a.UserPriority == "1"

            ddl.errortypelst = errortypeslst.AsQueryable();
            ddl.Userddlist = userlist.AsQueryable();

            return ddl;
        }

        public FTWebApi.Models.ticketuser ValidateUser(string ID)
        {
            var lstsm = (from qm in DataContext.usermasters
                         where qm.Userid == ID
                         select new FTWebApi.Models.ticketuser
                         {
                             UserId = qm.Userid,
                             Password = qm.UserPassword,
                             userrole = qm.UserRole
                         }).FirstOrDefault();

            return lstsm;

        }



        public string getfromemailid(string batchno)
        {
            string fromemailid = (from a in DataContext.Batches
                                  where a.BatchID == batchno
                                  select a.FromEmail).FirstOrDefault();

            return fromemailid;

        }

        public string Sendacceptemail(string batchno)
        {
            var ticketcount = (from a in DataContext.Tickets
                               where a.BatchID == batchno
                               select a.TicketID).Count();

            var ticketacceptcount = (from a in DataContext.Tickets
                                     where a.BatchID == batchno & a.acceptstatus == "Accept"
                                     select a.TicketID).Count();

            if (ticketcount == ticketacceptcount)
            {
                return "ok";
            }

            return "no";
        }

        public IQueryable<FTWebApi.Models.locemail> GetEmailHistory(string ticketno)
        {
            var lstemail = (from a in DataContext.LocationEmailHistories
                            where a.Ticketno == ticketno
                            select new FTWebApi.Models.locemail
                            {
                                ticketno = a.Ticketno,
                                toemailid = a.toemailid,
                                emailsubject1 = a.emailsubject,
                                emailbody = a.emailbody,
                                ccemailid = a.ccemailid,
                                sentdate = a.emailsentdate
                            });

            return lstemail;
        }

        public void updatemailhistory(FTWebApi.Models.locemail objemail)
        {
            try
            {
                FTWebApi.Repository.LocationEmailHistory emailhist = new LocationEmailHistory();

                emailhist.Ticketno = objemail.ticketno;
                emailhist.toemailid = objemail.toemailid;
                emailhist.emailbody = objemail.emailbody;
                emailhist.emailsubject = objemail.emailsubject1;
                emailhist.emailsentdate = DateTime.Today;

                DataContext.LocationEmailHistories.Add(emailhist);
                DataContext.SaveChanges();
            }
            catch (Exception ec)
            {
                throw ec;
            }
        }

        public FTWebApi.Models.rcmdetail getcrnno(string customer, string pickupcode, string clientcode)
        {
            //join area in rcmDataContext.LocationMasters on a.AreaCode equals area.LocationCode
            //&area.regioncode == a.Regioncode & area.LocationCode == a.LocationCode & area.hublocationcode == a.HubLocationCode

                    var crnno = (from a in rcmDataContext.ClientCustMasters
                         join b in rcmDataContext.ClientCustAccountMasters on a.ClientCustCode equals b.ClientCustCode
                         join c in rcmDataContext.CustomerMasters on a.CustomerCode equals c.CustomerCode
                         join r in rcmDataContext.RegionMasts on a.Regioncode equals r.regioncode
                         join l in rcmDataContext.LocationMasts on a.LocationCode equals l.locationcode
                         join area in rcmDataContext.LocationMasters on a.AreaCode equals area.LocationCode
                         join h in rcmDataContext.HublocationMasts on a.HubLocationCode equals h.hublocationcode
                         join j in rcmDataContext.ClientCustBankDetails on a.ClientCustCode equals j.ClientCustCode
                         where b.PickupCode == pickupcode & b.ClientCode == clientcode & c.CustomerName.StartsWith(customer.Substring(0,3)) & b.AccountId == clientcode
                         & l.regioncode == r.regioncode & h.locationcode == l.locationcode
                         select new rcmdetail { crn = a.ClientCustCode, clientname = a.ClientCustName, region = r.regionname,
                             location = l.locationname, hublocation = h.hublocationname, area = area.LocationName
                             , customertype = a.AccountType, cdpncm = j.DepositionType, hierarchycode = b.HierarchyCode,Company = a.CompanyCode
                         }).SingleOrDefault();

                return crnno;

        }

        public FTWebApi.Models.rcmdetail getrcmdata(string crnnos, string clientcode)
        {
            //join area in rcmDataContext.LocationMasters on a.AreaCode equals area.LocationCode
            //&area.regioncode == a.Regioncode & area.LocationCode == a.LocationCode & area.hublocationcode == a.HubLocationCode

            var crnno = (from a in rcmDataContext.ClientCustMasters
                         join b in rcmDataContext.ClientCustAccountMasters on a.ClientCustCode equals b.ClientCustCode
                         join c in rcmDataContext.CustomerMasters on a.CustomerCode equals c.CustomerCode
                         join r in rcmDataContext.RegionMasts on a.Regioncode equals r.regioncode
                         join l in rcmDataContext.LocationMasts on a.LocationCode equals l.locationcode
                         join area in rcmDataContext.LocationMasters on a.AreaCode equals area.LocationCode
                         join h in rcmDataContext.HublocationMasts on a.HubLocationCode equals h.hublocationcode
                         join j in rcmDataContext.ClientCustBankDetails on a.ClientCustCode equals j.ClientCustCode
                         where b.ClientCode == clientcode & a.ClientCustCode == crnnos & b.AccountId == clientcode
                         & l.regioncode == r.regioncode & h.locationcode == l.locationcode
                         select new rcmdetail
                         {
                             pickupcode = b.PickupCode,
                             clientname = a.ClientCustName,
                             region = r.regionname,
                             location = l.locationname,
                             hublocation = h.hublocationname,
                             area = area.LocationName,
                             customertype = a.AccountType,
                             cdpncm = j.DepositionType,
                             hierarchycode = b.HierarchyCode,
                             Company = a.CompanyCode
                         }).SingleOrDefault();

            return crnno;

        }
        public void AddExcelCloseData(List<FTWebApi.Models.Ticket> lstTicket, List<FTWebApi.Models.ticketdetails> lsttd)
        {
            foreach (var objticket in lstTicket)
            {
                
                Ticket tc = (from c in DataContext.Tickets
                             where c.TicketID == objticket.ticketno
                             select c).FirstOrDefault();

                var errorid = (from e in DataContext.ErrorTypes
                               where e.querytype == tc.tikcettype && e.ErrorType1 == objticket.errortype
                               select e.ID).SingleOrDefault();

                tc.mistakedoneby = objticket.mistakedoneby;
                tc.errortype = errorid.ToString();
                tc.problem = objticket.problem;
                tc.querystatus = "Close";
                tc.resolveddate = DateTime.Now;
                tc.ModifiedDate = DateTime.Now;

                TicketDetail td = (from c in DataContext.TicketDetails
                                    where c.TicketID == objticket.ticketno
                                    select c).FirstOrDefault();

                FTWebApi.Models.ticketdetails tdexcel = lsttd.Where(a => a.ticketno == objticket.ticketno).FirstOrDefault();

                td.TicketID = objticket.ticketno;
                td.pickupcode = tc.pickupcode;
                td.clientcode = tc.clientcode;
                td.crnno = tc.crnno;
                td.pickupdate = tdexcel.pickupdate;
                td.actualhcin = tdexcel.actualhcin;
                td.actualamt = tdexcel.actualamt;
                td.ModifiedDate = DateTime.Now;

                DataContext.SaveChanges();

            }

        }

        public void AddExcelData(List<FTWebApi.Models.Ticket> lstticket)
        {
            string ticketno = "";
            string assigneduser = "";
            string sbatchno = "";
            string ssubject = "";
            string semailfrom = "";
            sbatchno = getbatchno();


            foreach (var objticket in lstticket)
            {
                ssubject = objticket.emailsubject;
                semailfrom = objticket.emailfrom;

                var newticket = (from a in rcmDataContext.ClientCustMasters
                                  join b in rcmDataContext.ClientCustAccountMasters on a.ClientCustCode equals b.ClientCustCode
                                  join c in rcmDataContext.CustomerMasters on a.CustomerCode equals c.CustomerCode
                                  join r in rcmDataContext.RegionMasts on a.Regioncode equals r.regioncode
                                  join l in rcmDataContext.LocationMasts on a.LocationCode equals l.locationcode
                                  join area in rcmDataContext.LocationMasters on a.AreaCode equals area.LocationCode
                                  join h in rcmDataContext.HublocationMasts on a.HubLocationCode equals h.hublocationcode
                                  join j in rcmDataContext.ClientCustBankDetails on a.ClientCustCode equals j.ClientCustCode
                                  where a.ClientCustCode == objticket.crnno
                                  & l.regioncode == r.regioncode & h.locationcode == l.locationcode
                                  select new
                                  {
                                      crnno = objticket.crnno,
                                      tickettype = objticket.tickettype,
                                      pickupcode = b.PickupCode,
                                      clientcode = b.ClientCode,
                                      client = a.ClientCustName,
                                      region = r.regionname,
                                      location = l.locationname,
                                      hublocation = h.hublocationname,
                                      area = area.LocationName,
                                      customertype = a.AccountType,
                                      cdpncm = j.DepositionType,
                                      hierarchycode = b.HierarchyCode

                                  }).FirstOrDefault();

                    ticketno = genticketno();
                    assigneduser = "";

                    Ticket tc = new Ticket();

                    tc.TicketID = ticketno;
                    tc.querystatus = "Open";
                    if (objticket.bank != null && objticket.bank != "")
                    {
                        if (objticket.tickettype != "" && objticket.tickettype != null)
                        {
                            assigneduser = (from c in DataContext.usermasters
                                            join m in DataContext.userbankmaps on c.Userid equals m.Userid
                                            where m.Bank.Contains(objticket.bank) & m.QueryType == objticket.tickettype & m.UserPriority == "1"
                                            select c.Userid).SingleOrDefault();
                        }
                        else
                        {
                            assigneduser = (from c in DataContext.usermasters
                                            join m in DataContext.userbankmaps on c.Userid equals m.Userid
                                            where m.Bank.Contains(objticket.bank) & m.UserPriority == "1"
                                            select c.Userid).FirstOrDefault();
                        }
                        tc.assignedto = assigneduser;
                        tc.querystatus = "Assigned";
                    }

                    tc.ticketdate = DateTime.Today;
                    tc.BatchID = sbatchno;
                    tc.tikcettime = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
                    tc.tikcettype = objticket.tickettype;
                    tc.customer = objticket.bank;
                    tc.acceptstatus = "MAccept";
                    tc.pickupcode = newticket.pickupcode;
                    tc.clientcode = newticket.clientcode;
                    tc.client = newticket.client;
                    tc.crnno = newticket.crnno;
                    tc.area = newticket.area;
                    tc.cdpncm = newticket.cdpncm;
                    tc.region = newticket.region;
                    tc.location = newticket.location;
                    tc.hublocation = newticket.hublocation;
                    tc.customertype = newticket.customertype;
                    tc.hierarchycode = newticket.hierarchycode;
                    tc.CreatedBy = "";
                    tc.CreatedDate = DateTime.Now;

                    DataContext.Tickets.Add(tc);

                
            }

            Batch bt = new Batch();
            bt.BatchID = sbatchno;
            bt.Date = DateTime.Today;
            bt.EmailBody = "upload";
            bt.EmailSubject = ssubject;
            bt.FromEmail = semailfrom;
            bt.CreatedDate = DateTime.Now;
            DataContext.Batches.Add(bt);

            DataContext.SaveChanges();

        }


        public IQueryable<FTWebApi.Models.Ticket> GetAllTickets(string userid, string userrole)
        {
            
            IQueryable<FTWebApi.Models.Ticket> lstTicket;

            try
            {
                if (userrole == "admin")
                {
                    lstTicket = (from a in DataContext.Tickets
                                 join b in DataContext.Batches on a.BatchID equals b.BatchID into ps
                                 from b in ps.DefaultIfEmpty()
                                 where a.ticketdate == DateTime.Today
                                 select new FTWebApi.Models.Ticket
                                 {
                                     ticketno = a.TicketID,
                                     ticketdate = a.ticketdate,
                                     tickettype = a.tikcettype,
                                     tickettime = a.tikcettime,
                                     assignedto = a.assignedto,
                                     batchno = a.BatchID,
                                     oldbatchno = b.oldbatchid,
                                     acceptstatus = a.acceptstatus,
                                     emailsubject = b.EmailSubject,
                                     emailfrom = b.FromEmail,
                                     //resolveddate = (DateTime)a.resolveddate,
                                     bank = a.customer,
                                     pickupcode = a.pickupcode,
                                     clientcode = a.clientcode,
                                     client = a.client,
                                     crnno = a.crnno,
                                     customertype = a.customertype,
                                     hierarchycode = a.hierarchycode,
                                     area = a.area,
                                     cdpncm = a.cdpncm,
                                     region = a.region,
                                     location = a.location,
                                     hublocation = a.hublocation,
                                     problem = a.problem,
                                     mistakedoneby = a.mistakedoneby,
                                     errortype = a.errortype,
                                     status = a.querystatus,
                                     createduser = a.CreatedBy,
                                     //createddate = (DateTime)a.CreatedDate,
                                     modifieduser = a.ModifiedBy,
                                     //modifieddate = (DateTime)a.ModifiedDate
                                 }).AsQueryable();
                }
                else
                {
                    lstTicket = (from a in DataContext.Tickets
                                 join b in DataContext.Batches on a.BatchID equals b.BatchID into ps
                                 from b in ps.DefaultIfEmpty()
                                 where a.ticketdate == DateTime.Today && a.assignedto == userid
                                 && !a.acceptstatus.Contains("Reject") && !a.acceptstatus.Contains("Duplicate") && !a.querystatus.Contains("Close")
                                 select new FTWebApi.Models.Ticket
                                 {
                                     ticketno = a.TicketID,
                                     ticketdate = a.ticketdate,
                                     tickettype = a.tikcettype,
                                     tickettime = a.tikcettime,
                                     assignedto = a.assignedto,
                                     batchno = a.BatchID,
                                     acceptstatus = a.acceptstatus,
                                     emailsubject = b.EmailSubject,
                                     emailfrom = b.FromEmail,
                                     //resolveddate = (DateTime)a.resolveddate,
                                     bank = a.customer,
                                     pickupcode = a.pickupcode,
                                     clientcode = a.clientcode,
                                     client = a.client,
                                     crnno = a.crnno,
                                     customertype = a.customertype,
                                     hierarchycode = a.hierarchycode,
                                     area = a.area,
                                     cdpncm = a.cdpncm,
                                     region = a.region,
                                     location = a.location,
                                     hublocation = a.hublocation,
                                     problem = a.problem,
                                     mistakedoneby = a.mistakedoneby,
                                     errortype = a.errortype,
                                     status = a.querystatus,
                                     createduser = a.CreatedBy,
                                     //createddate = (DateTime)a.CreatedDate,
                                     modifieduser = a.ModifiedBy,
                                     //modifieddate = (DateTime)a.ModifiedDate
                                 }).AsQueryable();
                }
            }
            catch (Exception ec)
            {
                throw ec;
            }

            return lstTicket;
        }

        public IQueryable<FTWebApi.Models.Ticket> GetAllTicketsfordate(string datefilter,string todate, string userid, string userrole,string filter)
        {
            //join b in DataContext.Batches on a.BatchID equals b.BatchID
            //emailsubject = b.EmailSubject,s
            //emailfrom =b.FromEmail,
            DateTime dt,dttodate;
            string sfilter = "";

            
            IQueryable<FTWebApi.Models.Ticket> lstTicket;

            sfilter = (filter == "undefined") ? "" : filter;

            try
            {
                
                if (userrole == "admin")
                {
                   
                    if (filter=="close" || filter == "reject" || filter == "duplicate")
                    {
                        sfilter = filter;
                    }    
                    else
                    {
                        sfilter = "";
                    }

                    if (datefilter == null)
                    {
                        //dt = DateTime.Today;
                        lstTicket = (from a in DataContext.Tickets
                                     join b in DataContext.Batches on a.BatchID equals b.BatchID into ps
                                     from b in ps.DefaultIfEmpty()
                                     //where a.ticketdate == dt
                                     where  !a.acceptstatus.Contains("Reject") && a.acceptstatus != "Duplicate" && !a.querystatus.Contains("Close")
                                     orderby a.TicketID descending
                                     select new FTWebApi.Models.Ticket
                                     {
                                         ticketno = a.TicketID,
                                         ticketdate = a.ticketdate,
                                         tickettype = a.tikcettype,
                                         tickettime = a.tikcettime,
                                         assignedto = a.assignedto,
                                         batchno = a.BatchID,
                                         acceptstatus = a.acceptstatus,
                                         emailsubject = b.EmailSubject,
                                         emailfrom = b.FromEmail,
                                         //resolveddate = (DateTime)a.resolveddate,
                                         bank = a.customer,
                                         pickupcode = a.pickupcode,
                                         clientcode = a.clientcode,
                                         client = a.client,
                                         crnno = a.crnno,
                                         customertype = a.customertype,
                                         hierarchycode = a.hierarchycode,
                                         area = a.area,
                                         cdpncm = a.cdpncm,
                                         region = a.region,
                                         location = a.location,
                                         hublocation = a.hublocation,
                                         problem = a.problem,
                                         mistakedoneby = a.mistakedoneby,
                                         errortype = a.errortype,
                                         status = a.querystatus,
                                         rejectremark = a.rejectremark,
                                         createduser = a.CreatedBy,
                                         //createddate = (DateTime)a.CreatedDate,
                                         modifieduser = a.ModifiedBy,
                                         //modifieddate = (DateTime)a.ModifiedDate
                                     }).AsQueryable();

                        Int32 ncount = lstTicket.Count();

                    }
                    else
                    {
                        dt = DateTime.Parse(datefilter);
                        dttodate = DateTime.Parse(todate);
                        if (sfilter == "")
                        {
                            lstTicket = (from a in DataContext.Tickets
                                         join b in DataContext.Batches on a.BatchID equals b.BatchID into ps
                                         from b in ps.DefaultIfEmpty()
                                         where a.ticketdate >= dt && a.ticketdate <= dttodate
                                         //where  !a.acceptstatus.Contains("Reject") && a.acceptstatus != "Duplicate" && !a.querystatus.Contains("Close")
                                         orderby a.TicketID descending
                                         select new FTWebApi.Models.Ticket
                                         {
                                             ticketno = a.TicketID,
                                             ticketdate = a.ticketdate,
                                             tickettype = a.tikcettype,
                                             tickettime = a.tikcettime,
                                             assignedto = a.assignedto,
                                             batchno = a.BatchID,
                                             acceptstatus = a.acceptstatus,
                                             emailsubject = b.EmailSubject,
                                             emailfrom = b.FromEmail,
                                             //resolveddate = (DateTime)a.resolveddate,
                                             bank = a.customer,
                                             pickupcode = a.pickupcode,
                                             clientcode = a.clientcode,
                                             client = a.client,
                                             crnno = a.crnno,
                                             customertype = a.customertype,
                                             hierarchycode = a.hierarchycode,
                                             area = a.area,
                                             cdpncm = a.cdpncm,
                                             region = a.region,
                                             location = a.location,
                                             hublocation = a.hublocation,
                                             problem = a.problem,
                                             mistakedoneby = a.mistakedoneby,
                                             errortype = a.errortype,
                                             status = a.querystatus,
                                             rejectremark = a.rejectremark,
                                             createduser = a.CreatedBy,
                                             //createddate = (DateTime)a.CreatedDate,
                                             modifieduser = a.ModifiedBy,
                                             //modifieddate = (DateTime)a.ModifiedDate
                                         }).AsQueryable();
                        }
                        else
                        {
                            lstTicket = (from a in DataContext.Tickets
                                         join b in DataContext.Batches on a.BatchID equals b.BatchID into ps
                                         from b in ps.DefaultIfEmpty()
                                         where a.ticketdate >= dt && a.ticketdate <= dttodate
                                         && (a.acceptstatus.Contains(sfilter) || a.querystatus.Contains(sfilter))
                                         orderby a.TicketID descending
                                         select new FTWebApi.Models.Ticket
                                         {
                                             ticketno = a.TicketID,
                                             ticketdate = a.ticketdate,
                                             tickettype = a.tikcettype,
                                             tickettime = a.tikcettime,
                                             assignedto = a.assignedto,
                                             batchno = a.BatchID,
                                             acceptstatus = a.acceptstatus,
                                             emailsubject = b.EmailSubject,
                                             emailfrom = b.FromEmail,
                                             //resolveddate = (DateTime)a.resolveddate,
                                             bank = a.customer,
                                             pickupcode = a.pickupcode,
                                             clientcode = a.clientcode,
                                             client = a.client,
                                             crnno = a.crnno,
                                             customertype = a.customertype,
                                             hierarchycode = a.hierarchycode,
                                             area = a.area,
                                             cdpncm = a.cdpncm,
                                             region = a.region,
                                             location = a.location,
                                             hublocation = a.hublocation,
                                             problem = a.problem,
                                             mistakedoneby = a.mistakedoneby,
                                             errortype = a.errortype,
                                             status = a.querystatus,
                                             rejectremark = a.rejectremark,
                                             createduser = a.CreatedBy,
                                             //createddate = (DateTime)a.CreatedDate,
                                             modifieduser = a.ModifiedBy,
                                             //modifieddate = (DateTime)a.ModifiedDate
                                         }).AsQueryable();
                        }
                    }
                }
                else
                {
                    lstTicket = (from a in DataContext.Tickets
                                 join b in DataContext.Batches on a.BatchID equals b.BatchID into ps
                                 from b in ps.DefaultIfEmpty()
                                 where a.assignedto == userid
                                 //where a.ticketdate == dt && a.assignedto == userid
                                 && !a.acceptstatus.Contains("Reject") && a.acceptstatus!="Duplicate" && !a.querystatus.Contains("Close")
                                 orderby a.TicketID descending
                                 select new FTWebApi.Models.Ticket
                                 {
                                     ticketno = a.TicketID,
                                     ticketdate = a.ticketdate,
                                     tickettype = a.tikcettype,
                                     tickettime = a.tikcettime,
                                     assignedto = a.assignedto,
                                     batchno = a.BatchID,
                                     acceptstatus = a.acceptstatus,
                                     emailsubject = b.EmailSubject,
                                     emailfrom = b.FromEmail,
                                     //resolveddate = (DateTime)a.resolveddate,
                                     bank = a.customer,
                                     pickupcode = a.pickupcode,
                                     clientcode = a.clientcode,
                                     customertype = a.customertype,
                                     hierarchycode = a.hierarchycode,
                                     client = a.client,
                                     crnno = a.crnno,
                                     area = a.area,
                                     cdpncm = a.cdpncm,
                                     region = a.region,
                                     location = a.location,
                                     hublocation = a.hublocation,
                                     problem = a.problem,
                                     mistakedoneby = a.mistakedoneby,
                                     errortype = a.errortype,
                                     status = a.querystatus,
                                     rejectremark = a.rejectremark,
                                     createduser = a.CreatedBy,
                                     //createddate = (DateTime)a.CreatedDate,
                                     modifieduser = a.ModifiedBy,
                                     //modifieddate = (DateTime)a.ModifiedDate
                                 }).AsQueryable();
                }
            }
            catch (Exception ec)
            {
                throw ec;
            }

            return lstTicket;
        }


        public IQueryable<FTWebApi.Models.ticketdetails> GetTicketDetails(string ID)
        {
            var lstTicket = (from a in DataContext.TicketDetails
                             join b in DataContext.Tickets on a.TicketID equals b.TicketID
                             where a.TicketID == ID
                             select new FTWebApi.Models.ticketdetails
                             {
                                 id = a.ID,
                                 ticketno = a.TicketID,
                                 pickupdate = a.pickupdate,
                                 pickupcode = b.pickupcode,
                                 clientcode = b.clientcode,
                                 crnno = b.crnno,
                                 actualhcin = a.actualhcin,
                                 wronghcin = a.wronghcin,
                                 actualdispis = a.actualdispis,
                                 wrongdispis = a.wrongdispis,
                                 actualamt = a.actualamt,
                                 wrongamt = a.wrongamt,
                                 customeruniquecode = a.customeruniquecode,
                                 wrongpickupcode = a.wrongpickupcode,
                                 wrongclientcode = a.wrongclientcode,
                                 wrongcustomeruniquecode = a.wrongcustomeruniquecode,
                                 depositiondate = a.depostiondate,
                                 soleid = a.soleid,
                                 bankbranchlocation = a.bankbranchlocation
                             }).AsQueryable();

            return lstTicket;
        }

        public FTWebApi.Models.Ticket GetTicket(string ID)
        {
            //join b in DataContext.Batches on a.BatchID equals b.BatchID
            //emailsubject = b.EmailSubject,emailfrom = b.FromEmail,

            //TimeSpan.Parse(DateTime.Now.ToLongTimeString());

            var lstTicket = (from a in DataContext.Tickets
                             join b in DataContext.Batches on a.BatchID equals b.BatchID into ps
                             from b in ps.DefaultIfEmpty()
                             where a.TicketID == ID
                             select new FTWebApi.Models.Ticket
                             {
                                 ticketno = a.TicketID,
                                 ticketdate = a.ticketdate,
                                 tickettype = a.tikcettype,
                                 tickettime = a.tikcettime,
                                 assignedto = a.assignedto,
                                 batchno = a.BatchID,
                                 emailsubject = b.EmailSubject,
                                 emailfrom = b.FromEmail,
                                 emailbody = b.EmailBody,
                                 emailcc = b.Emailcc,
                                 acceptstatus = a.acceptstatus,
                                 bank = a.customer,
                                 pickupcode = a.pickupcode,
                                 clientcode = a.clientcode,
                                 client = a.client,
                                 crnno = a.crnno,
                                 customertype = a.customertype,
                                 hierarchycode = a.hierarchycode,
                                 area = a.area,
                                 cdpncm = a.cdpncm,
                                 region = a.region,
                                 location = a.location,
                                 hublocation = a.hublocation,
                                 problem = a.problem,
                                 mistakedoneby = a.mistakedoneby,
                                 errortype = a.errortype,
                                 status = a.querystatus,
                                 filepath = a.filepath,
                                 filename = a.filename,
                                 createduser = a.CreatedBy,
                                 company = a.Company,
                                 //createddate = (DateTime)a.CreatedDate,
                                 modifieduser = a.ModifiedBy,
                                 modifieddate = DateTime.Today
                             }).SingleOrDefault();

            lstTicket.tickettime = TimeSpan.Parse(lstTicket.tickettime.ToString(@"hh\:mm"));

            return lstTicket;
        }

        public void updateuploadfilepath(string ticketno, string filePath,string FileName)
        {

             Ticket tc = (from c in DataContext.Tickets
                          where c.TicketID == ticketno
                          select c).FirstOrDefault();

             tc.filepath = filePath;
             tc.filename = FileName;

             DataContext.SaveChanges();

        }
        public void UpdateTicketAccept(FTWebApi.Models.Ticket[] objlst)
        {
            string batchno = "";
            try
            {
                /*
                foreach (FTWebApi.Models.Ticket tmptilist in objlst)
                {
                    Ticket tc = (from c in DataContext.Tickets
                                 where c.TicketID == tmptilist.ticketno
                                 select c).FirstOrDefault();

                    tc.acceptstatus = tmptilist.acceptstatus;
                    tc.rejectremark =  tmptilist.rejectremark;
                }
                */

                foreach (FTWebApi.Models.Ticket tmptilist in objlst)
                {
                    if (batchno != tmptilist.batchno)
                    {
                        var ticketlist = (from c in DataContext.Tickets
                                          where c.BatchID == tmptilist.batchno
                                          select c).AsQueryable();

                        foreach (var objticket in ticketlist)
                        {
                            Ticket tc = (from c in DataContext.Tickets
                                         where c.TicketID == objticket.TicketID
                                         select c).FirstOrDefault();

                            tc.acceptstatus = tmptilist.acceptstatus;
                            tc.rejectremark = tmptilist.rejectremark;
                        }
                    }
                    batchno = tmptilist.batchno;
                }

                DataContext.SaveChanges();
                batchno = "";
                foreach (FTWebApi.Models.Ticket tmptilist in objlst)
                {
                    if (batchno != tmptilist.batchno)
                    {
                        var ticketcount = (from a in DataContext.Tickets
                                           where a.BatchID == tmptilist.batchno
                                           select a.TicketID).Count();

                        var ticketacceptcount = (from a in DataContext.Tickets
                                                 where a.BatchID == tmptilist.batchno & a.acceptstatus == "Accept"
                                                 select a.TicketID).Count();

                        var ticketrejectcount = (from a in DataContext.Tickets
                                                 where a.BatchID == tmptilist.batchno & a.acceptstatus == "Reject"
                                                 select a.TicketID).Count();

                        var ticketduplicatecount = (from a in DataContext.Tickets
                                                    where a.BatchID == tmptilist.batchno & a.acceptstatus == "Duplicate"
                                                    select a.TicketID).Count();

                        if (ticketcount == ticketacceptcount)
                        {
                            if (tmptilist.acceptstatus == "Accept")
                            {
                                FTWebApi.Repository.Batch vbatch = DataContext.Batches.Where(x => x.BatchID == tmptilist.batchno).SingleOrDefault();
                                sendemailaccept(vbatch, "accept", "");
                            }
                        }

                        if (ticketcount == ticketrejectcount)
                        {
                            if (tmptilist.acceptstatus == "Reject")
                            {
                                FTWebApi.Repository.Batch vbatch = DataContext.Batches.Where(x => x.BatchID == tmptilist.batchno).SingleOrDefault();
                                sendemailaccept(vbatch, "reject", tmptilist.rejectremark);
                            }
                        }

                        if (ticketcount == ticketduplicatecount)
                        {
                            if (tmptilist.acceptstatus == "Duplicate")
                            {

                                FTWebApi.Repository.Batch vbatch = DataContext.Batches.Where(x => x.BatchID == tmptilist.batchno).SingleOrDefault();
                                sendemailaccept(vbatch, "duplicate", "");
                            }
                        }
                    }
                    batchno = tmptilist.batchno;
                }

            }
            catch (Exception ec)
            {
                throw ec;
            }

        }


        private void sendemailaccept(FTWebApi.Repository.Batch vbatch,string status,string rejectremark)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            string msgsubject = "";
            string msgbody = "";

            if (vbatch.FromEmail=="")
            {
                return;
            }

            message.From = new MailAddress("rcmuatquery@cms.com");
            message.To.Add(new MailAddress(vbatch.FromEmail));

            if (status=="accept")
            {
                msgsubject = vbatch.BatchID + " " + vbatch.EmailSubject;

                msgbody = "<table><tr><td>Dear Sir/Madam,</tr></td><tr><td>Thank you for writing to CMS Customer Service team!</td></tr><tr><td>Your query has been received vide batch no " + vbatch.BatchID +
                         " and will revert asap.</td></tr><tr><td>Please note the interaction number for future reference.</td></tr><tr><td>This is an auto acknowledgement mail. Kindly do not reply to this mail.</td></tr><tr><td>Regards,</td></tr><tr><td>CMS Customer Service Team</td></tr></table>";

            }
            else if (status == "duplicate")
            {
                msgsubject = "Duplicate "+ vbatch.EmailSubject;

                msgbody = "<table><tr><td>Dear Sir/Madam,</tr></td><tr><td>Thank you for writing to CMS Customer Service team!</td></tr><tr><td>Please note this is considered Duplicate request as team already working on same query against batch no" + vbatch.oldbatchid +
                         "</td></tr><tr><td>This is an auto acknowledgement mail. Kindly do not reply to this mail.</td></tr><tr><td>Regards,</td></tr><tr><td>CMS Customer Service Team</td></tr></table>";

            }
            else if (status == "reject")
            {
                msgsubject = "Rejected " + vbatch.BatchID + " " + vbatch.EmailSubject;

                msgbody = "<table><tr><td>Dear Sir/Madam,</tr></td><tr><td>Thank you for writing to CMS Customer Service team!</td></tr><tr><td>Your query has been received vide batch no " + vbatch.BatchID +
                            " and will revert asap. Please note your query has been rejected due to below concerns.</td></tr><tr><td>"+ rejectremark + "</tr></td><tr><td>This is an auto acknowledgement mail. Kindly do not reply to this mail.</td></tr><tr><td>Regards,</td></tr><tr><td>CMS Customer Service Team</td></tr></table>";

            }

            message.Subject = msgsubject;
            message.IsBodyHtml = true;
            message.Body = msgbody;

            smtp.Port = 587;
            //smtp.Host = "outlook.office365.com"; //for gmail host  
            smtp.Host = "sampark.cms.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("rcmuatquery@cms.com", "rcmu@T2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }

        public void UpdateTicket(FTWebApi.Models.Ticket objticket)
        {
            string assigneduser = "";

            try
            {
                Ticket tc = (from c in DataContext.Tickets
                                          where c.TicketID == objticket.ticketno
                                          select c).FirstOrDefault();

                if (objticket.bank != null && objticket.bank != "")
                {
                    if (objticket.assignedto == "" || objticket.assignedto == null)
                    {
                        assigneduser = (from c in DataContext.usermasters
                                        join m in DataContext.userbankmaps on c.Userid equals m.Userid 
                                        where  m.Bank.Contains(objticket.bank) & m.QueryType == objticket.tickettype & m.UserPriority == "1" 
                                        select c.Userid).SingleOrDefault();
                        tc.assignedto = assigneduser;
                        tc.querystatus = "Assigned";
                    }
                    else
                    {
                        tc.assignedto = objticket.assignedto;
                    }    
                }

                tc.tikcettype = objticket.tickettype;
                tc.acceptstatus = objticket.acceptstatus;
                //tc.resolveddate = (DateTime)objticket.resolveddate;
                tc.customer = objticket.bank;
                tc.pickupcode = objticket.pickupcode;
                tc.clientcode = objticket.clientcode;
                tc.client = objticket.client;
                tc.crnno = objticket.crnno;
                tc.area = objticket.area;
                tc.cdpncm = objticket.cdpncm;
                tc.region = objticket.region;
                tc.location = objticket.location;
                tc.hublocation = objticket.hublocation;
                tc.customertype = objticket.customertype;
                tc.hierarchycode = objticket.hierarchycode;
                tc.problem = objticket.problem;
                tc.mistakedoneby = objticket.mistakedoneby;
                tc.errortype = objticket.errortype;
                tc.querystatus = objticket.status;
                tc.customertype = objticket.customertype;
                tc.hierarchycode = objticket.hierarchycode;
                tc.Company = objticket.company;

                if (objticket.status == "Close")
                {
                    tc.resolveddate = DateTime.Now;
                }

                    //tc.ModifiedBy = objticket.modifieduser;
                tc.ModifiedDate = DateTime.Now; 

                DataContext.SaveChanges();
            }
            catch (Exception ec)
            {
                throw ec;
            }
        }

        public string ProcessTat(string datefilter)
        {
            
            DateTime dt = DateTime.Parse(datefilter);
            TimeSpan? diff;
            double hours=0;
            string html;
            DateTime dtprocess = DateTime.Today.AddDays(-3);
            DateTime tdate;
            string st1;
            TimeSpan t1;
            TimeSpan t2;

            List<FTWebApi.Models.Ticket> objticketlist = new List<FTWebApi.Models.Ticket>();
            FTWebApi.Models.Ticket objticket;

            var lstTicket = (from a in DataContext.Tickets
                             join b in DataContext.Batches on a.BatchID equals b.BatchID
                             where a.ticketdate >= dtprocess && a.ticketdate <= DateTime.Today && a.resolveddate == null  
                             select new
                             {
                                 ticketno = a.TicketID,
                                 batchno = b.BatchID,
                                 emailfrom = b.FromEmail,
                                 emailsubject = b.EmailSubject,
                                 customer = a.customer,
                                 ticketdate = a.ticketdate,
                                 tickettime = a.tikcettime,
                                 ticketstatus = a.querystatus,
                                 tickettype = a.tikcettype,
                                 createddate = a.CreatedDate,
                                 assignedto  = a.assignedto, 
                                 resolveddate = a.resolveddate
                             }).AsQueryable();

            foreach ( var ticket in lstTicket)
            {

                if (ticket.createddate.HasValue)
                {
                    tdate = ticket.createddate.Value;
                    st1 = tdate.ToString("H:mm");
                    t1 = TimeSpan.Parse(st1);
                    t2 = TimeSpan.Parse("18:00");

                    if (ticket.createddate.Value.Date == DateTime.Today && t1 <= t2)
                    {
                        diff = (DateTime.Now - ticket.createddate);
                        hours = diff.Value.TotalHours;
                    }
                    else if (ticket.createddate.Value.Date != DateTime.Today )
                    {
                        diff = (DateTime.Now - ticket.createddate);
                        hours = diff.Value.TotalHours - 16;
                    }


                    if (hours > 2 && ticket.tickettype == "PICKUP")
                    {
                        objticket = new FTWebApi.Models.Ticket
                        {
                            batchno = ticket.batchno,
                            ticketno = ticket.ticketno,
                            ticketdate = ticket.ticketdate,
                            tickettype = ticket.tickettype,
                            status = ticket.ticketstatus,
                            bank = ticket.customer,
                            emailfrom = ticket.emailfrom,
                            assignedto = ticket.assignedto,
                            emailsubject = ticket.emailsubject
                        };

                        objticketlist.Add(objticket);

                    }
                    else if (hours > 4 && ticket.tickettype == "CREDIT")
                    {
                        objticket = new FTWebApi.Models.Ticket
                        {
                            batchno = ticket.batchno,
                            ticketno = ticket.ticketno,
                            ticketdate = ticket.ticketdate,
                            tickettype = ticket.tickettype,
                            status = ticket.ticketstatus,
                            bank = ticket.customer,
                            emailfrom = ticket.emailfrom,
                            assignedto = ticket.assignedto,
                            emailsubject = ticket.emailsubject
                        };

                        objticketlist.Add(objticket);
                    }
                }        

            }

            html = GenerateHtmlstring(objticketlist);

            return html;
        }

        public string GenerateHtmlstring(List<FTWebApi.Models.Ticket> objticket)
        {
            var table = new HtmlTable();
            var mailMessage = new StringBuilder();
            string html;

            HtmlTableRow row = new HtmlTableRow();

            row.Attributes.Add("style", "height: 30px; border: 1px solid;");
            row.Cells.Add(new HtmlTableCell { InnerText = "Batchno" });
            row.Cells[0].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Ticketno" });
            row.Cells[1].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Ticket date" });
            row.Cells[2].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Ticket Type" });
            row.Cells[3].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Customer" });
            row.Cells[4].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Query Status" });
            row.Cells[5].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Email from" });
            row.Cells[6].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Email Subject" });
            row.Cells[7].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Assigned User" });
            row.Cells[8].Attributes.Add("style", "border-bottom: black thin solid");
            table.Rows.Add(row);

            foreach (FTWebApi.Models.Ticket objtd in objticket)
            {
                row = new HtmlTableRow();
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.batchno });
                row.Cells[0].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.ticketno });
                row.Cells[1].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.ticketdate.ToString() });
                row.Cells[2].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.tickettype });
                row.Cells[3].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.bank });
                row.Cells[4].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.status });
                row.Cells[5].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.emailfrom });
                row.Cells[6].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.emailsubject });
                row.Cells[7].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = objtd.assignedto });
                row.Cells[8].Attributes.Add("style", "border-bottom: black thin solid");
                table.Rows.Add(row);
            }
        

            using (var sw = new StringWriter())
            {
                table.RenderControl(new HtmlTextWriter(sw));
                html = sw.ToString();
            }
            return html;
        }

public string GetReportData(string fromdate,string todate,string customer, string user,string customertype,
            string region, string location, string hublocation, string cdpncm, string issuetype, string sla)
        {
            DateTime dtfromdate = DateTime.Parse(fromdate);
            DateTime dttodate = DateTime.Parse(todate);
            DateTime TransactionDate;
            string scustomer;
            string suser;
            string scustomertype;
            string sregion,slocation, shublocation, scdpncm, sissuetype, ssla;


            scustomer = (customer == "undefined") ? null : customer;
            suser = (user == "undefined") ? null : user;
            scustomertype = (customertype == "undefined") ? null : customertype;
            sregion = (region == "undefined") ? null : region;
            slocation = (location == "undefined") ? null : location;
            shublocation = (hublocation == "undefined") ? null : hublocation;
            scdpncm = (cdpncm == "undefined") ? null : cdpncm;
            sissuetype = (issuetype == "undefined") ? null : issuetype; 
            ssla = (sla == "undefined") ? null : sla;


            var lstTicket = (from a in DataContext.TicketDetails
                             join b in DataContext.Tickets on a.TicketID equals b.TicketID
                             join c in DataContext.Batches on b.BatchID equals c.BatchID
                             join d in DataContext.ErrorTypes on b.errortype equals d.ID.ToString()
                             where b.ticketdate >= dtfromdate && b.ticketdate <= dttodate && b.customer == (scustomer ?? b.customer)
                             && b.assignedto == (suser ?? b.assignedto) && b.customertype == (scustomertype ?? b.customertype)
                             && b.region == (sregion ?? b.region) && b.location == (slocation ?? b.location) && b.hublocation == (shublocation ?? b.hublocation)
                             && b.cdpncm == (scdpncm ?? b.cdpncm) && b.tikcettype == (sissuetype ?? b.tikcettype)
                             select new
                             {
                                 assignedto = b.assignedto,
                                 batchid = b.BatchID,
                                 ticketno = b.TicketID,
                                 ticketdate = b.ticketdate,
                                 tickettime = b.tikcettime,
                                 ticketype = b.tikcettype,
                                 ticketstatus = b.acceptstatus,
                                 resolveddate = b.resolveddate,
                                 createddate = b.CreatedDate,
                                 emailfrom = c.FromEmail,
                                 emailsubject = c.EmailSubject,
                                 querytype = b.tikcettype,
                                 typeoferror = d.ErrorType1,
                                 problemdetails = b.problem,
                                 region = b.region,
                                 location = b.location,
                                 hublocation = b.hublocation,
                                 customer = b.customer,
                                 customertype = b.customertype,
                                 transactiondate = a.pickupdate,
                                 crnno = b.crnno,
                                 clientname = b.client,
                                 area = b.area,
                                 cdpncm = b.cdpncm,
                                 wrongclientcode = a.wrongclientcode,
                                 actualclientcode = a.clientcode,
                                 wrongpickupcode = a.wrongpickupcode,
                                 actualpickupcode = a.pickupcode,
                                 actualhiecode = b.hierarchycode,
                                 wrongcustomeruniquecode = a.wrongcustomeruniquecode,
                                 actualcustomeruniquecode = a.customeruniquecode,
                                 wronghcin = a.wronghcin,
                                 actualhcin = a.actualhcin,
                                 wrongdispis = a.wrongdispis,
                                 actualdispis = a.actualdispis,
                                 wrongamt = a.wrongamt,
                                 actualamt = a.actualamt,
                                 depostiondate = a.depostiondate,
                                 querystatus = b.querystatus,
                                 company = b.Company,
                                 rejectremark = b.rejectremark,
                                 oldbatchid = c.oldbatchid,
                                 aging = 0,
                                 sla = ""
                             });

            var table = new HtmlTable();
            var mailMessage = new StringBuilder();
            string html="";
            Int32 count = 0;

            if (lstTicket != null)
            {
                HtmlTableRow row = new HtmlTableRow();
                row.Cells.Add(new HtmlTableCell { InnerText = "SR"});
                row.Cells.Add(new HtmlTableCell { InnerText = "Assigned to" });
                row.Cells.Add(new HtmlTableCell { InnerText = "CRA - CMS/SIPL" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Batch id" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Ticket no" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Ticket Date" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Ticket Time" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Ticket status" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Reject Remark" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Ticket Resolved Date" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Aging" });
                row.Cells.Add(new HtmlTableCell { InnerText = "SLA" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Email from" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Email subject" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Query type" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Type of error" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Problem details" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Region" });
                row.Cells.Add(new HtmlTableCell { InnerText = "location" });
                row.Cells.Add(new HtmlTableCell { InnerText = "hublocation" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Customer" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Customer type" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Transaction Date" });
                row.Cells.Add(new HtmlTableCell { InnerText = "CRN no" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Client name" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Area" });
                row.Cells.Add(new HtmlTableCell { InnerText = "cdpncm" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Clientcode" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Actual Clientcode" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Pickupcode" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Actual Pickupcode" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Hie Code" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Actual Hie Code" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Customeruniquecode" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Actual Customeruniquecode" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Hcin" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Actual Hcin" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Dispis" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Actual Dispis" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Amount" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Actual Amount" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Depostion date" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Original BatchId" });
                row.Cells.Add(new HtmlTableCell { InnerText = "Query Status" });
                table.Rows.Add(row);

                foreach (var objreport in lstTicket)
                {
                    count++;
                    row = new HtmlTableRow();
                    row.Cells.Add(new HtmlTableCell { InnerText = count.ToString() });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.assignedto });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.company });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.batchid });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.ticketno });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.ticketdate.ToShortDateString() });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.tickettime.ToString() });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.ticketstatus});
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.rejectremark });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.resolveddate.ToString() });

                    if (objreport.resolveddate == null)
                    {
                        var diff = (DateTime.Now - objreport.createddate);
                        var hours = Math.Round(diff.Value.TotalHours, 2);
                        var days = Math.Round(hours / 24, 0);
                        row.Cells.Add(new HtmlTableCell { InnerText = days.ToString() });
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.sla });
                    }
                    else
                    {
                        var diff = (objreport.resolveddate - objreport.createddate);
                        var hours = Math.Round(diff.Value.TotalHours,2);
                        var days = Math.Round(hours / 24, 0);
                        row.Cells.Add(new HtmlTableCell { InnerText = days.ToString() });

                        if (objreport.ticketype == "PICKUP")
                        {
                            if (hours <=2)
                            {
                                row.Cells.Add(new HtmlTableCell { InnerText = "Within TAT" });
                            }
                            else
                            {
                                row.Cells.Add(new HtmlTableCell { InnerText = "After TAT" });
                            }
                        }
                        else
                        {
                            if (hours <= 4)
                            {
                                row.Cells.Add(new HtmlTableCell { InnerText = "Within TAT" });
                            }
                            else
                            {
                                row.Cells.Add(new HtmlTableCell { InnerText = "After TAT" });
                            }
                        }            

                    }
                    
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.emailfrom });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.emailsubject });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.querytype });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.typeoferror });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.problemdetails });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.region });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.location });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.hublocation });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.customer });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.customertype });
                    if (objreport.transactiondate.HasValue)
                    {
                        TransactionDate = objreport.transactiondate.Value;
                        row.Cells.Add(new HtmlTableCell { InnerText = TransactionDate.ToShortDateString() });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.crnno });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.clientname });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.area });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.cdpncm });
                    
                    if (objreport.wrongclientcode=="NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.wrongclientcode });
                    }

                    if (objreport.actualclientcode == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.actualclientcode });
                    }

                    if (objreport.wrongpickupcode == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.wrongpickupcode });
                    }

                    if (objreport.actualpickupcode == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.actualpickupcode });
                    }

                    if (objreport.wrongcustomeruniquecode == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.wrongcustomeruniquecode });
                    }

                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.actualhiecode });
                    row.Cells.Add(new HtmlTableCell { InnerText = "" });

                    if (objreport.actualcustomeruniquecode == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.actualcustomeruniquecode });
                    }

                    if (objreport.wronghcin == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.wronghcin });
                    }

                    if (objreport.actualhcin == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.actualhcin });
                    }

                    if (objreport.wrongdispis == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.wrongdispis });
                    }

                    if (objreport.actualdispis == "NA")
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = "" });
                    }
                    else
                    {
                        row.Cells.Add(new HtmlTableCell { InnerText = objreport.actualdispis });
                    }

                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.wrongamt.ToString() });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.actualamt.ToString() });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.depostiondate.ToString() });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.oldbatchid });
                    row.Cells.Add(new HtmlTableCell { InnerText = objreport.querystatus });

                    table.Rows.Add(row);
                }

                using (var sw = new StringWriter())
                {
                    table.RenderControl(new HtmlTextWriter(sw));
                    html = sw.ToString();
                }

                return html;
            }
            return html;
        }   

        public string getticketclosecount(string batchno)
        {
            string sresponse = "";

            var ticketcount = (from a in DataContext.Tickets
                               where a.BatchID == batchno
                               select a.TicketID).Count();

            var ticketclosecount = (from a in DataContext.Tickets
                                    where a.BatchID == batchno & a.querystatus == "Close"
                                    select a.TicketID).Count();


            if (ticketcount == ticketclosecount+1 )
            {
                sresponse="allclose";
            }

            return sresponse;
        }

        public string checkticketdata(string ticketno)
        {
            string sresponse = "";

            var modifieddate = (from a in DataContext.TicketDetails
                               where a.TicketID == ticketno
                               select a.ModifiedDate).SingleOrDefault();



            if (modifieddate is null)
            {
                sresponse = "N";
            }
            else
            {
                sresponse = "Y";
            }

            return sresponse;
        }

        public string GenerateHtmlResponse(FTWebApi.Models.Ticket objticket)
        {
            var table = new HtmlTable();
            var mailMessage = new StringBuilder();
            string html;
            Int32 count = 0;

            var ticketcount = (from a in DataContext.Tickets
                               where a.BatchID == objticket.batchno
                               select a.TicketID).Count();

            var ticketclosecount = (from a in DataContext.Tickets
                                     where a.BatchID == objticket.batchno & a.querystatus == "Close"
                                     select a.TicketID).Count();

            if (ticketcount != ticketclosecount)
            {
                html = "notallclose";
                return html;
            }
         
            table.Attributes.Add("style", "border: 1px solid;");


            HtmlTableRow row = new HtmlTableRow();
            //row.Attributes.Add("style", "height: 30px; border: 1px solid;");
            row.Cells.Add(new HtmlTableCell { InnerText = "SR" });
            row.Cells[0].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "CMS/SIPL" });
            row.Cells[1].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Batch Id" });
            row.Cells[2].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Ticket Id" });
            row.Cells[3].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Transaction Date" });
            row.Cells[4].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Query Type" });
            row.Cells[5].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "CRN No" });
            row.Cells[6].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Customer Unique Code" });
            row.Cells[7].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Hub Location" });
            row.Cells[8].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Customer Name" });
            row.Cells[9].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Client Name" });
            row.Cells[10].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Client Code" });
            row.Cells[11].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "Pickup Code" });
            row.Cells[12].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            row.Cells.Add(new HtmlTableCell { InnerText = "HCIN No" });
            row.Cells[13].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
            if (objticket.tickettype == "CREDIT")
            {
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Amt" });
                row.Cells[14].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Correct Amt" });
                row.Cells[15].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Wrong Deposit Slip No" });
                row.Cells[16].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Correct Deposit Slip No" });
                row.Cells[17].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Deposition Date" });
                row.Cells[18].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Type of Error" });
                row.Cells[19].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Problem Details" });
                row.Cells[20].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Query Status" });
                row.Cells[21].Attributes.Add("style", "border-bottom: black thin solid");

            }
            else
            {
                row.Cells.Add(new HtmlTableCell { InnerText = "Correct Amt" });
                row.Cells[14].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Deposit Slip No" });
                row.Cells[15].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Problem Details" });
                row.Cells[16].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                row.Cells.Add(new HtmlTableCell { InnerText = "Query Status" });
                row.Cells[17].Attributes.Add("style", "border-bottom: black thin solid");
            }
            
            table.Rows.Add(row);

            var tlist = DataContext.Tickets.Where(x => x.BatchID == objticket.batchno);

            foreach(var ticket in tlist)
            {
                var error = (from a in DataContext.ErrorTypes
                             where ticket.errortype == a.ID.ToString()
                             select a.ErrorType1).SingleOrDefault();

                var lstTicket = (from a in DataContext.TicketDetails
                                 join b in DataContext.Tickets on a.TicketID equals b.TicketID
                                 where a.TicketID == ticket.TicketID
                                 select new FTWebApi.Models.ticketdetails
                                 {
                                     id = a.ID,
                                     ticketno = a.TicketID,
                                     pickupdate = a.pickupdate,
                                     pickupcode = a.pickupcode,
                                     clientcode = a.clientcode,
                                     crnno = b.crnno,
                                     actualhcin = a.actualhcin,
                                     wronghcin = a.wronghcin,
                                     actualdispis = a.actualdispis,
                                     wrongdispis = a.wrongdispis,
                                     actualamt = a.actualamt,
                                     wrongamt = a.wrongamt,
                                     customeruniquecode = a.customeruniquecode,
                                     wrongpickupcode = a.wrongpickupcode,
                                     wrongclientcode = a.wrongclientcode,
                                     wrongcustomeruniquecode = a.wrongcustomeruniquecode,
                                     depositiondate = a.depostiondate
                                 }).AsQueryable();


                if (lstTicket != null)
                {
                   foreach (FTWebApi.Models.ticketdetails objtd in lstTicket)
                    {
                        count++;
                        row = new HtmlTableRow();
                        row.Cells.Add(new HtmlTableCell { InnerText = count.ToString() });
                        row.Cells[0].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = "CMS" });
                        row.Cells[1].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = ticket.BatchID });
                        row.Cells[2].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = ticket.TicketID });
                        row.Cells[3].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = ticket.ticketdate.ToString() });
                        row.Cells[4].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = ticket.tikcettype });
                        row.Cells[5].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = ticket.crnno });
                        row.Cells[6].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = objtd.customeruniquecode });
                        row.Cells[7].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = ticket.hublocation });
                        row.Cells[8].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = ticket.customer });
                        row.Cells[9].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = ticket.client });
                        row.Cells[10].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = objtd.clientcode });
                        row.Cells[11].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        row.Cells.Add(new HtmlTableCell { InnerText = objtd.pickupcode });
                        row.Cells[12].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        if (objtd.actualhcin=="NA")
                        {
                            objtd.actualhcin = "";
                        }
                        row.Cells.Add(new HtmlTableCell { InnerText = objtd.actualhcin });
                        row.Cells[13].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                        if (objticket.tickettype == "CREDIT")
                        {
                            row.Cells.Add(new HtmlTableCell { InnerText = objtd.wrongamt.ToString() });
                            row.Cells[14].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            row.Cells.Add(new HtmlTableCell { InnerText = objtd.actualamt.ToString() });
                            row.Cells[15].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            if (objtd.wrongdispis == "NA")
                            {
                                objtd.wrongdispis = "";
                            }
                            row.Cells.Add(new HtmlTableCell { InnerText = objtd.wrongdispis });
                            row.Cells[16].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            if (objtd.actualdispis == "NA")
                            {
                                objtd.actualdispis = "";
                            }
                            row.Cells.Add(new HtmlTableCell { InnerText = objtd.actualdispis });
                            row.Cells[17].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            row.Cells.Add(new HtmlTableCell { InnerText = objtd.depositiondate.ToString() });
                            row.Cells[18].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            row.Cells.Add(new HtmlTableCell { InnerText = error });
                            row.Cells[19].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            row.Cells.Add(new HtmlTableCell { InnerText = ticket.problem });
                            row.Cells[20].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            row.Cells.Add(new HtmlTableCell { InnerText = ticket.querystatus });
                            row.Cells[21].Attributes.Add("style", "border-bottom: black thin solid");

                        }
                        else
                        {
                            row.Cells.Add(new HtmlTableCell { InnerText = objtd.actualamt.ToString() });
                            row.Cells[14].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            row.Cells.Add(new HtmlTableCell { InnerText = objtd.actualdispis });
                            row.Cells[15].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            row.Cells.Add(new HtmlTableCell { InnerText = ticket.problem });
                            row.Cells[16].Attributes.Add("style", "border-bottom: black thin solid;border-right:black thin solid");
                            row.Cells.Add(new HtmlTableCell { InnerText = ticket.querystatus });
                        }
                        //row.Cells[17].Attributes.Add("style", "border-bottom: black thin solid");
                        table.Rows.Add(row);
                    }
                }

            }

            
            using (var sw = new StringWriter())
            {
                table.RenderControl(new HtmlTextWriter(sw));
                html = sw.ToString();
            }
            return html;
        }    


         private string genticketno()
        {
            var ticketno = (from c in DataContext.KeyGens
                            where c.Name == "ticket"
                            select c.srno).SingleOrDefault();

            Int32 nticketno =  Int32.Parse(ticketno) + 1;

            UpdatekeyGen(nticketno, "ticket");

            string sticketno = "T000" + nticketno.ToString();

            return sticketno;
        }

        private void UpdatekeyGen(Int32 nticketno,string strkey)
        {
            try
            {
                KeyGen kg = (from c in DataContext.KeyGens
                         where c.Name == strkey
                             select c).FirstOrDefault();

                kg.srno = nticketno.ToString();

                DataContext.SaveChanges();
            }
            catch (Exception ec)
            {
                throw ec;
            }

        }

        private string getbatchno()
        {
            var ticketno = (from c in DataContext.KeyGens
                            where c.Name == "batch"
                            select c.srno).SingleOrDefault();

            Int32 nticketno = Int32.Parse(ticketno) + 1;

            UpdatekeyGen(nticketno, "batch");

            string sticketno = "B000" + nticketno.ToString();

            return sticketno;
        }

        public string InsertBatch(FTWebApi.Models.batch objbatch)
        {
            string batchno = getbatchno();
            try
            {

                Batch bt = new Batch();

                if (objbatch.batchid == "" || objbatch.batchid is null)
                {
                    bt.BatchID = batchno;
                }
                else
                {
                    bt.BatchID = objbatch.batchid;
                }

                bt.Date = objbatch.batchdate;
                bt.EmailBody = objbatch.emailbody;
                bt.EmailSubject = objbatch.emailsubject;
                bt.FromEmail = objbatch.fromemail;
                bt.Emailcc = objbatch.emailcc;

                var count = (from b in DataContext.Batches
                             where b.Date.Value.Year == objbatch.batchdate.Year
                             && b.Date.Value.Month == objbatch.batchdate.Month
                             && b.Date.Value.Day == objbatch.batchdate.Day
                             && b.EmailSubject == objbatch.emailsubject
                             select b.BatchID).Count();

                if (count > 0)
                {
                    var OldBatchno = (from b in DataContext.Batches
                                      where b.Date.Value.Year == objbatch.batchdate.Year
                                      && b.Date.Value.Month == objbatch.batchdate.Month
                                      && b.Date.Value.Day == objbatch.batchdate.Day
                                      && b.EmailSubject == objbatch.emailsubject
                                      select b.BatchID).AsEnumerable();

                    bt.oldbatchid = OldBatchno.Max();
                    bt.acceptstatus = "MDuplicate";
                }
                else
                {
                    bt.acceptstatus = "MAccept";
                }

                DataContext.Batches.Add(bt);
                DataContext.SaveChanges();
            }
            catch (Exception ec)
            {
                throw ec;
            }
            return batchno;
        }

        public string getlastbatchno()
        {
            var maxValue = DataContext.Batches.Max(x => x.BatchID);

            return maxValue.ToString();

        }

        public void InsertTicketDetails(FTWebApi.Models.ticketdetails[] objticketdetails)
        {
            TicketDetail td = new TicketDetail();

            foreach (FTWebApi.Models.ticketdetails objtmp in objticketdetails)
            {
                TicketDetail lstom = (from om in DataContext.TicketDetails
                             where om.TicketID == objtmp.ticketno & om.ID == objtmp.id
                             select om).SingleOrDefault();
                if (lstom != null)
                {
                    DataContext.TicketDetails.Remove(lstom);
                    DataContext.SaveChanges();
                }
            }

            foreach (FTWebApi.Models.ticketdetails objtmp in objticketdetails)
            {
                td.TicketID = objtmp.ticketno;
                td.pickupdate = objtmp.pickupdate;
                td.pickupcode = objtmp.pickupcode;
                td.clientcode = objtmp.clientcode;
                td.crnno = objtmp.crnno;
                td.customeruniquecode = objtmp.customeruniquecode;
                td.actualhcin = objtmp.actualhcin;
                td.wronghcin = objtmp.wronghcin;
                td.actualdispis = objtmp.actualdispis;
                td.wrongdispis = objtmp.wrongdispis;
                td.actualamt = objtmp.actualamt;
                td.wrongamt = objtmp.wrongamt;
                td.wrongpickupcode = objtmp.wrongpickupcode;
                td.wrongclientcode = objtmp.wrongclientcode;
                td.wrongcustomeruniquecode = objtmp.wrongcustomeruniquecode;
                td.depostiondate = objtmp.depositiondate;
                td.soleid = objtmp.soleid;
                td.bankbranchlocation = objtmp.bankbranchlocation;
                td.ModifiedDate = DateTime.Now;

                DataContext.TicketDetails.Add(td);

                DataContext.SaveChanges();
            }    
        }

        public void InsertTicket(FTWebApi.Models.Ticket objticket)
        {
            string ticketno = genticketno();
            string assigneduser = "";
            string sbatchno = "";

            Ticket tc = new Ticket();
            tc.TicketID = ticketno;

            if (objticket.batchno == "" || objticket.batchno is null)
            {
                sbatchno = getbatchno();
                tc.BatchID = sbatchno;

                Batch bt = new Batch();

                bt.BatchID = sbatchno;
                bt.Date = objticket.ticketdate;
                bt.EmailBody = "EmailBody";
                bt.EmailSubject = objticket.emailsubject;
                bt.FromEmail = objticket.emailfrom;
                bt.Emailcc = objticket.emailcc;
                bt.CreatedDate = DateTime.Now;

                DataContext.Batches.Add(bt);
            }
            else
            {
                tc.BatchID = objticket.batchno;
            }

            tc.acceptstatus = "MAccept";
            tc.querystatus = "Open";
            if (objticket.bank != null && objticket.bank != "")
            {
                if (objticket.assignedto == "" || objticket.assignedto == null)
                {

                    if (objticket.tickettype != "" && objticket.tickettype != null)
                    {
                        assigneduser = (from c in DataContext.usermasters
                                        join m in DataContext.userbankmaps on c.Userid equals m.Userid
                                        where m.Bank.Contains(objticket.bank) & m.QueryType == objticket.tickettype & m.UserPriority == "1"
                                        select c.Userid).SingleOrDefault();
                    }
                    else
                    {
                        assigneduser = (from c in DataContext.usermasters
                                        join m in DataContext.userbankmaps on c.Userid equals m.Userid
                                        where m.Bank.Contains(objticket.bank) & m.UserPriority == "1"
                                        select c.Userid).FirstOrDefault();
                    }

                    tc.assignedto = assigneduser;
                    tc.querystatus = "Assigned";
                }
                else
                {
                    tc.assignedto = objticket.assignedto;
                }
            }

            tc.ticketdate = objticket.ticketdate;
            tc.tikcettime = objticket.tickettime;
            tc.tikcettype = objticket.tickettype ;
            tc.customer = objticket.bank;
            tc.pickupcode = objticket.pickupcode;
            tc.clientcode = objticket.clientcode;
            tc.client = objticket.client;
            tc.crnno = objticket.crnno;
            tc.area = objticket.area;
            tc.cdpncm = objticket.cdpncm;
            tc.region = objticket.region;
            tc.location = objticket.location;
            tc.hublocation = objticket.hublocation;
            tc.customertype = objticket.customertype;
            tc.hierarchycode = objticket.hierarchycode;
            tc.problem = objticket.problem;
            tc.mistakedoneby = objticket.mistakedoneby;
            tc.errortype = objticket.errortype;
            tc.CreatedBy = objticket.createduser;
            tc.CreatedDate = DateTime.Now;
            
            DataContext.Tickets.Add(tc);

            


            DataContext.SaveChanges();

        }


        public void InsertTicketList(FTWebApi.Models.Ticket[] objticketlst)
        {

            Ticket tc = new Ticket();
            string ticketno;
            string assigneduser = "";

            try
            {
                foreach (FTWebApi.Models.Ticket tmptilist in objticketlst)
                {
                        ticketno = genticketno();
                        assigneduser = "";


                    if (tmptilist.ticketno == "" || tmptilist.ticketno is null)
                        {
                            tc.TicketID = ticketno;
                        }
                        else
                        {
                            tc.TicketID = tmptilist.ticketno;
                        }

                        if (tmptilist.bank != null && tmptilist.bank != "")
                        {
                            if (tmptilist.assignedto == "" || tmptilist.assignedto == null)
                            {
                                assigneduser = (from c in DataContext.usermasters
                                            join m in DataContext.userbankmaps on c.Userid equals m.Userid
                                            where m.Bank == tmptilist.bank & m.QueryType == tmptilist.tickettype & m.UserPriority == "1"
                                            select c.Userid).SingleOrDefault();

                                tc.assignedto = assigneduser;
                            }
                            else
                            {
                                tc.assignedto = tmptilist.assignedto;
                            }
                        }

                        tc.ticketdate = tmptilist.ticketdate;
                        tc.BatchID = tmptilist.batchno;
                        tc.tikcettime = TimeSpan.Parse("12:15");
                        tc.tikcettype = tmptilist.tickettype;
                        tc.assignedto = tmptilist.assignedto;
                        tc.acceptstatus = tmptilist.acceptstatus;
                        //tc.resolveddate = (DateTime)tmptilist.resolveddate;
                        tc.customer = tmptilist.bank;
                        tc.pickupcode = tmptilist.pickupcode;
                        tc.clientcode = tmptilist.clientcode;
                        tc.crnno = tmptilist.crnno;
                        tc.area = tmptilist.area;
                        tc.cdpncm = tmptilist.cdpncm;
                        tc.region = tmptilist.region;
                        tc.location = tmptilist.location;
                        tc.hublocation = tmptilist.hublocation;
                        tc.problem = tmptilist.problem;
                        tc.mistakedoneby = tmptilist.mistakedoneby;
                        tc.errortype = tmptilist.errortype;
                        tc.querystatus = tmptilist.status;
                        tc.CreatedBy = tmptilist.createduser;
                        //tc.CreatedDate = (DateTime)tmptilist.createddate;
                        DataContext.Tickets.Add(tc);
                        DataContext.SaveChanges();
                    }
                

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void InsertTicketBulk(FTWebApi.Models.Ticketbulk[] objticketlst)
        {
            Ticket tc;
            TicketDetail td;
            string ticketno="";
            string assigneduser = "";

            try
            {
                foreach (FTWebApi.Models.Ticketbulk tmptilist in objticketlst)
                {
                    tc = new Ticket();
                    td = new TicketDetail();
                    ticketno="";
                    assigneduser = "";

                    ticketno = genticketno();
                    assigneduser = "";

                    if (tmptilist.ticketno == "" || tmptilist.ticketno is null)
                    {
                        tc.TicketID = ticketno;
                    }
                    else
                    {
                        tc.TicketID = tmptilist.ticketno;
                    }
                    tc.querystatus = "Open";
                    if (tmptilist.bank != null && tmptilist.bank != "")
                    {
                        if (tmptilist.tickettype != "" && tmptilist.tickettype !=null)
                        {
                            assigneduser = (from c in DataContext.usermasters
                                            join m in DataContext.userbankmaps on c.Userid equals m.Userid
                                            where m.Bank.Contains(tmptilist.bank) & m.QueryType == tmptilist.tickettype & m.UserPriority == "1"
                                            select c.Userid).SingleOrDefault();
                        }
                        else
                        {
                            assigneduser = (from c in DataContext.usermasters
                                            join m in DataContext.userbankmaps on c.Userid equals m.Userid
                                            where m.Bank.Contains(tmptilist.bank) & m.UserPriority == "1"
                                            select c.Userid).FirstOrDefault();
                        }

                        tc.assignedto = assigneduser;
                        tc.querystatus = "Assigned";
                    }

                    var crnno = (from a in rcmDataContext.ClientCustMasters
                                 join b in rcmDataContext.ClientCustAccountMasters on a.ClientCustCode equals b.ClientCustCode
                                 join c in rcmDataContext.CustomerMasters on a.CustomerCode equals c.CustomerCode
                                 join r in rcmDataContext.RegionMasts on a.Regioncode equals r.regioncode
                                 join l in rcmDataContext.LocationMasts on a.LocationCode equals l.locationcode
                                 join area in rcmDataContext.LocationMasters on a.AreaCode equals area.LocationCode
                                 join h in rcmDataContext.HublocationMasts on a.HubLocationCode equals h.hublocationcode
                                 join j in rcmDataContext.ClientCustBankDetails on a.ClientCustCode equals j.ClientCustCode
                                 where b.PickupCode == tmptilist.pickupcode & b.ClientCode == tmptilist.clientcode & c.CustomerName.StartsWith(tmptilist.bank.Substring(0, 3)) & b.AccountId == tmptilist.clientcode
                                 & l.regioncode == r.regioncode & h.locationcode == l.locationcode
                                 select new rcmdetail
                                 {
                                     crn = a.ClientCustCode,
                                     clientname = a.ClientCustName,
                                     region = r.regionname,
                                     location = l.locationname,
                                     hublocation = h.hublocationname,
                                     area = area.LocationName,                                     
                                     customertype = a.AccountType,
                                     cdpncm = j.DepositionType,
                                     hierarchycode = b.HierarchyCode,
                                     Company = a.CompanyCode
                                 }).SingleOrDefault();


                    tc.ticketdate = tmptilist.ticketdate;
                    tc.BatchID = tmptilist.batchno;

                    var batchstatus = (from b in DataContext.Batches
                                       where b.BatchID == tmptilist.batchno
                                       select b.acceptstatus).SingleOrDefault();

                    tc.tikcettime = tmptilist.tickettime;
                    tc.tikcettype = tmptilist.tickettype;
                    //tc.assignedto = tmptilist.assignedto;
                    if (batchstatus != null && batchstatus != "")
                    {
                        tc.acceptstatus = batchstatus;
                    }
                    else
                    {
                        tc.acceptstatus = "Open";
                    }    
                    //tc.resolveddate = (DateTime)tmptilist.resolveddate;
                    tc.customer = tmptilist.bank;
                    tc.pickupcode = tmptilist.pickupcode;
                    tc.clientcode = tmptilist.clientcode;

                    if (crnno != null)
                    {
                        tc.crnno = crnno.crn ;
                        tc.client = crnno.clientname;
                        tc.area = crnno.area;
                        tc.cdpncm = crnno.cdpncm;
                        tc.region = crnno.region;
                        tc.location = crnno.location;
                        tc.hublocation = crnno.hublocation;
                        tc.customertype = crnno.customertype;
                        tc.hierarchycode = crnno.hierarchycode;
                        tc.Company = crnno.Company;
                    }
                    else
                    {
                        tc.crnno = tmptilist.crnno;
                        tc.area = tmptilist.area;
                        tc.cdpncm = tmptilist.cdpncm;
                        tc.region = tmptilist.region;
                        tc.location = tmptilist.location;
                        tc.hublocation = tmptilist.hublocation;
                        tc.customertype = tmptilist.customertype;
                        tc.hierarchycode = tmptilist.hierarchycode;
                    }

                    tc.problem = tmptilist.problem;
                    tc.mistakedoneby = tmptilist.mistakedoneby;
                    tc.errortype = tmptilist.errortype;
                    
                    tc.CreatedBy = tmptilist.createduser;
                    tc.CreatedDate =  DateTime.Now;

                    DataContext.Tickets.Add(tc);

                    td.TicketID = ticketno;
                    td.clientcode = tmptilist.clientcode;
                    td.pickupcode = tmptilist.pickupcode;
                    td.crnno = tmptilist.crnno;
                    td.actualamt = tmptilist.actualamt;
                    td.actualdispis = tmptilist.actualdispis;
                    td.actualhcin = tmptilist.actualhcin;
                    td.wrongamt = tmptilist.wrongamt;
                    td.wrongdispis = tmptilist.wrongdispis;
                    td.wronghcin = tmptilist.wronghcin;
                    td.pickupdate = tmptilist.pickupdate;
                    td.customeruniquecode = tmptilist.customeruniquecode;

                    if (td.actualamt != 0 || td.actualdispis !="" || td.actualdispis != "" || td.actualhcin != "" || td.wrongdispis != "" || td.wronghcin != ""
                        || td.wrongamt !=0 || td.customeruniquecode !="")
                    {
                        DataContext.TicketDetails.Add(td);
                    }     
                    
                }

                DataContext.SaveChanges();



            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
