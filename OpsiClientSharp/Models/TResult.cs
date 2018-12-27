namespace OpsiClientSharp.Models
{
    public class TResult<T>
    {
        public int Id { get; set; }

        public T Result { get; set; }

        public Error Error { get; set; }
    }
}
