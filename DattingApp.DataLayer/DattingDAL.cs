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
            List<UsersDto> objUserList = new List<UsersDto>();
            List<PhotoDto> objPhotoList = new List<PhotoDto>();

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
                    objUserDto.id = Convert.ToInt32(rdr["id"]);
                    objUserDto.Username = Convert.ToString(rdr["Username"]);
                    objUserDto.Gender = Convert.ToString(rdr["Gender"]);
                    objUserDto.DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]);                    
                    objUserDto.KnownAs = Convert.ToString(rdr["KnownAs"]);
                    objUserDto.Age = Convert.ToInt32(rdr["Age"]);
                    objUserDto.Created = Convert.ToDateTime(rdr["Created"]);
                    objUserDto.LastActive = Convert.ToDateTime(rdr["LastActive"]);
                    objUserDto.Introduction = Convert.ToString(rdr["Introduction"]);
                    objUserDto.lookingFor = Convert.ToString(rdr["lookingFor"]);
                    objUserDto.Interests = Convert.ToString(rdr["Interests"]);
                    objUserDto.City = Convert.ToString(rdr["City"]);
                    objUserDto.Country = Convert.ToString(rdr["Country"]);
                    objUserDto.PhotoUrl = Convert.ToString(rdr["MainUrl"]);

                    if (!objUserList.Exists(x => x.id == objUserDto.id))
                    {
                        objUserList.Add(objUserDto);
                    }

                    objPhotosDto.Id = Convert.ToInt32(rdr["Id"]);
                    objPhotosDto.FkUserId = Convert.ToInt32(rdr["FkUserId"]);
                    objPhotosDto.url = Convert.ToString(rdr["Url"]);
                    objPhotosDto.IsMain = Convert.ToBoolean(rdr["IsMain"]);
                    objPhotosDto.Description = Convert.ToString(rdr["Description"]);
                    objPhotosDto.AddDate = Convert.ToDateTime(rdr["AddDate"]);
                    objPhotoList.Add(objPhotosDto);

                }

                foreach (var user in objUserList)
                {
                    List<PhotoDto> objTempPhotoList = new List<PhotoDto>();
                    foreach (var photo in objPhotoList)
                    {
                        if (user.id == photo.FkUserId)
                        {
                            PhotoDto objPhoto = new PhotoDto();

                            objPhoto.Id = photo.Id;
                            objPhoto.url = photo.url;
                            objPhoto.IsMain = photo.IsMain;
                            objPhoto.Description = photo.Description;
                            objPhoto.AddDate = photo.AddDate;

                            objTempPhotoList.Add(objPhoto);
                        }
                    }
                    user.Photos = objTempPhotoList;
                }
                con.Close();
            }

            return Task.FromResult(objUserList);
        }


        public Task<DataTable> GetUserById(int UserId)
        {
            List<UsersDto> listUsers = new List<UsersDto>();
            DataTable dtUser = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_getUserById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@UserId", UserId);
                var dataReader = cmd.ExecuteReader();
                dtUser.Load(dataReader);
                con.Close();
            }

            return Task.FromResult(dtUser);
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

        public Task<int> UpdateUserLogActivity(int id)
        {            
            int userIdOut = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateUserLogActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);                ;               
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
