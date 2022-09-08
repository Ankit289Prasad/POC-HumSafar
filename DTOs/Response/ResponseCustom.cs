using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumSafar.API.DTOs.Response
{
    public class ResponseCustom<T>
    {
        public T Data { get; set; }
        public List<string> Message { get; set; } = new List<string>();
    }
    public class ResponseCustom
    {
        public List<string> Message { get; set; } = new List<string>();
    }

}
