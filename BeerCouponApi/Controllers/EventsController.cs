using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ToolBox.DataAccess.Database;
using BeerCouponApi.App_Start;
using BeerCouponApi.Models;
using static BeerCouponApi.App_Start.DBConfig;
using System.Web.Http.Cors;

//Coupons are now Events in most of client app
namespace BeerCouponApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventsController : ApiController
    {
        Connection conn = new Connection(DBConfig.connectionString, ProviderDB.SqlClient);
        Command cmd;

        //Coupon p is used because promo was the old name,have to change it to coupon c ? 

        //Return a list of all Events
        [Route("bcapi/events/")]
        public IEnumerable<Events> Get()
        {
            cmd = new Command("SELECT * FROM Events");

            return conn.ExecuteReader(cmd, p => new Events()
            {
                Id          = (int)checkDBNull(p["Id"]),
                Description = (string)checkDBNull(p["Description"]),
                BarCode     = (string)checkDBNull(p["BarCode"]),
                StartDate   = (DateTime)checkDBNull(p["StartDate"]),
                EndDate     = (DateTime)checkDBNull(p["EndDate"]),
                FkBarId     = (int)checkDBNull(p["FkBarId"])
            });
        }

        //Return a list of all Events in a specific PostCode ( by City ) 
        [HttpGet]
        [Route("bcapi/events/byPostCode/{postCode}")]
        public IEnumerable<Events> GetEventsByPostCode(int postCode)
        {
            cmd = new Command("SELECT * FROM Events WHERE FkBarId IN (SELECT Id FROM Bars WHERE FkCityId IN (SELECT Id FROM Cities WHERE PostCode = @PostCode))");
            cmd.AddParameter("PostCode", postCode);

            return conn.ExecuteReader(cmd, p => new Events()
            {
                Id          = (int)checkDBNull(p["Id"]),
                Type        = (string)checkDBNull(p["Type"]),
                Description = (string)checkDBNull(p["Description"]),
                BarCode     = (string)checkDBNull(p["BarCode"]),
                StartDate   = (DateTime)checkDBNull(p["StartDate"]),
                EndDate     = (DateTime)checkDBNull(p["EndDate"]),
                FkBarId     = (int)checkDBNull(p["FkBarId"])
            });
        }

        //Return a list of all Events by City Id
        [HttpGet]
        [Route("bcapi/events/{type}/bycityid/{idCity}")]
        public IEnumerable<Events> GetEventsByCityId(string type, int idCity)
        {
            cmd = new Command("SELECT * FROM Events WHERE Type = @type AND FkBarId IN (SELECT Id FROM Bars WHERE FkCityId = @idCity)");
            cmd.AddParameter("type", type);
            cmd.AddParameter("idCity", idCity);

            return conn.ExecuteReader(cmd, p => new Events()
            {
                Id = (int)checkDBNull(p["Id"]),
                Type = (string)checkDBNull(p["Type"]),
                Description = (string)checkDBNull(p["Description"]),
                BarCode = (string)checkDBNull(p["BarCode"]),
                StartDate = (DateTime)checkDBNull(p["StartDate"]),
                EndDate = (DateTime)checkDBNull(p["EndDate"]),
                FkBarId = (int)checkDBNull(p["FkBarId"])
            });
        }

        //Return a list of Events by Bar Id
        [HttpGet]
        [Route("bcapi/events/{type}/bybarid/{idBar}")]
        public IEnumerable<Events> GetCouponsByBarId(string type, int idBar)
        {
            cmd = new Command("SELECT * FROM Events WHERE FkBarId = @barId AND Type = @type");
            cmd.AddParameter("type", type);
            cmd.AddParameter("barId", idBar);

            return conn.ExecuteReader(cmd, p => new Events()
            {
                Id = (int)checkDBNull(p["Id"]),
                Type = (string)checkDBNull(p["Type"]),
                Description = (string)checkDBNull(p["Description"]),
                BarCode = (string)checkDBNull(p["BarCode"]),
                StartDate = (DateTime)checkDBNull(p["StartDate"]),
                EndDate = (DateTime)checkDBNull(p["EndDate"]),
                FkBarId = (int)checkDBNull(p["FkBarId"])
            });
        }

        //Return ONE Event by id
        [Route("bcapi/events/{id}")]
        public Events Get(int id)
        {
            cmd = new Command("SELECT * FROM Events WHERE Id = @Id");
            cmd.AddParameter("Id", id);

            return conn.ExecuteReader(cmd, p => new Events()
            {
                Id          = (int)checkDBNull(p["Id"]),
                Type        = (string)checkDBNull(p["Type"]),
                Description = (string)checkDBNull(p["Description"]),
                BarCode     = (string)checkDBNull(p["BarCode"]),
                StartDate   = (DateTime)checkDBNull(p["StartDate"]),
                EndDate     = (DateTime)checkDBNull(p["EndDate"]),
                FkBarId     = (int)checkDBNull(p["FkBarId"])

            }).SingleOrDefault();
        }

        //Add Event by HTTP POST
        [Route("bcapi/events/")]
        public int Post(Events p)
        {
            cmd = new Command("INSERT INTO Events VALUES ( @Type, @Description, @BarCode, @StartDate, @EndDate, @FkBarId)");
            cmd.AddParameter("Type", p.Description);
            cmd.AddParameter("Description", p.Description);
            cmd.AddParameter("BarCode", p.BarCode);
            cmd.AddParameter("StartDate", p.StartDate);
            cmd.AddParameter("EndDate", p.EndDate);
            cmd.AddParameter("FkBarId", p.FkBarId);

            return conn.ExecuteNonQuery(cmd);
        }

        //Delete one Event by HTTP DELETE
        [Route("bcapi/events/{id}")]
        public void Delete(int id)
        {
            Command cmd = new Command("DELETE FROM Events WHERE Id = @Id");
            cmd.AddParameter("Id", id);
            conn.ExecuteNonQuery(cmd);
        }

        //Modify Event by HTTP PUT
        [Route("bcapi/events/{id}")]
        public int Put(int id, Events p)
        {
            cmd = new Command(@"UPDATE Events SET Type= @Type, Description = @Description, BarCode = @BarCode, StartDate = @StartDate, EndDate = @EndDate, FkBarId = @FkBarId, WHERE Id = @Id");
            cmd.AddParameter("Id", id);
            cmd.AddParameter("Type", p.Type);
            cmd.AddParameter("Description", p.Description);
            cmd.AddParameter("BarCode", p.BarCode);
            cmd.AddParameter("StartDate", p.StartDate);
            cmd.AddParameter("EndDate", p.EndDate);
            cmd.AddParameter("FkBarId", p.FkBarId);

            return conn.ExecuteNonQuery(cmd);
        }
    }
}
