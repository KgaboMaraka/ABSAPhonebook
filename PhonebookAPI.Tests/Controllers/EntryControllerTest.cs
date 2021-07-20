using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhonebookAPI.Controllers;
using PhonebookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace PhonebookAPI.Tests.Controllers
{
    /// <summary>
    /// Summary description for EntryControllerTest
    /// </summary>
    [TestClass]
    public class EntryControllerTest
    {
        public EntryControllerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetEntries_Returns_The_Correct_Number_Of_Entries()
        {
            //Arrange
            var testEntries = GetTestEntries();
            var controller = new EntryController(testEntries);

            // Act
            var result = controller.GetEntries() as List<Entry>;

            //Assert
            Assert.AreEqual(testEntries.Count, result.Count);
        }

        [TestMethod]
        public void PostEntry_Returns_HttpStatusCode_Created()
        {
            // Arrange
            var dbContext = A.Fake<PhonebookEntities>();            
            var fakeEntry = A.Fake<Entry>();

            IQueryable<Entry> fakeIQueryable = new List<Entry>().AsQueryable();

             var fakeDbSet = A.Fake<DbSet<Entry>>((d =>
                 d.Implements(typeof(IQueryable<Entry>))));

            A.CallTo(() => ((IQueryable<Entry>)fakeDbSet).GetEnumerator())
                .Returns(fakeIQueryable.GetEnumerator());
            A.CallTo(() => ((IQueryable<Entry>)fakeDbSet).Provider)
                .Returns(fakeIQueryable.Provider);
            A.CallTo(() => ((IQueryable<Entry>)fakeDbSet).Expression)
                .Returns(fakeIQueryable.Expression);
            A.CallTo(() => ((IQueryable<Entry>)fakeDbSet).ElementType)
              .Returns(fakeIQueryable.ElementType);

            var fakeContext = A.Fake<PhonebookEntities>();

            A.CallTo(() => fakeContext.Entries).Returns(fakeDbSet);

            var entryRepository = new EntryController(fakeContext);
            
            entryRepository.Configuration = new HttpConfiguration();

            entryRepository.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Entry")
            };

            entryRepository.Configuration.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional });

            entryRepository.RequestContext.RouteData = new HttpRouteData(
            route: new HttpRoute(),
            values: new HttpRouteValueDictionary { { "controller", "Entry" } });

            Entry entry = new Entry() { ID = 20, PhonebookID = 2, Name = "Test Name 1", PhoneNumber = "0723695958" };

            //Act
            var response = entryRepository.PostEntry(entry);

            // Assert
            Assert.AreEqual("http://localhost/api/Entry/20", response.Headers.Location.AbsoluteUri);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        private List<Entry> GetTestEntries()
        {
            var testEntries = new List<Entry>();
            testEntries.Add(new Entry { ID = 1, Name = "Test 1", PhoneNumber = "0721122325", PhonebookID = 1 });
            testEntries.Add(new Entry { ID = 2, Name = "Test 2", PhoneNumber = "0725588458", PhonebookID = 2 });
            testEntries.Add(new Entry { ID = 3, Name = "Test 3", PhoneNumber = "0712233659", PhonebookID = 3 });
            testEntries.Add(new Entry { ID = 4, Name = "Test 4", PhoneNumber = "0825588475", PhonebookID = 4 });
            testEntries.Add(new Entry { ID = 4, Name = "Test 4", PhoneNumber = "0762255845", PhonebookID = 5 });

            return testEntries;
        }
    }
}
