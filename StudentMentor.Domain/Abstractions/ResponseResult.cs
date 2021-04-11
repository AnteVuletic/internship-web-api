namespace StudentMentor.Domain.Abstractions
{
    public class ResponseResult
    {
        public static readonly ResponseResult Ok = new ResponseResult();
        public static ResponseResult Error(string message) => new ResponseResult(message, true);

        public string Message { get; private set; }
        public bool IsError { get; private set; }

        public ResponseResult(bool isError = false)
        {
            IsError = isError;
        }

        public ResponseResult(string message, bool isError)
        {
            Message = message;
            IsError = isError;
        }
    }

    public class ResponseResult<TData> : ResponseResult where TData : class
    {
        public TData Data { get; private set; }

        public ResponseResult(TData data, bool isError = false) : base(isError)
        {
            Data = data;
        }

        public ResponseResult(TData data, string message, bool isError = false) : base(message, isError)
        {
            Data = data;
        }

        public new static readonly ResponseResult<TData> Ok = new ResponseResult<TData>(null);
        public new static ResponseResult<TData> Error(string message) => new ResponseResult<TData>(null, message, true);
    }
}
