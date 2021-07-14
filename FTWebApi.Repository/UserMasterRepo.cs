using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTWebApi.Models;
using FTWebApi.Interface;

namespace FTWebApi.Repository
{
    public class UserMasterRepo : IUserMasterRepo
    {
        TMSEntitiesnew DataContext = new TMSEntitiesnew();

        public IQueryable<FTWebApi.Models.UserMaster> GetAllUser()
        {
                    var lstsm = DataContext.usermasters.Select(a => new FTWebApi.Models.UserMaster
                    {
                       UserId = a.Userid,
                       UserName = a.UserName,
                       UserPass = a.UserPassword,
                       UserRole = a.UserRole
                    }).AsQueryable();
            
            return lstsm;
        }

        public IQueryable<FTWebApi.Models.userbankmap>  GetUserBankMaster()
        {
            var lstuserbankmap = (from u in DataContext.BankMasters
                                  join q in DataContext.QueryTypeMasters on 1 equals 1
                                  orderby u.Bank
                                 select new FTWebApi.Models.userbankmap
                                 {
                                     UserId = "",
                                     Bank = u.Bank,
                                     QueryType = q.QueryType,
                                     UserPriority=""
                                 }).AsQueryable();

            return lstuserbankmap;
        }

        public FTWebApi.Models.UserMaster GetUser(String ID)
        {
            //IQueryable<FTWebApi.Models.userbankmap> objub ;
            //FTWebApi.Models.userbankmap[] objub;

            FTWebApi.Models.UserMaster objum;

            objum = DataContext.usermasters.Where(p => p.Userid == ID).Select(a => new FTWebApi.Models.UserMaster
            {
                UserId = a.Userid,
                UserName = a.UserName,
                UserPass = a.UserPassword,
                UserRole = a.UserRole

            }).SingleOrDefault();

            var ub = (from u in DataContext.userbankmaps
                      where u.Userid == ID
                      select new FTWebApi.Models.userbankmap
                      {
                          UserId = u.Userid,
                          Bank = u.Bank,
                          QueryType = u.QueryType,
                          UserPriority = u.UserPriority
                      }).ToArray();

            objum.lstuserbankmap = ub;

            return objum;
        }


        public Int32 ValidatePriorityUser(string bank,string querytype,string userid)
        {
            Int32 count = (from om in DataContext.userbankmaps
                  where om.Userid != userid & om.Bank == bank & om.QueryType == querytype & om.UserPriority =="1"
                  select om).Count();

            return count;
        }

        public bool DeleteUser(String ID)
        {
            var lstsm = DataContext.usermasters.Where(p => p.Userid == ID).FirstOrDefault();

            if (lstsm != null)
            {
                DataContext.usermasters.Remove(lstsm);
                var result = DataContext.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        
        public void UpdatePlan(FTWebApi.Models.UserMaster objUserMaster)
        {
            userbankmap ub;
            
            try
            {
                usermaster ddUserMaster = (from c in DataContext.usermasters
                                                    where c.Userid == objUserMaster.UserId
                                                    select c).FirstOrDefault();

                ddUserMaster.UserName = objUserMaster.UserName;
                ddUserMaster.UserPassword = objUserMaster.UserPass;
                ddUserMaster.UserRole = objUserMaster.UserRole;

                foreach (FTWebApi.Models.userbankmap objuserbankmap in objUserMaster.lstuserbankmap)
                {
                    ub = (from om in DataContext.userbankmaps
                                          where om.Userid == objuserbankmap.UserId & om.Bank == objuserbankmap.Bank  & om.QueryType == objuserbankmap.QueryType
                                          select om).SingleOrDefault();
                    if (ub != null)
                    {
                        DataContext.userbankmaps.Remove(ub);
                        DataContext.SaveChanges();
                    }
                }

                foreach (FTWebApi.Models.userbankmap objuserbankmap in objUserMaster.lstuserbankmap)
                {
                    if (objuserbankmap.UserPriority != null && objuserbankmap.UserPriority != "")
                    {
                        ub = new userbankmap();
                        ub.Userid = objUserMaster.UserId;
                        ub.Bank = objuserbankmap.Bank;
                        ub.QueryType = objuserbankmap.QueryType;
                        ub.UserPriority = objuserbankmap.UserPriority;
                        DataContext.userbankmaps.Add(ub);
                    }
                }

                DataContext.SaveChanges();
            }
            catch (Exception ec)
            {
                throw ec;
            }
        }
        public void InsertPlan(FTWebApi.Models.UserMaster objUserMaster)
        {
            userbankmap ub;

            usermaster ddUserMaster = new usermaster();

            ddUserMaster.Userid = objUserMaster.UserId;
            ddUserMaster.UserName = objUserMaster.UserName;
            ddUserMaster.UserPassword = objUserMaster.UserPass;
            ddUserMaster.UserRole = objUserMaster.UserRole;
            
            DataContext.usermasters.Add(ddUserMaster);

            foreach (FTWebApi.Models.userbankmap objuserbankmap in objUserMaster.lstuserbankmap)
            {
                if (objuserbankmap.UserPriority != null && objuserbankmap.UserPriority!="")
                {
                    ub = new userbankmap();
                    ub.Userid = objUserMaster.UserId;
                    ub.Bank = objuserbankmap.Bank;
                    ub.QueryType = objuserbankmap.QueryType;
                    ub.UserPriority = objuserbankmap.UserPriority;
                    DataContext.userbankmaps.Add(ub);
                }
            }

            DataContext.SaveChanges();

        }
    }
}
