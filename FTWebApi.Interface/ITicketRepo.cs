using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWebApi.Interface
{
    public interface ITicketRepo
    {
        FTWebApi.Models.rcmdetail getcrnno(string customer, string pickupcode, string clientcode);
        IQueryable<FTWebApi.Models.Ticket> GetAllTickets(string userid,string userrole);
        FTWebApi.Models.Ticket GetTicket(string ID);
        void UpdateTicket(FTWebApi.Models.Ticket objticket);
        void InsertTicket(FTWebApi.Models.Ticket objticket);
        void InsertTicketList(FTWebApi.Models.Ticket[] objticketlst);
        string InsertBatch(FTWebApi.Models.batch objbatch);

        //IQueryable<FTWebApi.Models.QBank> GetQBank();

    }
}
