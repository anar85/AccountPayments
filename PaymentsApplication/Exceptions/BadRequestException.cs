using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsApplication.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string code, string message) : base(code, message) { }
        public BadRequestException(string code, string message, List<string> errors) : base(code, message, errors) { }
    }
}
