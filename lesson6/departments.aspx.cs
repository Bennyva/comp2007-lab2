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
    public partial class departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading page for first time, populate the student grid
            if (!IsPostBack)
            {
                GetDepartments();
            }
        }

        protected void GetDepartments()
        {
            using (comp2007Entities db = new comp2007Entities())
            {
                //query the students table using EF and LINQ
                var Dept = from d in db.Departments
                               select d;

                //bind he result to the gridview
                grdDepts.DataSource = Dept.ToList();
                grdDepts.DataBind();
            }
        }

        protected void grdDepts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //store which row was clicked
            Int32 selectedRow = e.RowIndex;

            //get the selected studentID using the grid's data key collection
            Int32 DepartmentID = Convert.ToInt32(grdDepts.DataKeys[selectedRow].Values["DepartmentID"]);

            //use Entity Framework to remove the selected student from the db
            using (comp2007Entities db = new comp2007Entities())
            {
                Department d = (from objS in db.Departments
                             where objS.DepartmentID == DepartmentID
                             select objS).FirstOrDefault();

                //do the delete
                db.Departments.Remove(d);
                db.SaveChanges();
            }

            //refresh the grid
            GetDepartments();
        }
    }
}