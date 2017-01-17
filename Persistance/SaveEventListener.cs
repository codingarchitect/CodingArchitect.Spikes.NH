using NHibernate.Event.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System.Reflection;
using CodingArchitect.Spikes.NH.Domain;

namespace CodingArchitect.Spikes.NH.Persistance
{
    public class SaveEventListener : DefaultSaveOrUpdateEventListener
    {
        protected override object PerformSave(object entity, object id, IEntityPersister persister, bool useIdentityColumn, object anything, IEventSource source, bool requiresImmediateIdAccess)
        {
            Console.WriteLine("Interception Event fired. PerformSave");
            var textStorageProperties = (from p in persister.EntityMetamodel.Properties
                           where p.Type.Name == "CodingArchitect.Spikes.NH.Domain.TextStorage"
                           select p);
            foreach(var textStorageProperty in textStorageProperties)
            {
                PropertyInfo pi = entity.GetType().GetProperty(textStorageProperty.Name);
                var textStorage = (TextStorage)pi.GetValue(entity);
                if (textStorage != null && textStorage.TextNumber == 0)
                    pi.SetValue(entity, null);
            }
            return base.PerformSave(entity, id, persister, useIdentityColumn, anything, source, requiresImmediateIdAccess);
        }
    }
}
