using NUnit.Framework;
using ServiceStack.Text;
using Sheep.ServiceModel.Books;
using Sheep.ServiceModel.Chapters;
using Sheep.ServiceModel.Paragraphs;
using Sheep.ServiceModel.Subjects;
using Sheep.ServiceModel.Volumes;

namespace Sheep.Tests.ServiceInterface.Books
{
    [TestFixture]
    public class BookBibleParagraphAnnotationClientTest : BookClientTestBase
    {
        [Test]
        public void CreateParagraphAnnotations()
        {
            ServiceClient.Post(new ParagraphAnnotationCreate { BookId = "bible", VolumeNumber = 1, ChapterNumber = 1, ParagraphNumber = 1, AnnotationNumber = 1, Title = "书名", Annotation = "《创世记》" }).PrintDump();
        }
    }
}