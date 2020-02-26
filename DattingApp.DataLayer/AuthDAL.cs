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
                    objUserDto.PhotoUrl = Convert.ToString(rdr["MainUrl"]);

                    listUsers.Add(objUserDto);
                }
                con.Close();
            }

            return Task.FromResult(listUsers);
        }

        public async Task<UsersDto> Register(UsersDto objUserDto)
        {            
            UsersDto objReturnUserDto = new UsersDto();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_InsertUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Username", objUserDto.Username);
                cmd.Parameters.AddWithValue("@PasswordHash", objUserDto.PasswordHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", objUserDto.PasswordSalt);
                cmd.Parameters.AddWithValue("@Gender", objUserDto.Gender);                
                cmd.Parameters.AddWithValue("@DateOfBirth", objUserDto.DateOfBirth);
                cmd.Parameters.AddWithValue("@KnownAs", objUserDto.KnownAs);
                cmd.Parameters.AddWithValue("@Created", objUserDto.Created);
                cmd.Parameters.AddWithValue("@LastActive", objUserDto.LastActive);
                cmd.Parameters.AddWithValue("@Interests", objUserDto.Interests);
                cmd.Parameters.AddWithValue("@lookingFor", objUserDto.lookingFor);
                cmd.Parameters.AddWithValue("@Introduction", objUserDto.Introduction);
                cmd.Parameters.AddWithValue("@City", objUserDto.City);
                cmd.Parameters.AddWithValue("@Country", objUserDto.Country);                                

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {                    
                    objUserDto.id = Convert.ToInt32(rdr["id"]);
                    objUserDto.Username = Convert.ToString(rdr["Username"]);
                    objUserDto.PasswordHash = (byte[])rdr["PasswordHash"];
                    objUserDto.PasswordSalt = (byte[])rdr["PasswordSalt"];                    
                    objUserDto.Gender = Convert.ToString(rdr["Gender"]);
                    objUserDto.DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]);
                    objUserDto.KnownAs = Convert.ToString(rdr["KnownAs"]);
                    objUserDto.Created = Convert.ToDateTime(rdr["Created"]);
                    objUserDto.LastActive = Convert.ToDateTime(rdr["LastActive"]);
                    objUserDto.Interests = Convert.ToString(rdr["Interests"]);
                    objUserDto.lookingFor = Convert.ToString(rdr["lookingFor"]);
                    objUserDto.Introduction = Convert.ToString(rdr["Introduction"]);
                    objUserDto.City = Convert.ToString(rdr["City"]);
                    objUserDto.Country = Convert.ToString(rdr["Country"]);
                }

                con.Close();
            }

            return await Task.FromResult(objReturnUserDto);
        }       
    }
}
