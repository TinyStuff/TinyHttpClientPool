using System;
using TinyHttpClientPoolLib;
using Xunit;

namespace TinyHttpClient.Tests
{
  
    public class TinyHttpClientTest
    {
        [Fact]
        public void InitializationTest()
        {
            // Arrange
            var pool = new TinyHttpClientPool();

            // Act
            var a = pool.Fetch();

            // Assert
            Assert.NotNull(a);
        }

        [Fact]
        public void NewInstanceTest()
        {
            // Arrange
            var pool = new TinyHttpClientPool();

            // Act
            var a = pool.Fetch();
            var b = pool.Fetch();

            // Assert
            Assert.NotSame(a, b);
        }

        [Fact]
        public void ReuseTest()
        {
            // Arrange
            var pool = new TinyHttpClientPool();

            // Act
            var a = pool.Fetch();
            a.Dispose();
            var b = pool.Fetch();

            // Assert
            Assert.Same(a, b);
        }
    }
}
