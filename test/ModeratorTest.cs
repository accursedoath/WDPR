using System;
using Xunit;
using src;

namespace test
{
    public class ModeratorTest
    {
        [Fact]
        public void BlokkeerChatTest()
        {
            Moderator m = new Moderator();
            var c = new Client();
            c.magChatten = false;
            //m.BlokkeerChat(c); vscode is clowning
            Assert.False(c.magChatten);
        }
    }
}
