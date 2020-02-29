using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DattingApp.API.Helper
{
    public static class Extensions
    {
        public static void AddApplicaitonError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error",message);
            response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPages, int totalItems, int totalPages)
        {
            var PaginationHeadre = new PaginationHeader(currentPage, itemsPerPages, totalItems, totalPages);
            var camelCaseFormator = new JsonSerializerSettings();
            camelCaseFormator.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(PaginationHeadre,camelCaseFormator));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

    }
}
