using DattingApp.API.Common;
using DattingAppDataTransferObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DattingAppDataLayer
{
    public class TestDAL: DbConfig
    {        
        public Task<List<TestDto>> GetTestData()
        {
            List<TestDto> lstTest = new List<TestDto>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_Test", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TestDto objTest = new TestDto();
                    objTest.id = Convert.ToInt32(rdr["id"]);
                    objTest.first_name = rdr["first_name"].ToString();
                    objTest.last_name = rdr["last_name"].ToString();

                    lstTest.Add(objTest);
                }
                con.Close();
            }
            return Task.FromResult(lstTest);
        }
    }
}
