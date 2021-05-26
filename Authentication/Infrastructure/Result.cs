using System.Security.Policy;

namespace Authentication.Infrastructure
{
    public class Result
    {
        public bool IsSuccessful { get; protected set; }
        public string ErrorMessage { get; protected set; }

        public Result(bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }
        
        public static Result<T2> Success<T2>(T2 value)
        {
            return new Result<T2>(true, null, value);
        }

        public static Result<T2> Failure<T2>(string errorMsg = null)
        {
            return new Result<T2>(false, errorMsg, default);
        }

        public static Result Success()
            => new Result(true, null);
        
        public static Result Failure(string errorMsg = null)
            => new Result(false, errorMsg);
    }
    
    public class Result<T> : Result
    {
        public T Value { get; private set; }

        public Result(bool isSuccessful, string errorMessage, T value)
        :base(isSuccessful, errorMessage)
        {
            Value = value;
        }
    }
}