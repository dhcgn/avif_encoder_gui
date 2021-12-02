using FluentAssertions;
using Xunit;

namespace avifencodergui.lib.test
{
    public class CreateProgArgs
    {
        [Fact]
        public void New()
        {
            var c = new Config();

            var args = c.CreateProgArgs("input.jpg", "output.avif");

            args.Should().NotBeNull();
            args.Should().Be("\"input.jpg\" \"output.avif\"");
        }

        [Fact]
        public void CreateEmpty()
        {
            var c = Config.CreateEmpty();

            var args = c.CreateProgArgs("input.jpg", "output.avif");

            args.Should().NotBeNull();
            args.Should().Be("\"input.jpg\" \"output.avif\"");
        }


        [Fact]
        public void CreateSample1()
        {
            var c = Config.CreateSample1();

            var args = c.CreateProgArgs("input.jpg", "output.avif");

            args.Should().NotBeNull();
        }
    }
}