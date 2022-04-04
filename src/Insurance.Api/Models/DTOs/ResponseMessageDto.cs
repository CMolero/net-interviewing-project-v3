namespace Insurance.Api.Models.DTOs
{
    public class ResponseMessageDto<T>
    {

        public ResponseMessageDto(bool isSuccess, string messsage, T? data)
        {
            IsSuccess = isSuccess;
            Messsage = messsage;
            Data = data;
        }

        public bool IsSuccess { get; set; }
        public string Messsage { get; set; } = string.Empty;
        public T? Data { get; set; }

    }
}
