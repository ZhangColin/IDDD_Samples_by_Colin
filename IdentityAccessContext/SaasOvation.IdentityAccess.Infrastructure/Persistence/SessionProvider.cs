using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence {
    public class SessionProvider {
        private ISessionFactory _sessionFactory;
        private ISession _session;

        private ISessionFactory GetSessionFactory() {
            if(this._sessionFactory == null) {
                this._sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(
                        "server=.;uid=sa;pwd=truth;Trusted_Connection=no;database=IDDD"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionProvider>())
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