using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W1400.DataAccess.DTO;
using W1400.DataAccess.Factory;

namespace WWW.Controllers
{
    public class ProgramController : Controller
    {
        // GET: Program
        public ActionResult ProgramProcess()
        {
            int Total = 0;
            var list = AbstractDAOFactory.Instance().CreateProgramDAO().SP_Event_GetList_Web(0, "", -1, 1, 1000, out Total);            
            return View(list);
        }
        public ActionResult ProgramPass()
        {
            return View();
        }
        public ActionResult ProgramPassPartial(int Year)
        {
            int Total = 0;
            var list = AbstractDAOFactory.Instance().CreateProgramDAO().SP_Event_OldGetList_Web(0, "", -1,Year, 1, 1000, out Total);
            return PartialView(list);
        }
        // GET: Program/Details/5
        public ActionResult DetailPending(int id)
        {
            int Total = 0;
            var List =new  List<ProgramDetail>();
            var m_Program = new ProgramDetail();
             List =AbstractDAOFactory.Instance().CreateProgramDAO().SP_Event_GetDetail_Web(id, out Total);
            if (List != null && List.Count > 0)
            {
                m_Program=List[0];
            }
            else
            {
                m_Program = null;
            }
            ViewBag.List = List;
            ViewBag.Id = id;
            return View(m_Program);           
        }
        public ActionResult DetailPartial(int EventID, string FromDate, string ToDate)
        {
            DateTime fromDateTime = !string.IsNullOrWhiteSpace(FromDate) ? DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime toDateTime = !string.IsNullOrWhiteSpace(ToDate)? DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture): DateTime.Now;
            int Total = 0;
            var List = new List<ProgramReport>();
            List = AbstractDAOFactory.Instance().CreateProgramDAO().SP_Event_GetReport_Web(EventID, fromDateTime, toDateTime, out Total);
            if (List != null && List.Count > 0)
            {
                return PartialView(List);
            }
            else
            {
                return PartialView();
            }
            
        }
        public ActionResult DetailPass(int id)
        {
            int Total = 0;
            var List = new List<ProgramDetail>();
            var m_Program = new ProgramDetail();
            List = AbstractDAOFactory.Instance().CreateProgramDAO().SP_Event_GetDetail_Web(id, out Total);
            if (List != null && List.Count > 0)
            {
                m_Program = List[0];
            }
            else
            {
                m_Program = null;
            }
            ViewBag.List = List;
            ViewBag.Id = id;
            return View(m_Program);    
        }
        
    }
}
