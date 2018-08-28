using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public interface ITransactionDAL
    {
        IEnumerable<Transaction> GetAllTransactions(long accountNumber);
        void AddTransaction(Transaction trans);
        decimal GetBalance(long accountNumber);
    }

    public class TransactionDAL: ITransactionDAL
    {
        string connectionString = "";
        

        public TransactionDAL(IConfiguration configuration)
        {
            connectionString = configuration["DBConnectionString"];
        }


        //To View all user details    
        public IEnumerable<Transaction> GetAllTransactions(long accountNumber)
        {
            var lstTrans = new List<Transaction>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllTransactionsByAccountNumber", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var trans = new Transaction();

                    trans.ID = Convert.ToInt64(rdr["ID"]);
                    trans.Type = rdr["Type"].ToString();
                    trans.AccountNumber = Convert.ToInt64(rdr["AccountNumber"]);
                    trans.Amount = Convert.ToDecimal(rdr["Amount"]);
                    trans.Source = Convert.ToInt64(rdr["Source"]);
                    trans.Destination = Convert.ToInt64(rdr["Destination"]);
                    trans.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);

                    lstTrans.Add(trans);
                }
                con.Close();
            }
            return lstTrans;
        }

        //To Add new user record    
        public void AddTransaction(Transaction trans)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AccountNumber", trans.AccountNumber);
                cmd.Parameters.AddWithValue("@Type", trans.Type);
                cmd.Parameters.AddWithValue("@Amount", trans.Amount);
                cmd.Parameters.AddWithValue("@Source", trans.Source);
                cmd.Parameters.AddWithValue("@Destination", trans.Destination);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public decimal GetBalance(long accountNumber)
        {
            var balance = 0.0m;
            var trans = GetAllTransactions(accountNumber);

            foreach(var tran in trans)
            {
                balance += tran.Amount;
            }

            return balance;
        }
    }
}