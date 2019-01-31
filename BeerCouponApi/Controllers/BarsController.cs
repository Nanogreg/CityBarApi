using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ToolBox.DataAccess.Database;
using BeerCouponApi.App_Start;
using BeerCouponApi.Models;
using static BeerCouponApi.App_Start.DBConfig;
using System.Web.Http.Cors;

namespace BeerCouponApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BarsController : ApiController
    {
        Connection conn = new Connection(DBConfig.connectionString, ProviderDB.SqlClient);
        Command cmd;

        //Return a list of all Bars
        [Route("bcapi/bars/")]
        public IEnumerable<Bars> Get()
        {
            cmd = new Command("SELECT * FROM Bars");

            return conn.ExecuteReader(cmd, b => new Bars()
            {
                Id      = (int)checkDBNull(b["Id"]),
                Name    = (string)checkDBNull(b["Name"]),
                Address = (string)checkDBNull(b["Address"]),
                About   = (string)checkDBNull(b["About"]),
                TAGS    = (string)checkDBNull(b["TAGS"]), //TAGS are separated by - 
                BarPic  = (string)checkDBNull(b["BarPic"]),
                FkCityId= (int)checkDBNull(b["FkCityId"]) 
            });
        }

        //Return a list of Bars by CityId
        [HttpGet]
        [Route("bcapi/bars/bycity/{id}")]
        public IEnumerable<Bars> GetByCityId(int id)
        {
            cmd = new Command("SELECT * FROM Bars WHERE FkCityId = @id");
            cmd.AddParameter("id", id);

            return conn.ExecuteReader(cmd, b => new Bars()
            {
                Id = (int)checkDBNull(b["Id"]),
                Name = (string)checkDBNull(b["Name"]),
                Address = (string)checkDBNull(b["Address"]),
                About = (string)checkDBNull(b["About"]),
                TAGS = (string)checkDBNull(b["TAGS"]), //TAGS are separated by - 
                BarPic = (string)checkDBNull(b["BarPic"]),
                FkCityId = (int)checkDBNull(b["FkCityId"])
            });
        }

        //Return the Count of all Events in 1 Bar ( with Bar Id and Events.Type="E" for events ) 
        [HttpGet]
        [Route("bcapi/bars/{id}/countEventsE")]
        public int CountEventsE(int id)
        {
            cmd = new Command("SELECT COUNT (*) FROM Events WHERE FkBarId = @Id AND Type='E'");
            cmd.AddParameter("Id", id);

            return (int)conn.ExecuteReader(cmd, c => c[0]).SingleOrDefault();
        }

        //Return the Count of all Events in 1 Bar ( with Bar Id and Events.Type="C" for coupons ) 
        [HttpGet]
        [Route("bcapi/bars/{id}/countEventsC")]
        public int CountEventsC(int id)
        {
            cmd = new Command("SELECT COUNT (*) FROM Events WHERE FkBarId = @Id AND Type='C'");
            cmd.AddParameter("Id", id);

            return (int)conn.ExecuteReader(cmd, c => c[0]).SingleOrDefault();
        }

        //Return a list of Coupons by BarId
        [HttpGet]
        [Route("bcapi/bars/{id}/coupons")]
        public IEnumerable<Events> GetCouponsByBarId(int id)
        {
            cmd = new Command("SELECT * FROM Coupons WHERE FkBarId = @BarId");
            cmd.AddParameter("BarId", id);

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

        //Return ONE Bar
        [Route("bcapi/bars/{id}")]
        public Bars Get(int id)
        {
            cmd = new Command("SELECT * FROM Bars WHERE Id = @Id");
            cmd.AddParameter("Id", id);

            return conn.ExecuteReader(cmd, b => new Bars()
            {
                Id      = (int)checkDBNull(b["Id"]),
                Name    = (string)checkDBNull(b["Name"]),
                Address = (string)checkDBNull(b["Address"]),
                About   = (string)checkDBNull(b["About"]),
                TAGS    = (string)checkDBNull(b["TAGS"]), //TAGS are separated by - 
                BarPic  = (string)checkDBNull(b["BarPic"]),
                FkCityId= (int)checkDBNull(b["FkCityId"])  

            }).SingleOrDefault();
        }

        //Add ONE Bar
        [Route("bcapi/bars/")]
        public int Post(Bars b)
        {
            cmd = new Command("INSERT INTO Bars VALUES ( @Name, @Address, @About, @TAGS, @BarPic, @FkCityId)");
            cmd.AddParameter("Name", b.Name);
            cmd.AddParameter("Address", b.Address);
            cmd.AddParameter("About", b.About);
            cmd.AddParameter("TAGS", b.TAGS);
            cmd.AddParameter("BarPic", b.BarPic);
            cmd.AddParameter("FkCityId", b.FkCityId);

            return conn.ExecuteNonQuery(cmd);
        }

        //Delete ONE bar with Id = ID
        [Route("bcapi/bars/{id}")]
        public void Delete(int id)
        {
            Command cmd = new Command("DELETE FROM Bars WHERE Id = @ID");
            cmd.AddParameter("ID", id);
            conn.ExecuteNonQuery(cmd);
        }

        //Modify ONE Bar with Id = Id
        [Route("bcapi/bars/{id}")]
        public int Put(int id, Bars b)
        {
            cmd = new Command(@"UPDATE Bars SET Name = @Name, Address = @Address, About = @About, TAGS = @TAGS, BarPic = @BarPic, FkCityId = @FkCityId WHERE Id = @Id");
            cmd.AddParameter("Id", id);
            cmd.AddParameter("Name", b.Name);
            cmd.AddParameter("Address", b.Address);
            cmd.AddParameter("About", b.About);
            cmd.AddParameter("TAGS", b.TAGS);
            cmd.AddParameter("BarPic", b.BarPic);
            cmd.AddParameter("FkCityId", b.FkCityId);

            return conn.ExecuteNonQuery(cmd);
        }
    }
}
