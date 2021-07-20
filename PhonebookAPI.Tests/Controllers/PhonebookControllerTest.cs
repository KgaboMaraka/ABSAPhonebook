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
    /// Summary description for PhonebookControllerTest
    /// </summary>
    [TestClass]
    public class PhonebookControllerTest
    {
        public PhonebookControllerTest()
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
        public void GetPhonebooks_Returns_The_Correct_Number_Of_Entries()
        {
            //Arrange
            var testPhonebooks = GetTestPhonebooks();
            var controller = new PhonebookController(testPhonebooks);

            // Act
            var result = controller.Get() as List<Phonebook>;

            //Assert
            Assert.AreEqual(testPhonebooks.Count, result.Count);
        }

        [TestMethod]
        public void Post_Returns_HttpStatusCode_Created()
        {
            // Arrange
            var dbContext = A.Fake<PhonebookEntities>();
            var fakeEntry = A.Fake<Phonebook>();

            IQueryable<Phonebook> fakeIQueryable = new List<Phonebook>().AsQueryable();

            var fakeDbSet = A.Fake<DbSet<Phonebook>>((d =>
                d.Implements(typeof(IQueryable<Phonebook>))));

            A.CallTo(() => ((IQueryable<Phonebook>)fakeDbSet).GetEnumerator())
                .Returns(fakeIQueryable.GetEnumerator());
            A.CallTo(() => ((IQueryable<Phonebook>)fakeDbSet).Provider)
                .Returns(fakeIQueryable.Provider);
            A.CallTo(() => ((IQueryable<Phonebook>)fakeDbSet).Expression)
                .Returns(fakeIQueryable.Expression);
            A.CallTo(() => ((IQueryable<Phonebook>)fakeDbSet).ElementType)
              .Returns(fakeIQueryable.ElementType);

            var fakeContext = A.Fake<PhonebookEntities>();

            A.CallTo(() => fakeContext.Phonebooks).Returns(fakeDbSet);

            var entryRepository = new PhonebookController(fakeContext);

            entryRepository.Configuration = new HttpConfiguration();

            entryRepository.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Phonebook")
            };

            entryRepository.Configuration.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional });

            entryRepository.RequestContext.RouteData = new HttpRouteData(
            route: new HttpRoute(),
            values: new HttpRouteValueDictionary { { "controller", "Phonebook" } });

            Phonebook phonebook = new Phonebook() { ID = 20, Name = "Test Phonebook 1" };

            //Act
            var response = entryRepository.Post(phonebook);

            // Assert
            Assert.AreEqual("http://localhost/api/Phonebook/20", response.Headers.Location.AbsoluteUri);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        private List<Phonebook> GetTestPhonebooks()
        {
            var testPhonebooks = new List<Phonebook>();
            testPhonebooks.Add(new Phonebook { ID = 1, Name = "Test 1" });
            testPhonebooks.Add(new Phonebook { ID = 2, Name = "Test 2" });
            testPhonebooks.Add(new Phonebook { ID = 3, Name = "Test 3" });
            testPhonebooks.Add(new Phonebook { ID = 4, Name = "Test 4" });
            testPhonebooks.Add(new Phonebook { ID = 4, Name = "Test 4" });

            return testPhonebooks;
        }
    }
}
