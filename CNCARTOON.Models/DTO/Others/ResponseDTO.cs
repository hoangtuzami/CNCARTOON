using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCARTOON.Models.DTO.Others
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public object Result { get; set; } = null;
    }
}
