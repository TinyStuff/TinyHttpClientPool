using System;
using System.Linq;
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

        [Fact]
        public void ResetHeadersOnReuseTest()
        {
            // Arrange
            var pool = new TinyHttpClientPool(
                new TinyHttpClientPoolConfiguration()
                {
                    ResetHeadersOnReuse = true
                });

            // Act
            var a = pool.Fetch();
			a.DefaultRequestHeaders.Add("Ducks", "are awesome");
            a.Dispose();

            var b = pool.Fetch();

            // Assert
            Assert.Same(a, b);
            Assert.Equal(0, b.DefaultRequestHeaders.Count());
        }

        [Fact]
        public void ReuseHeadersOnReuseTest()
        {
            // Arrange
            var pool = new TinyHttpClientPool(
                new TinyHttpClientPoolConfiguration()
                {
                    ResetHeadersOnReuse = false
                });

            // Act
            var a = pool.Fetch();
            a.DefaultRequestHeaders.Add("Ducks", "are awesome");
            a.Dispose();

            var b = pool.Fetch();

            // Assert
            Assert.Same(a, b);
            Assert.Equal(1, b.DefaultRequestHeaders.Count());
        }

        [Fact]
        public void BaseUrlThroughConfigurationTest()
        {
            // Arrange
            var pool = new TinyHttpClientPool(
                new TinyHttpClientPoolConfiguration()
                {
                    BaseUrl = "https://www.ducksareawesome.net"
                });

            // Act
            var a = pool.Fetch();

            // Assert
            Assert.Equal("https://www.ducksareawesome.net/", a.BaseAddress.ToString());
        }

        [Fact]
        public void InitializeClientTest()
        {
            // Arrange
            var i = 0;
            var pool = new TinyHttpClientPool();
            pool.ClientInitializationOnCreation = (obj) => i++;

            // Act
            var a = pool.Fetch();
            a.Dispose();
            var b = pool.Fetch();
            b.Dispose();

            // Assert
            Assert.Equal(1, i);
        }

        [Fact]
        public void InitializeClientOnFetchTest()
        {
            // Arrange
            var i = 0;
            var pool = new TinyHttpClientPool();
            pool.ClientInitializationOnFetch = (obj) => i++;

            // Act
            var a = pool.Fetch();
            a.Dispose();
            var b = pool.Fetch();
            b.Dispose();

            // Assert
            Assert.Equal(2, i);
        }
    }
}
