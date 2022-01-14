using System;
using Xunit;
using Moq;
using src.Models;

namespace test
{
    public class Bericht_Test
    {
        [Fact]
        public void BerichtText()
        {
            //Arrange
            var berichtMock = new Mock<Bericht>();
            berichtMock.SetupProperty(m => m.text, "Testbericht!");

            //Assert
            Assert.Equal("Testbericht!", berichtMock.Object.text);
        }

        [Fact]
        public void Datum_Check(){
            //Arrange
            //Act
            //Assert
            var nu = DateTime.Now;
            var bericht = new Bericht(){Datum = nu, text = null};
            Assert.Equal(nu, bericht.Datum);
        }

        [Fact]
        public void Verzender_Check(){
            //Arrange
            //Act
            //Assert
            var verzender = new Account("David",null,null);       
            var nu = DateTime.Now;
            var bericht = new Bericht(){Datum = nu, text = null, Verzender = verzender};
            Assert.Equal("David", bericht.Verzender.Voornaam);
        }
    }
}
