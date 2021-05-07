using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.ExceptionHandler.Exception
{
    [Serializable]
    public class ErrorCodeException<T> : System.Exception
    {
        public ErrorCodeException(T code)
        {
            Code = code;
        }

        public ErrorCodeException(T code, string message)
            : base(message)
        {
            Code = code;
        }

        public ErrorCodeException(T code, string message, System.Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        public T Code { get; }
    }

    // Helper methods that allow for type inference (ctor cannot due to c# limitations)
    // Usage "throw ErrorCodeException.Create(ErrorCodeEnum.MyError)" 
    // instead of specifying type, i.e:
    // "throw new ErrorCodeException<ErrorCodeEnum>(ErrorCodeEnum.MyError)"
    public static class ErrorCodeException
    {
        public static ErrorCodeException<T> Create<T>(T code)
        {
            return new ErrorCodeException<T>(code);
        }

        public static ErrorCodeException<T> Create<T>(T code, string message)
        {
            return new ErrorCodeException<T>(code, message);
        }

        public static ErrorCodeException<T> Create<T>(T code, string message, System.Exception innerException)
        {
            return new ErrorCodeException<T>(code, message, innerException);
        }
    }
}
