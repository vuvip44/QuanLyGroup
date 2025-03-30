using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLy.Infrastructure
{
    public class ServiceException:Exception
    {
        public int StatusCode { get; set; } 
        public ServiceException(string message,int statusCode=400) : base(message)
        {
            StatusCode = statusCode;
        }

        public ServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    
}