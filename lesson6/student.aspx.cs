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
    public partial class student : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if save wasn't clicked AND we have a studentID in the URL
            if((!IsPostBack) && (Request.QueryString.Count > 0)){
                GetStudent();
            }
        }

        protected void GetStudent()
        {
            //populate form with existing student record
            Int32 StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

            //connect to db using entity framework
            using (comp2007Entities db = new comp2007Entities())
            {
                //populate a student instance with the studentID from the URL paramater
                Student s = (from objS in db.Students
                             where objS.StudentID == StudentID
                             select objS).FirstOrDefault();

                //map the student properties to the form controls if we found a record

                txtLastName.Text = s.LastName;
                txtFirstMidName.Text = s.FirstMidName;
                txtEnrollmentDate.Text = s.EnrollmentDate.ToShortDateString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //use EF to connect to SQL server
            using (comp2007Entities db = new comp2007Entities())
            {
                //use the student model to save the new record
                Student s = new Student();
                Int32 StudentID = 0;
                //check the query string for an id so we can determine add / update
                if(Request.QueryString["StudentID"] != null)
                {
                    //get the ID from the URL
                    StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

                    //get the current student from Entity Framework
                    s = (from objS in db.Students
                         where objS.StudentID == StudentID
                         select objS).FirstOrDefault();
                }
                s.LastName = txtLastName.Text;
                s.FirstMidName = txtFirstMidName.Text;
                s.EnrollmentDate = Convert.ToDateTime(txtEnrollmentDate.Text);

                //call add only if we have no student ID
                if(StudentID == 0)
                db.Students.Add(s);

                db.SaveChanges();
                //redirect to the updated students page
                Response.Redirect("students.aspx");
            }
        }
    }
}