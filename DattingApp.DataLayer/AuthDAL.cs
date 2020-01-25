using DattingApp.API.Common;
using DattingApp.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace DattingApp.DataLayer
{
    public class AuthDAL : DbConfig
    {
        public Task<List<UsersDto>> GetUserList()
        {
            List<UsersDto> listUsers = new List<UsersDto>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_getUserList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UsersDto objUserDto = new UsersDto();
                    objUserDto.id = Convert.ToInt32(rdr["id"]);
                    objUserDto.Username = Convert.ToString(rdr["Username"]);
                    objUserDto.PasswordHash = (byte[])rdr["PasswordHash"];
                    objUserDto.PasswordSalt = (byte[])rdr["PasswordSalt"];

                    listUsers.Add(objUserDto);
                }
                con.Close();
            }

            return Task.FromResult(listUsers);
        }

        public async Task<int> Register(UsersDto objUserDto)
        {
            int userIdOut = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_InsertUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Username", objUserDto.Username);
                cmd.Parameters.AddWithValue("@PasswordHash", objUserDto.PasswordHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", objUserDto.PasswordSalt);
                cmd.Parameters.Add("@userIdOut", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                userIdOut = Convert.ToInt32(cmd.Parameters["@userIdOut"].Value);
                con.Close();
            }

            return await Task.FromResult(userIdOut);
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // Convert a byte array to an Object
        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
