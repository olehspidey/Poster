namespace Poster.Http.Models
{
    public class Parameter
    {
        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        
        public object Value { get; }
    }
}