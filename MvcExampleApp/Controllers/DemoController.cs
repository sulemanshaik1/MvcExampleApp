using MvcExampleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MvcExampleApp.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool AddEmployee(EmployeeModel emp)
        {
            XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/Employee.xml"));
            xmlDoc.Element("Employees").Add(new XElement("Employee", new XElement("Name", emp.Name), new XElement("Location", emp.Location)));
            xmlDoc.Save(Server.MapPath("~/XML/Employee.xml"));
            return true;
        }

        //[HttpGet]
        public ActionResult GetEmployeeDetails()
        {
            var empdetails = GetEmployees();
            return Json(empdetails, JsonRequestBehavior.AllowGet);
        }

        public List<EmployeeModel> GetEmployees()
        {
            List<EmployeeModel> lstEmployees = new List<EmployeeModel>();
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("~/XML/Employee.xml"));
            DataView dvEmployees;
            if (ds.Tables.Count == 0)
            {
                return lstEmployees;
            }
            else
            {
                dvEmployees = ds.Tables[0].DefaultView;
                foreach (DataRowView dr in dvEmployees)
                {
                    EmployeeModel model = new EmployeeModel();
                    model.Name = Convert.ToString(dr[0]);
                    model.Location = Convert.ToString(dr[1]);
                    lstEmployees.Add(model);
                }

                return lstEmployees;
            }
        }
    }
}