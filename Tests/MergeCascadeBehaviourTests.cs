using CodingArchitect.Spikes.NH.Domain;
using CodingArchitect.Spikes.NH.Persistance;
using NUnit.Framework;
using SqlLocalDb;
using System.Data;

namespace CodingArchitect.Spikes.NH.Tests
{
    [TestFixture]
    public class MergeCascadeBehaviourTest
    {

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var emptyTextStorage = TextStorageUtility.EmptyTextStorage;
            if (emptyTextStorage == null)
                using (var connection = new LocalDatabase("NHTest").GetConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO TextStorage VALUES (0, 'Empty')";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
        }

        [Test]
        public void CanCreateCustomer()
        {
            var customer1 = new Customer();
            customer1.FirstName = "Sendhil Kumar";
            customer1.LastName = "Ramalingam";
            customer1.Comment = new TextStorage
            {
                TextData = "Empty"
            };
            var customer2 = new Customer();
            customer2.FirstName = "Sony";
            customer2.LastName = "Arouje";
            customer2.Comment = new TextStorage
            {
                TextData = "Empty"
            };
            var customer3 = new Customer();
            customer3.FirstName = "John";
            customer3.LastName = "Doe";
            customer3.Comment = new TextStorage
            {
                TextNumber = 1,
                TextData = "John Doe"
            };
            var customer4 = new Customer();
            customer4.FirstName = "Jane";
            customer4.LastName = "Doe";
            var sessionFactory = Repository.SessionFactory;
            using (var session = sessionFactory.OpenSession())
            {
                session.Save(customer1);
                session.Save(customer2);
                session.Save(customer3);
                session.Save(customer4);
            }

            using (var session = sessionFactory.OpenSession())
            {
                customer1 = session.Get<Customer>(1);
                customer2 = session.Get<Customer>(2);
                customer3 = session.Get<Customer>(3);
                customer4 = session.Get<Customer>(4);
                Assert.IsNotNull(customer1);
                Assert.IsNotNull(customer2);
                Assert.IsNotNull(customer3);
                Assert.IsNotNull(customer4);
            }            
        }
    }
}
