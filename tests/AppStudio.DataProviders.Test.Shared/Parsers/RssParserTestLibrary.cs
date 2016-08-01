using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AppStudio.DataProviders.Rss;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.Parsers
{
    [TestClass]
    public class RssParserTestLibrary
    {
        private static RssParser Parser = new RssParser();

        [TestMethod]
        public async Task TestRssImageRecovery1()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssImageNoEnclosure1.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            var item = data.First();
            Assert.AreEqual("http://4.fotos.web.sapo.io/i/B5911520a/18497358_cJ0Z6.png", item.ImageUrl);
        }

        [TestMethod]
        public async Task TestRssImageRecovery2()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssImageNoEnclosure2.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            var item = data.First();
            Assert.AreEqual("http://blogs.msdn.com/resized-image.ashx/__size/150x0/__key/communityserver-blogs-components-weblogfiles/00-00-01-65-36/6087.1.png", item.ImageUrl);
        }

        [TestMethod]
        public async Task TestRssImageRecoveryFromEnclosure()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssEnclosure.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            var item = data.First();
            Assert.AreEqual("http://blogs.msdn.com/cfs-file.ashx/__key/communityserver-components-postattachments/00-10-59-53-90/2.png", item.ImageUrl);
        }

        [TestMethod]
        public async Task TestRssSummaryContent()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssSummaryContent.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            var first = data.First();
            Assert.IsNotNull(first.Content);
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content does have html");
            Assert.IsFalse(first.Content.Contains("function msoCommentShow"), "The script is not succesfully stripped from content");
            Assert.IsNotNull(first.Summary);
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Summary.StartsWith("I’d like to start this blog post with a quick introduction."), "The summary does not strip html tags");
            Assert.IsFalse(first.Summary.Contains("function msoCommentShow"), "The script is not succesfully stripped from summary");
        }

        [TestMethod]
        public async Task TestRssSummaryComment()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssSummaryComment.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            var first = data.First();
            Assert.IsNotNull(first.Content);
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content does have html");
            Assert.IsFalse(first.Content.Contains("<!--"), "The comments are not succesfully stripped from content");
            Assert.IsNotNull(first.Summary);
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsFalse(first.Summary.Contains("<!--"), "The comments are not succesfully stripped from summary");
        }

        [TestMethod]
        public async Task TestAtomSample1()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/AtomSample1.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(1, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.IsNull(first.ImageUrl);
            Assert.AreEqual(string.Empty, first.Author);
            Assert.AreEqual("Atom-Powered Robots Run Amok", first.Title);
            Assert.AreEqual("http://example.org/2003/12/13/atom03", first.FeedUrl);
            Assert.AreEqual("urn:uuid:1225c695-cfb8-4ebb-aaaa-80da344efa6a", first._id);
            Assert.AreEqual("Some text.", first.Summary);
        }

        [TestMethod]
        public async Task TestAtomSample2()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/AtomSample2.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(15, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.IsNull(first.ImageUrl);
            Assert.AreEqual("Julian Robichaux", first.Author);
            Assert.AreEqual("Java Agent HTTP Connection Errors (Thursday, Jan 15)", first.Title);
            Assert.AreEqual("http://www.nsftools.com/blog/blog-01-2015.htm#01-15-15", first.FeedUrl);
            Assert.AreEqual("http://www.nsftools.com/blog/blog-01-2015.htm#01-15-15", first._id);
            Assert.AreEqual(DateTime.Parse("2015-01-15T17:56:18Z"), first.PublishDate);
            Assert.IsTrue(first.Summary.Contains("While working through some demos for my upcoming"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
        }

        [TestMethod]
        public async Task TestAtomWordpress()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/AtomWordpress.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(6, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.IsNull(first.ImageUrl);
            Assert.AreEqual("Ruth Garner", first.Author);
            Assert.AreEqual("Sweeping views and enchanting history at Greenway house and gardens near Brixham", first.Title);
            Assert.AreEqual("http://www.thisisyourkingdom.co.uk/article/greenway-devon/", first.FeedUrl);
            Assert.AreEqual("http://www.thisisyourkingdom.co.uk/?post_type=article&p=12829", first._id);
            Assert.AreEqual(DateTime.Parse("2015-05-22T08:24:38Z"), first.PublishDate);
            Assert.IsTrue(first.Summary.Contains("Positioned on a sweeping hillside beside the beautiful Dart estuary is the National Trust"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
        }

        [TestMethod]
        public async Task TestAtomBlogger()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/AtomBlogger.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(25, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.AreEqual("http://2.bp.blogspot.com/-c6AgJCcbbjY/VXXAQlymeSI/AAAAAAAABqM/P7IWEjDAK8M/s1600/trips1.png", first.ImageUrl);
            Assert.AreEqual("The Gmail Team", first.Author);
            Assert.AreEqual("Trip Bundles in Inbox by Gmail", first.Title);
            Assert.AreEqual("http://feedproxy.google.com/~r/OfficialGmailBlog/~3/MSuRQ7Ymrk4/trip-bundles-in-inbox-by-gmail.html", first.FeedUrl);
            Assert.AreEqual("tag:blogger.com,1999:blog-6781693.post-7092230003619582030", first._id);
            Assert.AreEqual(DateTime.Parse("2015-06-08T09:22:00.000-07:00"), first.PublishDate);
            Assert.IsTrue(first.Summary.Contains("I frequently travel to Mountain View and Seattle and always have a mess"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
        }


        [TestMethod]
        public async Task TestAtomSvbtle()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/AtomSvbtle.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(2, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.IsNull(first.ImageUrl);
            Assert.AreEqual(string.Empty, first.Author);
            Assert.AreEqual("The Adventure Continues -  ECPAT", first.Title);
            Assert.AreEqual("http://synergyblog.svbtle.com/ecpat", first.FeedUrl);
            Assert.AreEqual("tag:synergyblog.svbtle.com,2014:Post/ecpat", first._id);
            Assert.AreEqual(DateTime.Parse("2014-03-07T03:52:07-08:00"), first.PublishDate);
            Assert.IsTrue(first.Summary.Contains("To be completely honest, I thought they were but a "), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Content.Contains("To be completely honest, I thought they were but a "), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssBlogger()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssBlogger.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(25, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.AreEqual("http://2.bp.blogspot.com/-c6AgJCcbbjY/VXXAQlymeSI/AAAAAAAABqM/P7IWEjDAK8M/s1600/trips1.png", first.ImageUrl);
            Assert.AreEqual("The Gmail Team", first.Author);
            Assert.AreEqual("Trip Bundles in Inbox by Gmail", first.Title);
            Assert.AreEqual("http://feedproxy.google.com/~r/OfficialGmailBlog/~3/MSuRQ7Ymrk4/trip-bundles-in-inbox-by-gmail.html", first.FeedUrl);
            Assert.AreEqual("tag:blogger.com,1999:blog-6781693.post-7092230003619582030", first._id);
            Assert.AreEqual(DateTime.Parse("2015-06-08T09:22:00.000-07:00"), first.PublishDate);
            Assert.IsTrue(first.Summary.Contains("I frequently travel to Mountain View and Seattle and always have a mess"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Content.Contains("I frequently travel to Mountain View and Seattle and always have a mess"), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssGhost()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssGhost.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(15, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.AreEqual("http://blog.ghost.org/content/images/2015/06/privateblogs.jpg", first.ImageUrl);
            Assert.AreEqual("John O'Nolan", first.Author);
            Assert.AreEqual("Private Blogs", first.Title);
            Assert.AreEqual("http://blog.ghost.org/private-blogs/", first.FeedUrl);
            Assert.AreEqual("dbff77a6-d3cd-48d0-90ff-199510a231f2", first._id);
            Assert.AreEqual(DateTime.Parse("Mon, 01 Jun 2015 13:33:19 GMT"), first.PublishDate);
            Assert.AreEqual("We've introduced password protection for posts on your Ghost blog so you can run a simple, private or internal company blog without hassle.", first.Summary);
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Content.Contains("You can now run a private (password protected) blog with Ghost."), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssWordpress()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssWordpress.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(10, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.IsNull(first.ImageUrl);
            Assert.AreEqual("Molly Woodstock", first.Author);
            Assert.AreEqual("Fishing near Portland", first.Title);
            Assert.AreEqual("http://www.travelportland.com/article/fishing-near-portland/", first.FeedUrl);
            Assert.AreEqual("http://www.travelportland.com/?p=27361", first._id);
            Assert.AreEqual(DateTime.Parse("Tue, 02 Jun 2015 18:48:54 +0000"), first.PublishDate);
            Assert.IsTrue(first.Summary.StartsWith("These four waterways — all less than an hour away from Portland — provide the perfect setting for your next fish tale."), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Content.Contains("Oregon’s pristine waters make it a paradise for fishers, boaters and adventure-seekers."), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssSquarespace()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssSquarespace.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(20, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.AreEqual("http://static1.squarespace.com/static/4f0b1540d09a72608d47a4e1/4f0b1e6ad09a72608d47b308/55526600e4b03baa3e6f3f70/1431463432570/1.png?format=1000w", first.ImageUrl);
            Assert.AreEqual("Jesse Rose", first.Author);
            Assert.AreEqual("The Flatiron School", first.Title);
            Assert.AreEqual("http://www.bighuman.com/work/the-flatiron-school", first.FeedUrl);
            Assert.AreEqual("4f0b1540d09a72608d47a4e1:4f0b1e6ad09a72608d47b308:553fe4fce4b069a00eda332e", first._id);
            Assert.AreEqual(DateTime.Parse("Tue, 12 May 2015 20:48:44 +0000"), first.PublishDate);
            Assert.IsTrue(first.Summary.Contains("throw a Ruby library in this town without hitting"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Content.Contains("throw a Ruby library in this town without hitting"), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssTumblr()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssTumblr.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(20, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.IsNull(first.ImageUrl);
            Assert.AreEqual(string.Empty, first.Author);
            Assert.AreEqual("JetAtomic: need copywriting? 50% of fee donated to haiti relief RT @workinonaramp: comment with ideas. http://bit.ly/7UiliV", first.Title);
            Assert.AreEqual("http://jetatomic.tumblr.com/post/343891597", first.FeedUrl);
            Assert.AreEqual("http://jetatomic.tumblr.com/post/343891597", first._id);
            Assert.AreEqual(DateTime.Parse("Tue, 19 Jan 2010 22:01:11 -0800"), first.PublishDate);
            Assert.IsTrue(first.Summary.Contains("JetAtomic: need copywriting? 50% of fee donated to haiti relief RT"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Content.Contains("JetAtomic: need copywriting? 50% of fee donated to haiti relief RT"), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssMarca()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssMarca.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(20, data.Count());
            var element = data.First();
            Assert.IsNull(element.MediaUrl);
            Assert.IsNull(element.ExtraImageUrl);
            Assert.AreEqual("http://estaticos.marca.com/imagenes/2015/06/11/motor/modelos-coches/1434020195_extras_portadilla_1.jpg", element.ImageUrl);
            Assert.AreEqual("PABLO ÚBEDA", element.Author);
            Assert.AreEqual("Seat Ibiza 2015: progreso continuista", element.Title);
            Assert.AreEqual("http://marca.feedsportal.com/c/33136/f/538105/s/472bdf44/sc/18/l/0L0Smarca0N0C20A150C0A60C110Cmotor0Cmodelos0Ecoches0C14340A20A1950Bhtml/story01.htm", element.FeedUrl);
            Assert.AreEqual("http://www.marca.com/2015/06/11/motor/modelos-coches/1434020195.html", element._id);
            Assert.AreEqual(DateTime.Parse("Fri, 12 Jun 2015 07:27:23 GMT"), element.PublishDate);
            Assert.IsTrue(element.Summary.Contains("Estrena nuevos sistemas de conectividad y actualiza oferta mecánica. Motores de gasolina y diésel"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(element.Summary), "Summary does not have html");
            Assert.IsTrue(element.Content.Contains("Estrena nuevos sistemas de conectividad y actualiza oferta mecánica. Motores de gasolina y diésel"), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(element.Content), "Content has html");

            element = data.ElementAt(1);
            Assert.IsNull(element.MediaUrl);
            Assert.IsNull(element.ExtraImageUrl);
            Assert.AreEqual("http://estaticos.marca.com/movil/imagenes/2015/06/10/motor/modelos-coches/1433965267_extras_portadilla_1_150.jpg", element.ImageUrl);
            Assert.AreEqual("Enrique Naranjo", element.Author);
            Assert.AreEqual("BMW Serie 7, salto al futuro", element.Title);
            Assert.AreEqual("http://marca.feedsportal.com/c/33136/f/538105/s/472bdf3e/sc/18/l/0L0Smarca0N0C20A150C0A60C10A0Cmotor0Cmodelos0Ecoches0C14339652670Bhtml/story01.htm", element.FeedUrl);
            Assert.AreEqual("http://www.marca.com/2015/06/10/motor/modelos-coches/1433965267.html", element._id);
            Assert.AreEqual(DateTime.Parse("Fri, 12 Jun 2015 07:25:08 GMT"), element.PublishDate);
            Assert.IsTrue(element.Summary.Contains("En octubre llegará la sexta generación del buque insignia de BMW, que se convertirá en uno de los"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(element.Summary), "Summary does not have html");
            Assert.IsTrue(element.Content.Contains("En octubre llegará la sexta generación del buque insignia de BMW, que se convertirá en uno de los"), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(element.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssMeneame()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssMeneame.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(20, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.AreEqual("https://mnmstatic.net/cache/25/0e/media_thumb-link-2428470.jpeg?1434099006", first.ImageUrl);
            Assert.AreEqual("juanlu73", first.Author);
            Assert.AreEqual("Rita Barberá dimite", first.Title);
            Assert.AreEqual("http://meneame.feedsportal.com/c/34737/f/639540/s/472c8464/sc/3/l/0M0Smeneame0Bnet0Cstory0Crita0Ebarbera0Edimite/story01.htm", first.FeedUrl);
            Assert.AreEqual("https://www.meneame.net/story/rita-barbera-dimite", first._id);
            Assert.AreEqual(DateTime.Parse("Fri, 12 Jun 2015 09:15:05 GMT"), first.PublishDate);
            Assert.IsTrue(first.Summary.Contains("La alcaldesa en funciones de la ciudad de Valencia y durante los"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Content.Contains("La alcaldesa en funciones de la ciudad de Valencia y durante los"), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssUDiscoverMusic()
        {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssUDiscoverMusic.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            Assert.AreEqual(10, data.Count());
            var first = data.First();
            Assert.IsNull(first.MediaUrl);
            Assert.IsNull(first.ExtraImageUrl);
            Assert.AreEqual("http://www.udiscovermusic.com/wp-content/uploads/2015/06/moodyblues-150x150.jpg", first.ImageUrl);
            Assert.AreEqual("pausextudusm", first.Author);
            Assert.AreEqual("New Moody Blues Music In The Pipeline?", first.Title);
            Assert.AreEqual("http://www.udiscovermusic.com/new-moody-blues-music-in-the-pipeline", first.FeedUrl);
            Assert.AreEqual("http://www.udiscovermusic.com/?p=22619", first._id);
            Assert.AreEqual(DateTime.Parse("Mon, 15 Jun 2015 21:20:35 +0000"), first.PublishDate);
            Assert.IsTrue(first.Summary.StartsWith("As the Moody Blues continue their current UK tour,"), "Summary");
            Assert.IsFalse(Common.CheckHasHtmlTags(first.Summary), "Summary does not have html");
            Assert.IsTrue(first.Content.Contains("For all their tireless activity as a live group (in which Hayward is joined by his fellow longtime members John Lodge and Graeme Edge)"), "Content");
            Assert.IsTrue(Common.CheckHasHtmlTags(first.Content), "Content has html");
        }

        [TestMethod]
        public async Task TestRssYandexFulltextTag() {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssYandexFulltextTag.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            var item = data.First();
            Assert.IsTrue(item.Content.Contains("Международный день памятников и"), "Content should contains 'yandex:full-text' tag text in priority to 'description' tag");
        }

        [TestMethod]
        public async Task TestRssYandexFulltextSummaryTag() {
            string rssContents = await Common.ReadAssetFile("/Assets/Rss/RssYandexFulltextTag.xml");

            var data = Parser.Parse(rssContents);

            Assert.IsTrue(data.Any());
            var item = data.First();
            Assert.IsTrue(item.Summary.Contains("Международный день памятников и"), "Summary should contains 'yandex:full-text' tag text in priority to 'description' tag");
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
            ExceptionsAssert.Throws<XmlException>(() => Parser.Parse("<xml>"));
        }
    }
}