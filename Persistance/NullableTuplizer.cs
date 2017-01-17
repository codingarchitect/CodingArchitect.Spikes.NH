using CodingArchitect.Spikes.NH.Domain;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Tuple.Entity;
using System;
using System.Collections;
using System.Reflection;

namespace CodingArchitect.Spikes.NH.Persistance
{
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
}
