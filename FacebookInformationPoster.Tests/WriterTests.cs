using Autofac;
using Autofac.Extras.Moq;
using System;
using System.IO.Abstractions;
using Xunit;

namespace FacebookInformationPoster.Tests
{
    public class WriterTests
    {
        [Fact]
        public void Write_HappyPath()
        {
            using var am = AutoMock.GetLoose();
            var writer = am.Create<Writer>();
            writer.Log("test");
        }
    }
}
