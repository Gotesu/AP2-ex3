using GUICommunication.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// home model get the connection instance and checks that server is active and gets our students info
    /// </summary>
    public class HomeModel
    {
        private IGUIClient client;
        public HomeModel() {
            client = GUIClient.Instance();
        }

        public List<Student> GetStudents()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/StudentsInfo.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var students = new List<Student>();
            students = (from rows in ds.Tables[0].AsEnumerable()
                        select new Student
                        {  
                            FirstName = rows[0].ToString(),
                            LastName = rows[1].ToString(),
                            ID = Convert.ToInt32(rows[2].ToString()), //Convert row to int
                        }).ToList();
            return students;
        }

        public bool connectionStatus()
        {
            return client.isConnected();
        }
    }
}