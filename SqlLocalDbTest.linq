<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.CSharp.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.InteropServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.InteropServices.WindowsRuntime.dll</Reference>
  <NuGetReference>CodingArchitect.Utilities</NuGetReference>
  <NuGetReference>Microsoft.Web.Administration</NuGetReference>
  <NuGetReference>NHibernate</NuGetReference>
  <NuGetReference>NUnitLite</NuGetReference>
  <NuGetReference>SqlLocalDb</NuGetReference>
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>Microsoft.CSharp.RuntimeBinder</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

void Main(string[] args)
{
	var queryDirectory = Path.GetDirectoryName(Util.CurrentQueryPath);	
	
	var companyName = "CodingArchitect";	
	var applicationName = "NH";
	var appPhysicalPath = Path.Combine(queryDirectory, applicationName);
	if (!Directory.Exists(appPhysicalPath)) Directory.CreateDirectory(appPhysicalPath);
	CreateNHConfig(appPhysicalPath);
	CodingArchitect.Utilities.Linqpad.Util.SetupApplication(companyName, applicationName, queryDirectory, Util.CurrentQueryPath);		
}


private static void CreateNHConfig(string appPhysicalPath)
{
	var hibernateCfgFilePath = Path.Combine(appPhysicalPath, "hibernate.cfg.xml");
	var customerMappingFilePath = Path.Combine(appPhysicalPath, "Customer.hbm.xml");
	var textStorageMappingFilePath = Path.Combine(appPhysicalPath, "TextStorage.hbm.xml");
	HibernateConfigFile(hibernateCfgFilePath);
	CreateConfigFileForCustomer(customerMappingFilePath);
	CreateConfigFileForTextStorage(textStorageMappingFilePath);
}

public static void CreateConfigFileForCustomer(string customerEntityConfigFilePath)
{
	var ns = XNamespace.Get("urn:nhibernate-mapping-2.2");
	new XDocument(
		new XElement(ns + "hibernate-mapping",
			new XAttribute("assembly", "CodingArchitect.Spikes.NH"),
			new XAttribute("namespace", "CodingArchitect.Spikes.NH.Domain"),
			new XElement(ns + "class",
				new XAttribute("name", "Customer"),
				new XElement(ns + "id", 
					new XAttribute("name", "CustomerNumber"),
					new XAttribute("access", "property"),
					new XAttribute("type", "Int32"),
					new XElement(ns + "generator",
						new XAttribute("class", "identity")
					)
				),
				new XElement(ns + "property", 
					new XAttribute("name", "FirstName")
				),
				new XElement(ns + "property", 
					new XAttribute("name", "LastName")
				)
				,
				new XElement(ns + "many-to-one", 
					new XAttribute("name", "Comment"),
					new XAttribute("class", "CodingArchitect.Spikes.NH.Domain.TextStorage, CodingArchitect.Spikes.NH"),
					new XAttribute("access", "property"),
					new XAttribute("not-found", "ignore"),
					new XAttribute("cascade", "all-delete-orphan")
				)
				
			)
		)
	).Save(customerEntityConfigFilePath);
}

public static void CreateConfigFileForTextStorage(string textStorageEntityConfigFilePath)
{
	var ns = XNamespace.Get("urn:nhibernate-mapping-2.2");
	new XDocument(
		new XElement(ns + "hibernate-mapping",
			new XAttribute("assembly", "CodingArchitect.Spikes.NH"),
			new XAttribute("namespace", "CodingArchitect.Spikes.NH.Domain"),
			new XElement(ns + "class",
				new XAttribute("name", "TextStorage"),
				new XElement(ns + "id", 
					new XAttribute("name", "TextNumber"),
					new XAttribute("unsaved-value", "-1")
				),
				new XElement(ns + "property", 
					new XAttribute("name", "TextData")
				)
			)
		)
	).Save(textStorageEntityConfigFilePath);
}

public static void HibernateConfigFile(string hibernateCfgFilePath)
{
	var ns = XNamespace.Get("urn:nhibernate-configuration-2.2");
	new XDocument(
		new XElement(ns + "hibernate-configuration",
			new XElement(ns + "session-factory",
				new XElement(ns + "property", 
					new XAttribute("name", "connection.provider"),
					"CodingArchitect.Spikes.NH.Persistance.SqlLocalDbConnectionProvider, CodingArchitect.Spikes.NH"
				),
				new XElement(ns + "property", 
					new XAttribute("name", "dialect"),
					"NHibernate.Dialect.MsSql2012Dialect"
				),
				new XElement(ns + "property", 
					new XAttribute("name", "connection.driver_class"),
					"CodingArchitect.Spikes.NH.Persistance.SqlLocalDbClientDriver, CodingArchitect.Spikes.NH"
				),
				new XElement(ns + "property", 
					new XAttribute("name", "connection.connection_string"),
					""
				),
				new XElement(ns + "property", 
					new XAttribute("name", "connection.release_mode"),
					"auto"
				),
				new XElement(ns + "property", 
					new XAttribute("name", "show_sql"),
					"true"
				)
				/*
				,new XElement(ns + "property", 
					new XAttribute("name", "proxyfactory.factory_class"),
					"NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle"
				)*/
			)			
		)
	).Save(hibernateCfgFilePath);
}

// Define other methods and classes here
} 

#region Service Classes
namespace CodingArchitect.Spikes.NH
{	
	using NUnitLite;
	using CodingArchitect.Utilities.AppDomain;
	public class Program : ConsoleCatcherBase
	{
	   public static void Main(string[] args)
	   {
	   		
	   }
	   
	   public override void DoExecute()
	   {
	   		new AutoRun().Execute(new string[] {});
	   }
	}	
}

namespace CodingArchitect.Spikes.NH.Domain
{
	using CodingArchitect.Spikes.NH.Persistance;
	
    public class Customer
    {
        public virtual int CustomerNumber { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual TextStorage Comment { get; set; }
    }
	
	public class TextStorage
    {
        public virtual int TextNumber { get; set; }
        public virtual string TextData { get; set; }
    }
	
	public static class TextStorageUtility
    {
        private static TextStorage emptyTextStorage;
        public static TextStorage EmptyTextStorage
        {
            get
            {
                if (emptyTextStorage == null)
                    lock(typeof(TextStorage))
                    {
                        if (emptyTextStorage == null)
                        {
                            LoadEmptyTextStorage();
                        }
                    }
                return emptyTextStorage;
            }
        }
        public static void LoadEmptyTextStorage()
        {
            var sessionFactory = Repository.SessionFactory;
            using (var session = sessionFactory.OpenSession())
            {
                emptyTextStorage = session.Get<TextStorage>(0);
            }
        }
    }
}

namespace CodingArchitect.Spikes.NH.Persistance
{
	using NHibernate;
	using NHibernate.Cfg;
	using NHibernate.Connection;
	using NHibernate.Driver;
	using NHibernate.Mapping;
	using NHibernate.Tuple.Entity;
	using NHibernate.Tool.hbm2ddl;
	using SqlLocalDb;
	using System;
	using System.Data;
	using System.IO;
	using System.Text;
	using System.Reflection;
	
    public static class Repository
    {
        static Repository()
        {
            Configure();
        }

        public static NHibernate.Cfg.Configuration Cfg;
        public static ISessionFactory SessionFactory;

        private static void Configure()
        {
            Cfg = new Configuration();
            Cfg.Configure();
            Cfg.AddFile("Customer.hbm.xml");
            Cfg.AddFile("TextStorage.hbm.xml");
            //cfg.EventListeners.SaveEventListeners = new [] { new SaveEventListener() as ISaveOrUpdateEventListener }
            //    .Concat(cfg.EventListeners.SaveEventListeners)
            //    .ToArray();
            foreach (var persistentClass in Cfg.ClassMappings)
            {
                if (persistentClass.ClassName == "CodingArchitect.Spikes.NH.Domain.Customer, CodingArchitect.Spikes.NH, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
                    persistentClass.AddTuplizer(EntityMode.Poco, typeof(NullableTuplizer).AssemblyQualifiedName);
            }
            var schemaOutput = new StringBuilder();
            var textWriter = new StringWriter(schemaOutput);
            new SchemaExport(Cfg).Execute(true, true, false);
            SessionFactory = Cfg.BuildSessionFactory();
        }
    }
	
	public class NullableTuplizer : PocoEntityTuplizer
    {
        private static Type TextStorageUtiltyType = Type.GetType("CodingArchitect.Spikes.NH.Domain.TextStorageUtility, CodingArchitect.Spikes.NH");
        private static PropertyInfo EmptyTextStorageUtiltyPropertyInfo = TextStorageUtiltyType.GetProperty("EmptyTextStorage");
        private static object EmptyTextStorage = null;
        public NullableTuplizer(EntityMetamodel entityMetamodel, PersistentClass mappedEntity)
            : base(entityMetamodel, mappedEntity)
        {
        }
        public override object[] GetPropertyValues(object entity)
        {
            return base.GetPropertyValues(entity);
        }
        public override object GetPropertyValue(object entity, int i)
        {
            var propertyValue = base.GetPropertyValue(entity, i);
            if (propertyValue != null)
            {
                var textStorageType = propertyValue.GetType();
                if (textStorageType.Name == "TextStorage")
                {
                    var propertyInfo = textStorageType.GetProperty("TextNumber");
                    var textNumber = (int)propertyInfo.GetValue(propertyValue);
                    if (textNumber == 0)
                    {
                        if (EmptyTextStorage == null) EmptyTextStorage = EmptyTextStorageUtiltyPropertyInfo.GetValue(null);
                        return EmptyTextStorage;
                    }
                }
            }
            return propertyValue;
        }   
    }
	
	public sealed class SqlLocalDbClientDriver : DriverBase
    {
        private readonly LocalDatabase database = SqlLocalDbInstanceProvider.Instance;
        private IDbConnection connection;
        public override IDbConnection CreateConnection()
        {
            connection = database.GetConnection();
            connection.Close();
            Console.WriteLine(connection.State);
            return connection;
        }

        public override IDbCommand CreateCommand()
        {
            connection = database.GetConnection();
            return connection.CreateCommand();
        }

        public override bool UseNamedPrefixInSql
        {
            get { return true; }
        }
        public override bool UseNamedPrefixInParameter
        {
            get { return true; }
        }
        public override string NamedPrefix
        {
            get { return "@"; }
        }
    }
	
	public class SqlLocalDbConnectionProvider : DriverConnectionProvider
    {
        public override IDbConnection GetConnection()
        {
            LocalDatabase database = SqlLocalDbInstanceProvider.Instance;
            var connection = database.GetConnection();
            return connection;
        }
    }
	
	public static class SqlLocalDbInstanceProvider
    {
        public static LocalDatabase Instance = new LocalDatabase("NHTest");
    }
}

namespace CodingArchitect.Spikes.NH.Tests
{
	using CodingArchitect.Spikes.NH.Persistance;
	using CodingArchitect.Spikes.NH.Domain;
	using SqlLocalDb;
	using NUnitLite;
	using NUnit.Framework;
	
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

#endregion
class EOF {