using DattingApp.API.Common;
using DattingApp.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DattingApp.DataLayer
{
    public class LikesDAL : DbConfig
    {
        public Task<List<LikesDto>> GetLike()
        {
            List<LikesDto> objLikeList = new List<LikesDto>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_getLikes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    LikesDto objLikeDto = new LikesDto();
                    objLikeDto.LikeId = Convert.ToInt32(rdr["LikeId"]);
                    objLikeDto.LikerId = Convert.ToInt32(rdr["LikerId"]);
                    objLikeDto.LikeeId = Convert.ToInt32(rdr["LikeeId"]);
                    objLikeList.Add(objLikeDto);
                }

                con.Close();
            }

            return Task.FromResult(objLikeList);
        }

        public Task<int> AddLikes(LikesDto objLikeDto)
        {
            int likeIdOut = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_AddLikes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LikerId", objLikeDto.LikerId);
                cmd.Parameters.AddWithValue("@LikeeId", objLikeDto.LikeeId);
                cmd.Parameters.Add("@likeIdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();

                likeIdOut = Convert.ToInt32(cmd.Parameters["@likeIdOut"].Value);
                con.Close();
            }
            return Task.FromResult(likeIdOut);
        }

        public Task<int> RemoveLikes(LikesDto objLikeDto)
        {
            int likeIdOut = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_RemoveLikes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LikerId", objLikeDto.LikerId);
                cmd.Parameters.AddWithValue("@LikeeId", objLikeDto.LikeeId);
                cmd.Parameters.Add("@likeIdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();

                likeIdOut = Convert.ToInt32(cmd.Parameters["@likeIdOut"].Value);
                con.Close();
            }
            return Task.FromResult(likeIdOut);
        }

        public Task<List<UsersDto>> GetLikerUserList(int userId)
        {
            List<UsersDto> objUserList = new List<UsersDto>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_getLikerUserList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UsersDto objUserDto = new UsersDto();
                    objUserDto.id = Convert.ToInt32(rdr["id"]);
                    objUserDto.Username = Convert.ToString(rdr["Username"]);
                    objUserDto.Gender = Convert.ToString(rdr["Gender"]);
                    objUserDto.DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]);
                    objUserDto.KnownAs = Convert.ToString(rdr["KnownAs"]);
                    objUserDto.Age = Convert.ToInt32(rdr["Age"]);
                    objUserDto.Created = Convert.ToDateTime(rdr["Created"]);
                    objUserDto.Created = Convert.ToDateTime(rdr["Created"]);
                    objUserDto.LastActive = Convert.ToDateTime(rdr["LastActive"]);
                    objUserDto.Introduction = Convert.ToString(rdr["Introduction"]);
                    objUserDto.lookingFor = Convert.ToString(rdr["lookingFor"]);
                    objUserDto.Interests = Convert.ToString(rdr["Interests"]);
                    objUserDto.City = Convert.ToString(rdr["City"]);
                    objUserDto.Country = Convert.ToString(rdr["Country"]);
                    objUserDto.PhotoUrl = Convert.ToString(rdr["MainUrl"]);

                    objUserList.Add(objUserDto);
                }

                con.Close();

                return Task.FromResult(objUserList);
            }
        }

        public Task<List<UsersDto>> GetLikeeUserList(int userId)
        {
            List<UsersDto> objUserList = new List<UsersDto>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_getLikeeUserList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UsersDto objUserDto = new UsersDto();
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

                    objUserList.Add(objUserDto);                    
                }

                con.Close();

                return Task.FromResult(objUserList);
            }
        }
    }
}