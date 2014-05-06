using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;

namespace SaasOvation.Common.Persistence {
    public class SessionProvider {
        private ISessionFactory _sessionFactory;
        private ISession _session;
        private readonly string _connectionString;
        private readonly Assembly[] _mapAssemblies;

        public SessionProvider(string connectionString, params Assembly[] mapAssemblies) {
            this._connectionString = connectionString;
            this._mapAssemblies = mapAssemblies;
        }

        private ISessionFactory GetSessionFactory() {
            if(this._sessionFactory == null) {
                this._sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(
                        this._connectionString))
                    .Mappings(m => _mapAssemblies.ForEach(assembly=>m.FluentMappings.AddFromAssembly(assembly)))
                    .BuildSessionFactory();
            }
            return this._sessionFactory;
        }

        private ISession GetNewSession() {
            return this.GetSessionFactory().OpenSession();
        }

        public ISession GetSession() {
            if(this._session==null ||!this._session.IsOpen) {
                this._session = this.GetNewSession();
            }
            return this._session;
        }
    }
}