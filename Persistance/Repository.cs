using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.IO;
using System.Text;

namespace CodingArchitect.Spikes.NH.Persistance
{
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
                if (persistentClass.ClassName == "CodingArchitect.Spikes.NH.Domain.Customer, CodingArchitect.Spikes.NH, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
                    persistentClass.AddTuplizer(EntityMode.Poco, typeof(NullableTuplizer).AssemblyQualifiedName);
            }
            var schemaOutput = new StringBuilder();
            var textWriter = new StringWriter(schemaOutput);
            new SchemaExport(Cfg).Execute(true, true, false);
            SessionFactory = Cfg.BuildSessionFactory();
        }
    }
}
