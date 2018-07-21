using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Neudesic.AdoAssignment.Model;
using System.Data.SqlClient;
using System.Web;

namespace Neudesic.AdoAssignment.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private string connectionString;
        public StudentController(IConfiguration configuration)
        {
            connectionString = "Data Source = localhost; Initial Catalog = Student;Integrated Security = SSPI";
        }
        public List<StudentModel> StudentModels = new List<StudentModel>();
        [HttpGet]
        public ActionResult GetStudentData()
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("studentDetailsProcedure", myConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                myConnection.Open();
                command.Connection = myConnection;
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentModel studentModelInstance = new StudentModel();
                        studentModelInstance.Id = Convert.ToInt32((reader["Id"]));
                        studentModelInstance.Name = reader["Name"].ToString();
                        studentModelInstance.Age = Convert.ToInt32((reader["Id"]));
                        studentModelInstance.Remarks = reader["Remarks"].ToString();
                        StudentModels.Add(studentModelInstance);
                    }
                }
                myConnection.Close();   
            }
            return Ok(StudentModels);   
        }
        [HttpPost]
        public IActionResult Insert([FromBody]StudentModel student)
        {
            string query = "insert into  StudentDetails values(@id,@name,@age,@remarks)";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@Id", student.Id);
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Age", student.Age );
            command.Parameters.AddWithValue("@Remarks", student.Remarks);
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                command.Connection = myConnection;
                command.ExecuteNonQuery();
            }
            return Ok("One row inserted");
        }
        [HttpPut]
        public ActionResult Update([FromBody]StudentModel student)
        {
            string query = "UPDATE StudentDetails SET Remarks = @mobileNumber WHERE ID=@id";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@name", student.Name));
            command.Parameters.Add(new SqlParameter("@id", student.Id));
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                command.Connection = myConnection;
                command.ExecuteNonQuery();
            }
            return Ok("One row updated");
        }
        [HttpDelete]
        public ActionResult Delete([FromBody]StudentModel student)
        {
            string query = "DELETE from StudentDetails  WHERE ID=@id";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@id", student.Id));
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                myConnection.Open();
                command.Connection = myConnection;
                command.ExecuteNonQuery();
            }
            return Ok("One row deleted");
        }
    }
}