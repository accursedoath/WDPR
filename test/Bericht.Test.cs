using System;
using Xunit;
using src.Models;

namespace test
{
    public class Bericht_Test
    {
        [Fact]
        public void BerichtText()
        {
            var text = "hoe gaat het?";
            var bericht = new Bericht(){Datum = DateTime.Now, text = text };
            Assert.Equal(text, bericht.text);
        }

        [Fact]
        public void Datum_Check(){
            var nu = DateTime.Now;
            var bericht = new Bericht(){Datum = nu, text = null};
            Assert.Equal(nu, bericht.Datum);
        }

    }
}
