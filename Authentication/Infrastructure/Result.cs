namespace Authentication.Infrastructure
{
    public class Result
    {
        public static Result<T2> Success<T2>(T2 value)
        {
            return new Result<T2>(true, null, value);
        }

        public static Result<T2> Failure<T2>(string errorMsg = null)
        {
            return new Result<T2>(false, errorMsg, default);
        }
    }
    
    public class Result<T>
    {
        public bool IsSuccessful { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Value { get; private set; }

        public Result(bool isSuccessful, string errorMessage, T value)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            Value = value;
        }
    }
}