namespace Poster.Core.Attributes
{
    using Abstract;

    public class GetAttribute : HttpAttribute
    {
        public GetAttribute(string url) : base(url)
        {
        }
    }
}