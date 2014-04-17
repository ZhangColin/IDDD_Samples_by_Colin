using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Hql.Ast.ANTLR;

namespace SaasOvation.IdentityAccess.Domain.Test {
    public class SessionProvider {
        private ISessionFactory _sessionFactory;
        private ISession _session;

        private ISessionFactory GetSessionFactory() {
            if(_sessionFactory == null) {
                _sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(
                    ""))
                    .BuildSessionFactory();
            }
            return _sessionFactory;
        }

        private ISession GetNewSession() {
            return this.GetSessionFactory().OpenSession();
        }

        public ISession GetSession() {
            if(_session==null ||!_session.IsOpen) {
                _session = this.GetNewSession();
            }
            return _session;
        }
    }
}