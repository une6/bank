using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public interface IUserDAL
    {
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        User GetUserData(string loginName);
        User GetUserData(long accountNumber);
        bool LoginNameExists(string loginName);
        string ValidateLogin(User user);
    }

    public class UserDAL: IUserDAL
    {
        string connectionString = "";
        

        public UserDAL(IConfiguration configuration)
        {
            connectionString = configuration["DBConnectionString"];
        }


        //To View all user details    
        public IEnumerable<User> GetAllUsers()
        {
            var users = new List<User>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var user = new User();

                    user.ID = Convert.ToInt64(rdr["ID"]);
                    user.LoginName = rdr["LoginName"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.AccountNumber = Convert.ToInt64(rdr["AccountNumber"]);
                    user.Balance = Convert.ToDecimal(rdr["Balance"]);
                    user.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);

                    users.Add(user);
                }
                con.Close();
            }
            return users;
        }

        //To Add new user record    
        public void AddUser(User user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LoginName", user.LoginName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //Get the details of a particular user  
        public User GetUserData(string loginName)
        {
            var user = new User();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM tblUser WHERE LoginName= \'" + loginName + "\'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    user.ID = Convert.ToInt64(rdr["ID"]);
                    user.LoginName = rdr["LoginName"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.AccountNumber = Convert.ToInt64(rdr["AccountNumber"]);
                    user.Balance = Convert.ToDecimal(rdr["Balance"]);
                    user.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);
                }
            }
            return user;
        }

        //Get the details of a particular user  
        public User GetUserData(long accountNumber)
        {
            var user = new User();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM tblUser WHERE AccountNumber= " + accountNumber;
                SqlCommand cmd = new SqlCommand(sqlQuery, con);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    user.ID = Convert.ToInt64(rdr["ID"]);
                    user.LoginName = rdr["LoginName"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.AccountNumber = Convert.ToInt64(rdr["AccountNumber"]);
                    user.Balance = Convert.ToDecimal(rdr["Balance"]);
                    user.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);
                }
            }
            return user;
        }

        public bool LoginNameExists(string loginName)
        {
            bool ret = false;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM tblUser WHERE LoginName= \'" + loginName + "\'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ret = true;
                }
            }

            return ret;
        }

        public string ValidateLogin(User user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spValidateUserLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LoginName", user.LoginName);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                con.Open();
                string result = cmd.ExecuteScalar().ToString();
                con.Close();

                return result;
            }
        }
    }
}