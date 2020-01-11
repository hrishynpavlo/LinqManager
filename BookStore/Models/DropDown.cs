namespace BookStore.Models
{
    public class DropDown<T> where T: struct
    {
        public T Key { get; set; }
        public string Value { get; set; }
    }
}