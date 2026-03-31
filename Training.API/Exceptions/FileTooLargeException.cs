namespace Training.API.Exceptions
{
    public class FileTooLargeException : Exception
    {
        public FileTooLargeException() : base ("檔案大小不可超過 5MB") { }
    }
}
