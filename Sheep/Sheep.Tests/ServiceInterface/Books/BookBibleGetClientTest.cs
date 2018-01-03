using System;
using NUnit.Framework;
using ServiceStack.Text;
using Sheep.ServiceModel.Chapters;
using Sheep.ServiceModel.Paragraphs;
using Sheep.ServiceModel.Volumes;

namespace Sheep.Tests.ServiceInterface.Books
{
    [TestFixture]
    public class BookBibleGetClientTest : BookClientTestBase
    {
        [Test]
        public void GetVolumes()
        {
            var volumesResponse = ServiceClient.Get(new VolumeList
                                                    {
                                                        BookId = "bible"
                                                    });
            foreach (var volume in volumesResponse.Volumes)
            {
                var chaptersResponse = ServiceClient.Get(new ChapterList
                                                         {
                                                             BookId = "bible",
                                                             VolumeNumber = volume.Number
                                                         });
                foreach (var chapter in chaptersResponse.Chapters)
                {
                    try
                    {
                        var paragraphResponse = ServiceClient.Get(new ParagraphList
                                                                  {
                                                                      BookId = "bible",
                                                                      VolumeNumber = volume.Number,
                                                                      ChapterNumber = chapter.Number
                                                                  });
                        foreach (var paragraph in paragraphResponse.Paragraphs)
                        {
                            try
                            {
                                ServiceClient.Get(new ParagraphAnnotationList
                                                  {
                                                      BookId = "bible",
                                                      VolumeNumber = volume.Number,
                                                      ChapterNumber = chapter.Number,
                                                      ParagraphNumber = paragraph.Number
                                                  });
                            }
                            catch (Exception pex)
                            {
                                pex.Message.PrintDump();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Message.PrintDump();
                    }
                }
            }
        }
    }
}