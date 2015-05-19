using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NManga;
using NManga.Controllers;
using NUnit.Framework;
using Moq;
using NManga.Models;
using NManga.DataAccess;

namespace NManga.Tests.Controllers
{
    [TestFixture]
    public class ComicControllerTests
    {
        public class Package
        {
            public Mock<IComicDac> ComicDacMock { get; set; }
            public ComicController Tested { get; set; }

            public Package()
            {
                ComicDacMock = new Mock<IComicDac>();
                Tested = new ComicController(ComicDacMock.Object);
            }
        }

        /// <summary>
        /// Requirement: If we are doing a lookup of objects from the database, null results should throw exceptions.
        /// </summary>
        [Test]
        public void Id_Based_Action_Methods_Should_Throw_Exception_On_Null_Result()
        {
            var package = new Package();

            // We do nothing. This will make our mock DAC always return null.

            Assert.Throws(typeof(Exception), delegate { package.Tested.View(1); });
            Assert.Throws(typeof(Exception), delegate { package.Tested.Image(1); });
            Assert.Throws(typeof(Exception), delegate { package.Tested.Edit(1); });
        }
        
        /// <summary>
        /// Requirement: A permanent key is not acceptable to number comics. We must use an ordinal for navigational purposes.
        /// The controller actions must look up objects by ordinal.
        /// </summary>
        [Test]
        public void Id_Based_Action_Methods_Should_Use_Ordinal_For_Id()
        {
            var package = new Package();

            package.ComicDacMock.Setup(x => x.GetComicByOrdinal(It.IsAny<int>())).Returns(new Comic());

            package.Tested.View(1);
            package.ComicDacMock.Verify(x => x.GetComicByOrdinal(It.IsAny<int>()), Times.Exactly(1));

            package.Tested.Edit(1);
            package.ComicDacMock.Verify(x => x.GetComicByOrdinal(It.IsAny<int>()), Times.Exactly(2));
        }

        /// <summary>
        /// Requirement: The default view for the controller should show the latest comic.
        /// </summary>
        [Test]
        public void Index_Should_Get_Latest_Comic()
        {
            var package = new Package();

            package.ComicDacMock.Setup(x => x.GetLatestComic()).Returns(new Comic());

            package.Tested.Index();

            package.ComicDacMock.Verify(x => x.GetLatestComic(), Times.Once);
        }

        /// <summary>
        /// Requirement: The homepage must gracefully handle the lack of any comics in the database.
        /// </summary>
        [Test]
        public void If_No_Comics_Present_Index_Should_Throw_Exception()
        {
            var package = new Package();

            Assert.Throws(typeof(Exception), delegate { package.Tested.Index(); });
        }
    }
}
