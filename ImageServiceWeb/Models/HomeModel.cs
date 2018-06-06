﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class HomeModel
    {
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
    }
}