using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ToolBox.DataAccess.Database;
using ConnectDB.Models;
using System.Data.Common;

namespace ConnectDB
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ConnectionString = @"Server=localhost;Database=ADO_Exercices;Integrated Security=SSPI";
                    
           
            Connection c = new Connection(ConnectionString, ProviderDB.SqlClient);
                              

            //Command cmd4 = new Command("SELECT LastName, FirstName, BirthDate FROM V_Student");
            //foreach(DataRow row in c.GetDataTable(cmd4).Rows)
            //{
            //    Console.WriteLine($"{row[0]} - {row[1]}");
            //}
            
            Command cmd5 = new Command("SELECT AVG(CAST(YearResult AS Float)) FROM V_Student");
            double avg = (double)c.ExecuteScalar(cmd5);
            Console.WriteLine("Moyenne des Students inscrits : {0}", avg.ToString(".##"));

            Command cmd10 = new Command("SELECT SectionId, SectionName, AVG(CAST(YearResult AS FLOAT)) AS Moyenne FROM Student, Section WHERE SectionId = Section.Id GROUP BY SectionId, SectionName");
            foreach(var row in c.ExecuteReader(cmd10, x => new {
                Section = x["SectionId"],
                SectionName = x["SectionName"],
                Moyenne = x["Moyenne"]
            }))
            {
                Console.WriteLine($"{row.Section}\t{row.SectionName.ToString(), -25}\t{Convert.ToDouble(row.Moyenne).ToString(".##")}");
            }

            //Command cmd6 = new Command("SELECT YearResult FROM V_Student WHERE LastName LIKE @Nom");
            //cmd6.AddParameter("Nom", "Marceau");
            //int result6 = (int)c.ExecuteScalar(cmd6);
            //Console.WriteLine("Résultat de Marceau : {0}", result6);



            //Command cmd3 = new Command("delete from Student where Id = @Id");
            //cmd3.AddParameter("Id", 13);
            //int result3 = c.ExecuteNonQuery(cmd3);
            //Console.WriteLine("Student [{0}] supprimé... (désactivé)", result3);

            // Exemple d'un appel à une procédure stockée

            //Command cmd2 = new Command("DeleteStudent", true);
            //cmd2.AddParameter("Id", 14);
            //int result2 = c.ExecuteNonQuery(cmd2);
            //Console.WriteLine("Student [{0}] supprimé... (désactivé)", result2);

            //  Exemple d'un appel à une procédure stockée avec un retour de paramètre (output)

            //Command cmd1 = new Command("DeleteStudent", true, ParameterDirection.Output);
            //cmd1.AddParameter("Id", 14);
            //int result = c.ExecuteNonQuery(cmd1);
            //Console.WriteLine("student [{0}] supprimé... (désactivé)", result);

            //Command cmd7 = new Command("AddSection", true);
            //cmd7.AddParameter("SectionId", 1500);
            //cmd7.AddParameter("SectionName", "Recherches & Développement");
            //int result7 = c.ExecuteNonQuery(cmd7);

            //Command cmd8 = new Command("AddStudent", true);
            //cmd8.AddParameter("FirstName", "Greg");
            //cmd8.AddParameter("LastName", "Ceuleers");
            //cmd8.AddParameter("BirthDate", new DateTime(1980, 6, 26));
            //cmd8.AddParameter("YearResult", 14);
            //cmd8.AddParameter("SectionId", 1500);
            //int result8 = c.ExecuteNonQuery(cmd8);

            //Command cmd9 = new Command("UpdateStudent", true);
            //cmd9.AddParameter("Id", 1);
            ////cmd9.AddParameter("YearResult", 20);
            //cmd9.AddParameter("SectionId", 1020);
            //int result9 = c.ExecuteNonQuery(cmd9);

            // Afficher toutes les informations des Students

            Command cmd = new Command("select * from V_Student;");

            foreach (Student st in c.ExecuteReader(cmd, dr => new Student()
            {
                Id = (int)dr["Id"],
                FirstName = (string)dr["FirstName"],
                LastName = (string)dr["LastName"],
                BirthDate = (DateTime)dr["BirthDate"],
                YearResult = (int)dr["YearResult"],
                SectionId = (int)dr["SectionId"],
                Active = (bool)dr["Active"]
            }))
            {
                Console.WriteLine(st.ToString());
            }

            // Type Anonyme >> utilisation de var 

            //foreach (var st in c.ExecuteReader(cmd, dr => new
            //{
            //    Id = (int)dr["Id"],
            //    FirstName = (string)dr["FirstName"],
            //    LastName = (string)dr["LastName"],
            //    BirthDate = (DateTime)dr["BirthDate"],
            //    YearResult = (int)dr["YearResult"],
            //    SectionId = (int)dr["SectionId"],
            //    Active = (bool)dr["Active"]
            //}))
            //{
            //    Console.WriteLine(st.ToString());
            //}
                      

            Console.ReadLine();
        }
    }
}
