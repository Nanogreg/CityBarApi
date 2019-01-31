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
    public class CitiesController : ApiController
    {
        Connection conn = new Connection(DBConfig.connectionString, ProviderDB.SqlClient);
        Command cmd;

        //Return a list of all Cities --- ORDER By PostCode, maybe I'll remove that
        [Route("bcapi/cities/")]
        public IEnumerable<Cities> Get()
        {
            cmd = new Command("SELECT * FROM Cities ORDER BY PostCode");

            return conn.ExecuteReader(cmd, c => new Cities()
            {
                Id          = (int)checkDBNull(c["Id"]),
                PostCode    = (int)checkDBNull(c["PostCode"]),
                Name        = (string)checkDBNull(c["Name"]),
                About       = (string)checkDBNull(c["About"]),
                BannerPic   = (string)checkDBNull(c["BannerPic"])
            });
        }

        //Return ONE City
        [Route("bcapi/cities/{id}")]
        public Cities Get(int id)
        {
            cmd = new Command("SELECT * FROM Cities WHERE Id = @Id");
            cmd.AddParameter("Id", id);

            return conn.ExecuteReader(cmd, c => new Cities()
            {
                Id          = (int)checkDBNull(c["Id"]),
                PostCode    = (int)checkDBNull(c["PostCode"]),
                Name        = (string)checkDBNull(c["Name"]),
                About       = (string)checkDBNull(c["About"]),
                BannerPic   = (string)checkDBNull(c["BannerPic"])

            }).SingleOrDefault();
        }

        //Return the Count of all bars in 1 City ( with City Id - FkCityId ) 
        [HttpGet]
        [Route("bcapi/cities/{id}/countBars")]
        public int CountBars(int id)
        {
            cmd = new Command("SELECT COUNT (*) FROM Bars WHERE FkCityId = @Id");
            cmd.AddParameter("Id", id);

            return (int)conn.ExecuteReader(cmd, c => c[0]).SingleOrDefault();
        }

        //Count all Events in 1 City ( with City Id - FkCityId and Events.Type="E" ) 
        [HttpGet]
        [Route("bcapi/cities/{id}/countEvents")]
        public int CountEvents(int id)
        {
            cmd = new Command("SELECT COUNT (*) FROM Events WHERE Type='E' AND FkBarId in (SELECT Id from Bars WHERE FkCityId = @Id)");
            cmd.AddParameter("Id", id);

            return (int)conn.ExecuteReader(cmd, c => c[0]).SingleOrDefault();
        }

        //Count of all Coupons in 1 City ( with City Id - FkCityId and Events.Type="C" ) 
        [HttpGet]
        [Route("bcapi/cities/{id}/countCoupons")]
        public int CountCoupons(int id)
        {
            cmd = new Command("SELECT COUNT (*) FROM Events WHERE Type='C' AND FkBarId in (SELECT Id from Bars WHERE FkCityId = @Id)");
            cmd.AddParameter("Id", id);

            return (int)conn.ExecuteReader(cmd, c => c[0]).SingleOrDefault();
        }

        //Add ONE City
        [Route("bcapi/cities/")]
        public int Post(Cities c)
        {
            cmd = new Command("INSERT INTO Cities (PostCode,Name,About,BannerPic)  VALUES ( @PostCode, @Name, @About, @BannerPic)");
            cmd.AddParameter("PostCode", c.PostCode);
            cmd.AddParameter("Name", c.Name);
            cmd.AddParameter("About", c.About);
            cmd.AddParameter("BannerPic", c.BannerPic);

            return conn.ExecuteNonQuery(cmd);
        }

        //Delete ONE City with id
        [Route("bcapi/cities/{id}")]
        public void Delete(int id)
        {
            Command cmd = new Command("DELETE FROM Cities WHERE Id = @Id");
            cmd.AddParameter("Id", id);

            conn.ExecuteNonQuery(cmd);
        }

        //Modify ONE City with id
        [Route("bcapi/cities/{id}")]
        public int Put(int id, Cities c)
        {
            cmd = new Command(@"UPDATE Cities SET PostCode = @PostCode, Name = @Name, About = @About, BannerPic = @BannerPic, WHERE Id = @Id");
            cmd.AddParameter("Id", id);
            cmd.AddParameter("PostCode", c.PostCode);
            cmd.AddParameter("Name", c.Name);
            cmd.AddParameter("About", c.About);
            cmd.AddParameter("BannerPic", c.BannerPic);

            return conn.ExecuteNonQuery(cmd);
        }
    }
}