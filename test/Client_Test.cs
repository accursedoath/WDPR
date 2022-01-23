using System;
using Xunit;
using Moq;
using src.Models;

namespace test
{
    public class Client_Test
    {
        [Fact]
        public void magChattenCheck()
        {
            var client = new Client();
            client.magChatten = true;
            Assert.True(client.magChatten);
        }
    }
}
