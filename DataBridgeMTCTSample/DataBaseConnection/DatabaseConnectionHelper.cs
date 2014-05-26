using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;


namespace DataBridgeMTCTSample.DataBaseConnection
{
    public static class DatabaseConnectionHelper
    {
        public static ISessionFactory CreateDatabaseSessionFactory()
        {
            return Fluently.Configure().Database(
                MsSqlConfiguration.MsSql2008.ConnectionString(
                x => x.Server(@"CN01WPSUN-11A\SQLEXPRESS").Database("DataBridgeMTCTSample").Username("sa").Password("123")))
                .Mappings(x=>x.FluentMappings.AddFromAssembly(typeof(DatabaseConnectionHelper).Assembly).ExportTo(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase)).BuildSessionFactory();
        }
    }
}
