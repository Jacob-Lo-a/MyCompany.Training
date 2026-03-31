namespace Training.API.Exceptions
{
    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException() : base ("只接受 .jpg, .jpeg 或 .png") { }
    }
}
