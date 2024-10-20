using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsApplication.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string code, string message) : base(code, message) { }
        public NotFoundException(string code, string message, List<string> errors) : base(code, message, errors) { }
    }
}
