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
    public class UsersController : ApiController
    {
        Connection conn = new Connection(DBConfig.connectionString, ProviderDB.SqlClient);
        Command cmd;

        //Return a list of all Users
        [Route("bcapi/users/")]
        public IEnumerable<Users> Get()
        {
            cmd = new Command("SELECT * FROM Users");

            return conn.ExecuteReader(cmd, u => new Users()
            {
                Id          = (int)checkDBNull(u["Id"]),
                FirstName   = (string)checkDBNull(u["FirstName"]),
                Email       = (string)checkDBNull(u["Email"]),
                Password    = (string)checkDBNull(u["Password"]),
                FkBarId     = (int)checkDBNull(u["FkBarId"])
            });
        }

        //Return ONE User
        [Route("bcapi/users/{id}")]
        public Users Get(int id)
        {
            cmd = new Command("SELECT * FROM Users WHERE Id = @Id");
            cmd.AddParameter("Id", id);

            return conn.ExecuteReader(cmd, u => new Users()
            {
                Id = (int)checkDBNull(u["Id"]),
                FirstName = (string)checkDBNull(u["FirstName"]),
                Email = (string)checkDBNull(u["Email"]),
                Password = (string)checkDBNull(u["Password"]),
                FkBarId = (int)checkDBNull(u["FkBarId"])

            }).SingleOrDefault();
        }

        //Add ONE User By HTTP POST
        [Route("bcapi/users/")]
        public int Post(Users u)
        {
            cmd = new Command("INSERT INTO Users VALUES ( @FirstName, @Email, @Password, @FkBarId)");
            cmd.AddParameter("FirstName", u.FirstName);
            cmd.AddParameter("Email", u.Email);
            cmd.AddParameter("Password", u.Email);
            cmd.AddParameter("FkBarId", u.FkBarId);

            return conn.ExecuteNonQuery(cmd);
        }

        //Delete ONE User By HTTP DELETE
        [Route("bcapi/users/{id}")]
        public void Delete(int id)
        {
            Command cmd = new Command("DELETE FROM Users WHERE Id = @Id");
            cmd.AddParameter("Id", id);
            conn.ExecuteNonQuery(cmd);
        }

        //Modify ONE User By HTTP PUT
        [Route("bcapi/users/{id}")]
        public int Put(int id, Users u)
        {
            cmd = new Command(@"UPDATE Users SET FirstName = @FirstName, Email = @Email, Password = @Password, FkBarId = @FkBarId, WHERE Id = @Id");
            cmd.AddParameter("Id", id);
            cmd.AddParameter("FirstName", u.FirstName);
            cmd.AddParameter("Email", u.Email);
            cmd.AddParameter("Password", u.Password);
            cmd.AddParameter("FkBarId", u.FkBarId);

            return conn.ExecuteNonQuery(cmd);
        }
    }
}
