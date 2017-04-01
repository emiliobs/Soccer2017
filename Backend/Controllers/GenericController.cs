using Backend.Models;
using System.Linq;
using System.Web.Mvc;

namespace Backend.Controllers
{
    public class GenericController : Controller
    {

        //conection with Db:
            DataContextLocal db = new DataContextLocal();

        #region Methods Ajaxs

        public JsonResult GetTeams(int lagueId)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var team = db.Teams.Where(t => t.LeagueId == lagueId);

            return Json(team);
        }


        #endregion


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