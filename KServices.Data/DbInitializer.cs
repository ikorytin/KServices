using KServices.Data.HqlGenerators;
using KServices.Data.Mappings.Mapper;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;

namespace KServices.Data
{
    /// <summary>
    /// Represents class for <c>NHibernate</c> initializing.
    /// </summary>
    public class DbInitializer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbInitializer"/> class.
        /// </summary>
        /// <param name="connectionKey">The connection key.</param>
        public DbInitializer(string connectionKey)
        {
            ConnectionKey = connectionKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>The factory.</value>
        public static ISessionFactory Factory { get; protected set; }

        #endregion

        #region Properties

        /// <summary>
        /// Represents a connection key for accessing to DB.
        /// </summary>
        protected string ConnectionKey { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the session factory.
        /// </summary>
        public void Initialize()
        {
            Factory = BuildFactory();
        }

        /// <summary>
        /// Opens the session.
        /// </summary>
        /// <returns>The <c>NHibernate</c> session.</returns>
        public virtual ISession OpenSession()
        {
            return Factory.OpenSession();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the factory.
        /// </summary>
        /// <returns>The session factory.</returns>
        internal virtual ISessionFactory BuildFactory()
        {
            Configuration cfg = GetConfiguration();            
            return cfg.BuildSessionFactory();
        }
        
        /// <summary>
        /// Gets the <c>NHibernate</c> configuration.
        /// </summary>
        /// <returns>The <c>NHibernate</c> configuration.</returns>
        internal virtual Configuration GetConfiguration()
        {
            var mapper = new DomainMapper();
            HbmMapping generateMappings = mapper.GenerateMappings();

            var cfg = new Configuration();
            cfg.SessionFactory().Proxy.Through<NHibernate.Bytecode.DefaultProxyFactoryFactory>().Integrate.Using<MsSql2008Dialect>()
                .AutoQuoteKeywords().Connected.By<SqlClientDriver>().ByAppConfing(ConnectionKey).CreateCommands
                .ConvertingExceptionsThrough<SQLStateConverter>();
            cfg.SetProperty("show_sql", "true");
            cfg.SetProperty("format_sql", "true");
            cfg.AddDeserializedMapping(generateMappings, string.Empty);
            cfg.Properties.Add(Environment.LinqToHqlGeneratorsRegistry, typeof(LinqToHqlGeneratorsRegistry).AssemblyQualifiedName);            

            // We need it to create database schema
            new SchemaUpdate(cfg).Execute(true, true);
            return cfg;
        }

        #endregion
    }
}