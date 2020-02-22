using DattingApp.API.Common;
using DattingApp.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DattingApp.DataLayer
{
    public class PhotosDAL : DbConfig
    {
        public async Task<PhotoDto> InsertPhoto(PhotoDto objPhotoDto)
        {
            PhotoDto objUserPhoto = new PhotoDto();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_InsertPhoto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@FkUserId", objPhotoDto.FkUserId);
                cmd.Parameters.AddWithValue("@url", objPhotoDto.url);
                cmd.Parameters.AddWithValue("@Description", objPhotoDto.Description == null ? "" : objPhotoDto.Description);
                cmd.Parameters.AddWithValue("@AddDate", objPhotoDto.AddDate);
                cmd.Parameters.AddWithValue("@IsMain", objPhotoDto.IsMain);
                cmd.Parameters.AddWithValue("@PublicId", objPhotoDto.PublicId);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {                    
                    objUserPhoto.Id = Convert.ToInt32(rdr["Id"]);
                    objUserPhoto.url = Convert.ToString(rdr["Url"]);
                    objUserPhoto.Description = Convert.ToString(rdr["Description"]);
                    objUserPhoto.AddDate = Convert.ToDateTime(rdr["AddDate"]);
                    objUserPhoto.IsMain = Convert.ToBoolean(rdr["IsMain"]);
                    objUserPhoto.PublicId = Convert.ToString(rdr["PublicId"]);                    
                }
                con.Close();
            }

            return await Task.FromResult(objUserPhoto);
        }

        public Task<List<PhotoDto>> GetPhoto()
        {
            List<PhotoDto> listPhoto = new List<PhotoDto>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetPhoto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PhotoDto objUserPhoto = new PhotoDto();
                    objUserPhoto.Id = Convert.ToInt32(rdr["Id"]);                    
                    objUserPhoto.url = Convert.ToString(rdr["Url"]);
                    objUserPhoto.Description = Convert.ToString(rdr["Description"]);
                    objUserPhoto.AddDate = Convert.ToDateTime(rdr["AddDate"]);
                    objUserPhoto.IsMain = Convert.ToBoolean(rdr["IsMain"]);
                    objUserPhoto.PublicId = Convert.ToString(rdr["PublicId"]);

                    listPhoto.Add(objUserPhoto);
                }
                con.Close();
            }

            return Task.FromResult(listPhoto);
        }

        public async Task<int> SetMainPhoto(int userId, int id)
        {
            PhotoDto objUserPhoto = new PhotoDto();
            int userIdOut = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_SetMainPhoto", con);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.Add("@userIdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                userIdOut = Convert.ToInt32(cmd.Parameters["@userIdOut"].Value);
                con.Close();                
            }

            return await Task.FromResult(userIdOut);
        }

        public async Task<int> DeletePhoto(int id)
        {
            PhotoDto objUserPhoto = new PhotoDto();
            int userIdOut = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_DeletePhoto", con);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.Add("@userIdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                userIdOut = Convert.ToInt32(cmd.Parameters["@userIdOut"].Value);
                con.Close();
            }

            return await Task.FromResult(userIdOut);
        }
    }
}
