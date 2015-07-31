using System;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Facebook;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public partial class FacebookParserTestLibrary
    {
        private static FacebookParser Parser = new FacebookParser();

        [TestMethod]
        public async Task LoadFacebook1()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Facebook/FacebookQuery.json");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(24, data.Count());
            var item = data.First();
            Assert.AreEqual("8195378771_10153163840333772", item._id);
            Assert.AreEqual("Microsoft Lumia", item.Author);
            Assert.AreEqual("https://www.facebook.com/MicrosoftLumia/photos/a.107698418771.94817.8195378771/10153163840333772/?type=1", item.FeedUrl);
            Assert.AreEqual("https://scontent.xx.fbcdn.net/hphotos-xat1/v/t1.0-9/p130x130/11800619_10153163840333772_8431946436286493498_n.png?oh=e42a3a4764c38e49fcdd759a2dd31725&oe=564EFCCF", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2015-07-28T15:43:28+0000"), item.PublishDate);
            Assert.AreEqual("Power that delivers. #Lumia640XL http://lumia.ms/1yc8fI9", item.Content);
            Assert.AreEqual("Power that delivers. #Lumia640XL http://lumia.ms/1yc8fI9", item.Summary);
            Assert.AreEqual("Power that delivers. #Lumia640XL http://lumia.ms/1yc8fI9", item.Title);
        }

        [TestMethod]
        public async Task LoadFacebook2()
        {
            string plainContent = await Common.ReadAssetFile("/Assets/Facebook/FacebookQuery2.json");

            var data = Parser.Parse(plainContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(17, data.Count());
            var item = data.First();
            Assert.AreEqual("19034719952_10150889065674953", item._id);
            Assert.AreEqual("Real Madrid C.F.", item.Author);
            Assert.AreEqual("https://www.facebook.com/RealMadrid/videos/10150889065674953/", item.FeedUrl);
            Assert.AreEqual("https://fbcdn-vthumb-a.akamaihd.net/hvthumb-ak-xat1/v/t15.0-10/s130x130/11407376_10150889068494953_75849734_n.jpg?oh=74f550d45aba00fc64aafca89fd2e82a&oe=5646BEB3&__gda__=1448570675_a98157965d63ae7bc676188d96c12e1e", item.ImageUrl);
            Assert.AreEqual(DateTime.Parse("2015-07-28T13:50:20+0000"), item.PublishDate);
            Assert.AreEqual("Did you miss Benzema\u2019s goal against Manchester City or James\u2019 against Inter? You can watch all 7 goals that Madrid has scored so far on their #RMTour2015 here.\n\n\u00bfTe perdiste el golazo de Benzema ante el Manchester City o el de James ante el Inter? Aqu\u00ed puedes ver los 7 tantos que el Madrid ha marcado en su #RMTour2015 hasta el momento.", item.Content);
            Assert.AreEqual("Did you miss Benzema\u2019s goal against Manchester City or James\u2019 against Inter? You can watch all 7 goals that Madrid has scored so far on their #RMTour2015 here.\n\n\u00bfTe perdiste el golazo de Benzema ante el Manchester City o el de James ante el Inter? Aqu\u00ed puedes ver los 7 tantos que el Madrid ha marcado en su #RMTour2015 hasta el momento.", item.Summary);
            Assert.AreEqual("Did you miss Benzema\u2019s goal against Manchester City or James\u2019 against Inter? You can watch all 7 goals that Madrid has scored so far on their #RMTour2015 here.\n\n\u00bfTe perdiste el golazo de Benzema ante el Manchester City o el de James ante el Inter? Aqu\u00ed puedes ver los 7 tantos que el Madrid ha marcado en su #RMTour2015 hasta el momento.", item.Title);
        }

        [TestMethod]
        public void LoadNullQuery()
        {
            var data = Parser.Parse(null);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadEmptyQuery()
        {
            var data = Parser.Parse(string.Empty);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadInvalidXml()
        {
            ExceptionsAssert.Throws<Newtonsoft.Json.JsonSerializationException>(() => Parser.Parse("["));
        }
    }
}
