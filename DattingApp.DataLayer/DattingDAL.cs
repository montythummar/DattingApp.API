using DattingApp.API.Common;
using DattingApp.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DattingApp.DataLayer
{
    public class DattingDAL : DbConfig
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
                    PhotoDto objPhotosDto = new PhotoDto();
                    List<PhotoDto> objPhotosList = new List<PhotoDto>();
                    objUserDto.id = Convert.ToInt32(rdr["id"]);
                    objUserDto.Username = Convert.ToString(rdr["Username"]);
                    objUserDto.Gender = Convert.ToString(rdr["Gender"]);
                    objUserDto.DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]);
                    objUserDto.Age = Convert.ToInt32(rdr["Age"]);
                    objUserDto.KnownAs = Convert.ToString(rdr["KnownAs"]);
                    objUserDto.Created = Convert.ToDateTime(rdr["Created"]);
                    objUserDto.LastActive = Convert.ToDateTime(rdr["LastActive"]);
                    objUserDto.Introduction = Convert.ToString(rdr["Introduction"]);
                    objUserDto.lookingFor = Convert.ToString(rdr["lookingFor"]);
                    objUserDto.Interests = Convert.ToString(rdr["Interests"]);
                    objUserDto.City = Convert.ToString(rdr["City"]);
                    objUserDto.Country = Convert.ToString(rdr["Country"]);
                    objUserDto.PhotoUrl = Convert.ToString(rdr["Url"]);
                    objPhotosDto.Id = Convert.ToInt32(rdr["Id"]);
                    objPhotosDto.url = Convert.ToString(rdr["Url"]);
                    objPhotosDto.IsMain = Convert.ToBoolean(rdr["IsMain"]);
                    objPhotosDto.Description = Convert.ToString(rdr["Description"]);
                    objPhotosDto.AddDate = Convert.ToDateTime(rdr["AddDate"]);
                    objPhotosList.Add(objPhotosDto);
                    objUserDto.Photos = objPhotosList;
                    listUsers.Add(objUserDto);
                }
                con.Close();
            }

            return Task.FromResult(listUsers);
        }

        public Task<int> UpdateUser(int id, UpdateUserDto objUser)
        {
            List<UsersDto> listUsers = new List<UsersDto>();
            int userIdOut = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Introduction", objUser.Introduction);
                cmd.Parameters.AddWithValue("@lookingFor", objUser.lookingFor);
                cmd.Parameters.AddWithValue("@interests", objUser.interests);
                cmd.Parameters.AddWithValue("@City", objUser.City);
                cmd.Parameters.AddWithValue("@Country", objUser.Country);
                cmd.Parameters.Add("@userIdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();

                userIdOut = Convert.ToInt32(cmd.Parameters["@userIdOut"].Value);            
                con.Close();
            }
            return Task.FromResult(userIdOut);
        }
    }
}
