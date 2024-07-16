namespace MiniSpacePwa.DTO.Wrappers
{
    public class ResponseDto<T>
    {
        public T Content { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}