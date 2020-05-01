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
        public JsonResult AddEmployee(EmployeeModel emp)
        {
            return Json(Add(emp), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmployeeDetails()
        {
            var empdetails = GetEmployees();
            return Json(empdetails, JsonRequestBehavior.AllowGet);
        }

        public int Add(EmployeeModel emp)
        {
            if (!string.IsNullOrEmpty(emp.ToString()))
            {
                XDocument xmlDoc = XDocument.Load(Server.MapPath("~/XML/Employee.xml"));
                xmlDoc.Element("Employees").Add(new XElement("Employee", new XElement("Name", emp.Name), new XElement("Location", emp.Location)));
                xmlDoc.Save(Server.MapPath("~/XML/Employee.xml"));
                return 1;
            }
            else
            {
                return 0;
            }
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