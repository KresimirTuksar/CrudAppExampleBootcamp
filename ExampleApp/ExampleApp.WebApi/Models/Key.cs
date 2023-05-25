using ExampleApp.WebApi.Interfaces;


namespace ExampleApp.WebApi.Models
{
    public class Key : Ikey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
    }
}