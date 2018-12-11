namespace OpsiClientSharp.Models
{
    public class CommandResponse<T>
    {
        public int Id { get; set; }

        public T Result { get; set; }

        public Error Error { get; set; }
    }
}
