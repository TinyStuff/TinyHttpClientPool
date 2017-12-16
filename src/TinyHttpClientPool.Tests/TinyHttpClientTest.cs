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

        [Fact]
        public void FlushTest()
        {
            // Arrange
            var pool = new TinyHttpClientPool();

            // Act
            var a = pool.Fetch();
            var b = pool.Fetch();

            // Assert and dispose
            Assert.Equal(0, pool.AvailableCount);
            Assert.Equal(2, pool.TotalPoolSize);

            a.Dispose();

            Assert.Equal(1, pool.AvailableCount);
            Assert.Equal(2, pool.TotalPoolSize);

            pool.Flush(); // Only removes unused

            Assert.Equal(0, pool.AvailableCount);
            Assert.Equal(1, pool.TotalPoolSize); // There should be one left since it's still in use
        }
    }
}
