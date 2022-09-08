using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrendyKart.API.Helper
{
    public static class HttpContextExtensions
    {
        public static void InsertPaginationParametersInResponse<T>(this HttpContext httpContext,
            IQueryable<T> queryable, int recordsPerPage)
        {
            try
            {
                if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }
                //var a= queryable.Count();
                var count = queryable.Count();
                var totalAmountOfPages = Math.Ceiling((double)count / (double)recordsPerPage);
                httpContext.Response.Headers.Add("totalAmountOfPages", totalAmountOfPages.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
