namespace Poster.Tests
{
    using Xunit;

    public class PosterBuilderTests
    {
        [Fact]
        public void Builder_Should_Build_New_Poster_Instance()
        {
            var poster = new PosterBuilder()
                .Build();
            
            Assert.NotNull(poster);
        }
    }
}