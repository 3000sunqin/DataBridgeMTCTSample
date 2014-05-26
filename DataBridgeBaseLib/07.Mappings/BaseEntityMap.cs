using System;
using System.Linq;
using DataBridgeBaseLib.Entities;
using FluentNHibernate.Mapping;

namespace DataBridgeBaseLib.Mappings
{
    public class BaseEntityMap<T> : ClassMap<T> where T : BaseEntity
    {
        public BaseEntityMap()
        {
            DynamicUpdate();
            OptimisticLock.Version();

            // ID Column
            Id(x => x.Id);

            // Version Column
            Version(x => x.RowVersion)
                .Nullable()
                .Generated.Always()
                .CustomSqlType("timestamp");
        }
    }
}
