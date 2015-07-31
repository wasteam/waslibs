using System;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Menu;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class GenericParserTestLibrary
    {
        private static GenericParser<MenuSchema> MenuParser = new GenericParser<MenuSchema>();
        private static GenericParser<CollectionSchema> CollectionParser = new GenericParser<CollectionSchema>();

        [TestMethod]
        public async Task LoadMenuContactUs()
        {
            string menuContent = await Common.ReadAssetFile("/Assets/Generic/ContactUs.json");

            var data = MenuParser.Parse(menuContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(4, data.Count());
            var first = data.First();
            Assert.AreEqual("5ff5e805-dbdb-466b-97fe-ed12e51a61f2", first._id);
            Assert.AreEqual("DeepLink", first.MenuType);
            Assert.AreEqual("/Assets/DataImages/Item-dd7eb8ba-7f16-46b0-9a38-ff3ebe9433c5.png", first.Icon);
            Assert.AreEqual("bingmaps:?collection=point.47.70117950439453_-122.13028717041016_Contoso&lvl=18", first.Target);
        }

        [TestMethod]
        public async Task LoadCollection()
        {
            string collectionContent = await Common.ReadAssetFile("/Assets/LocalCollectionData.json");

            var data = CollectionParser.Parse(collectionContent);

            Assert.IsNotNull(data);
            Assert.AreEqual(5, data.Count());
            var first = data.First();
            Assert.AreEqual("555ee83e352d0403381fdb7a", first._id);
            Assert.AreEqual("Catherina", first.Name);
            Assert.AreEqual("Dicant evertitur mei in, ne his deserunt perpetua sententiae, ea sea omnes similique vituperatoribus. Ex mel errem intellegebat comprehensam, vel ad tantas antiopam delicatissimi, tota ferri affert eu nec. Legere expetenda pertinacia ne pro, et pro impetus persius assueverit.\r\n\r\nEa mei nullam facete, omnis oratio offendit ius cu. Doming takimata repudiandae usu an, mei dicant takimata id, pri eleifend inimicus euripidis at. His vero singulis ea, quem euripidis abhorreant mei ut, et populo iriure vix. Usu ludus affert voluptaria ei, vix ea error definitiones, movet fastidii signiferumque in qui.", first.PersonalSummary);
            Assert.AreEqual("http://img-dev.winappstudio.net/web-resources/Windows%20App%20Studio/Content.jpg", first.Image);
            Assert.AreEqual("Vis prodesset adolescens adipiscing te, usu mazim perfecto recteque at, assum putant erroribus mea in. Vel facete imperdiet id, cum an libris luptatum perfecto, vel fabellas inciderint ut. Veri facete debitis ea vis, ut eos oratio erroribus. Sint facete perfecto no vel, vim id omnium insolens. Vel dolores perfecto pertinacia ut, te mel meis ullum dicam, eos assum facilis corpora in.", first.Other);
            Assert.AreEqual("+00 000 000 000", first.Phone);
            Assert.AreEqual("catherina@mail.com", first.Mail);
            Assert.AreEqual("http://img-dev.winappstudio.net/web-resources/Windows%20App%20Studio/Content-1.jpg", first.Thumbnail);
        }

        [TestMethod]
        public void LoadNullMenu()
        {
            ExceptionsAssert.Throws<ArgumentNullException>(() => MenuParser.Parse(null));
        }

        [TestMethod]
        public void LoadEmptyMenu()
        {
            var data = MenuParser.Parse(string.Empty);

            Assert.IsNull(data);
        }

        [TestMethod]
        public void LoadNullCollection()
        {
            ExceptionsAssert.Throws<ArgumentNullException>(() => CollectionParser.Parse(null));
        }

        [TestMethod]
        public void LoadEmptyCollection()
        {
            var data = CollectionParser.Parse(string.Empty);

            Assert.IsNull(data);
        }
    }
}
