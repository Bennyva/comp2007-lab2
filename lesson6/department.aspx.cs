using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//reference the ef models
using lesson6.Models;
using System.Web.ModelBinding;

namespace lesson6
{
    public partial class department : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if save wasn't clicked AND we have a studentID in the URL
            if((!IsPostBack) && (Request.QueryString.Count > 0)){
                GetDepartment();
            }
        }

        protected void GetDepartment()
        {
            //populate form with existing student record
            Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

            //connect to db using entity framework
            using (comp2007Entities db = new comp2007Entities())
            {
                //populate a student instance with the studentID from the URL paramater
                Department d = (from objS in db.Departments
                             where objS.DepartmentID == DepartmentID
                             select objS).FirstOrDefault();

                //map the student properties to the form controls if we found a record

                txtDeptName.Text = d.Name;
                txtBudget.Text = d.Budget.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //use EF to connect to SQL server
            using (comp2007Entities db = new comp2007Entities())
            {
                //use the student model to save the new record
                Department d = new Department();
                Int32 DepartmentID = 0;
                //check the query string for an id so we can determine add / update
                if(Request.QueryString["DepartmentID"] != null)
                {
                    //get the ID from the URL
                    DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                    //get the current student from Entity Framework
                    d = (from objS in db.Departments
                         where objS.DepartmentID == DepartmentID
                         select objS).FirstOrDefault();
                }
                d.Name = txtDeptName.Text;
                d.Budget = Convert.ToDecimal(txtBudget.Text);

                //call add only if we have no student ID
                if (DepartmentID == 0)
                db.Departments.Add(d);

                db.SaveChanges();
                //redirect to the updated students page
                Response.Redirect("departments.aspx");
            }
        
        }
    }
}