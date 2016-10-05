using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConfOrm;
using ConfOrm.NH;
using ConfOrm.Patterns;
using ConfOrm.Shop.CoolNaming;
using ConfOrm.Shop.DearDbaNaming;
using ConfOrm.Shop.Inflectors;
using ConfOrm.Shop.Patterns;
using KServices.Core.Domain.Data.Entities;
using MedTeam.Data.Core.Domain.Extensions;
using MedTeam.Data.Core.Domain.Model.Entities;
using NHibernate.Cfg.MappingSchema;

namespace KServices.Data.Mappings.Mapper
{
    /// <summary>
    /// Create mapping for <c>NHibernate</c> entities.
    /// </summary>
    public class DomainMapper
    {
        #region Public Methods

        /// <summary>
        /// Generates the mappings.
        /// </summary>
        /// <returns>Mapped entities</returns>
        public HbmMapping GenerateMappings()
        {
            var orm = new ObjectRelationalMapper();

            orm.Patterns.PoidStrategies.Add(new NativePoidPattern());

            //// map .NET4 ISet<T> as a NHibernate's set
            orm.Patterns.Sets.Add(new UseSetWhenGenericCollectionPattern());

            IEnumerable<Type> allPersistEntities = GetDomainEntities();

            IEnumerable<Type> roots = allPersistEntities.Where(t => t.IsAbstract && t.InheritedFromBaseEntity());

            IEnumerable<Type> hierarchyEntities = allPersistEntities.Where(t => typeof(IHierarchyEntity).IsAssignableFrom(t));

            IEnumerable<Type> separateEntities = allPersistEntities.Except(roots).Except(hierarchyEntities);
            orm.TablePerConcreteClass(separateEntities);
            
            var hierarchyRoots = hierarchyEntities.Where(t => t.IsAbstract && t.InheritedFromBaseEntity());
            orm.TablePerClassHierarchy(hierarchyRoots);

            //orm.Cascade<DocTemplate, Document>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Document, DocTemplate>(CascadeOn.Persist | CascadeOn.Merge);

            //orm.Cascade<TrackPackDocument, Document>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Document, TrackPackDocument>(CascadeOn.Persist);

            //orm.Cascade<BaseDocumentPack, Client>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Client, BaseDocumentPack>(CascadeOn.Persist);

            //orm.Cascade<BaseDocumentPack, Payer>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Payer, BaseDocumentPack>(CascadeOn.None);

            //orm.Cascade<TrackPackDocument, Payer>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Payer, TrackPackDocument>(CascadeOn.None);

            //orm.Cascade<Document, Client>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Client, Document>(CascadeOn.Persist);

            //orm.Cascade<Document, DocumentHistory>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<DocumentHistory, Document>(CascadeOn.Persist);

            //orm.Cascade<SamDTDb, SamDTOffice>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<SamDTOffice, Document>(CascadeOn.Persist | CascadeOn.Merge);

            //orm.Cascade<BaseDocumentPack, SamDTDb>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<SamDTDb, BaseDocumentPack>(CascadeOn.None);

            //orm.Cascade<BaseDocumentPack, SamDTOffice>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<SamDTOffice, BaseDocumentPack>(CascadeOn.None);

            //orm.Cascade<TrackPackDocument, Client>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Client, TrackPackDocument>(CascadeOn.None);

            //orm.Cascade<TrackPackDocument, PatUser>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<PatUser, TrackPackDocument>(CascadeOn.None);

            //orm.Cascade<BaseDocumentPack, TrackPackDocument>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<TrackPackDocument, BaseDocumentPack>(CascadeOn.Persist);

            //orm.Cascade<BaseDocumentPack, PatUser>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<PatUser, BaseDocumentPack>(CascadeOn.None);

            //orm.Cascade<OfficePushedDocumentPack, OfficePushedItem>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<OfficePushedItem, OfficePushedDocumentPack>(CascadeOn.Persist | CascadeOn.Merge);

            //orm.Cascade<OfficePushedItem, Group>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Group, OfficePushedItem>(CascadeOn.None);

            //orm.Cascade<OfficePushedItem, PatUser>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<PatUser, OfficePushedItem>(CascadeOn.None);

            //orm.Cascade<OfficePushedItem, TrackPackDocument>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<TrackPackDocument, OfficePushedItem>(CascadeOn.None);

            //orm.Cascade<UserDevice, PatUser>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<PatUser, UserDevice>(CascadeOn.Persist | CascadeOn.Merge);

            //orm.Cascade<Case, Visit>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Visit, Case>(CascadeOn.Persist | CascadeOn.Merge);

            //orm.Cascade<Employee, SamEmployee>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<SamEmployee, Employee>(CascadeOn.None);

            //orm.Cascade<EmployeeTrack, CaseFeedback>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<CaseFeedback, EmployeeTrack>(CascadeOn.Persist | CascadeOn.Merge);

            //orm.Cascade<Employee, Phone>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Phone, Employee>(CascadeOn.Persist);

            //orm.Cascade<Case, SamDTDb>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<SamDTDb, Case>(CascadeOn.None);

            //orm.Cascade<Case, SamDTOffice>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<SamDTOffice, Case>(CascadeOn.None);

            //orm.Cascade<Visit, Employee>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Employee, Visit>(CascadeOn.None);

            //orm.Cascade<WorkerType, Skill>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Skill, WorkerType>(CascadeOn.Persist);

            //orm.Cascade<Visit, WorkerType>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<WorkerType, Visit>(CascadeOn.None);

            //orm.Cascade<Visit, EmployeeTrack>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<EmployeeTrack, Visit>(CascadeOn.Persist);

            //orm.Cascade<Visit, Skill>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<Skill, Visit>(CascadeOn.Persist);

            //orm.Cascade<SiteChatHistory, ChatNotification>(CascadeOn.Persist | CascadeOn.Merge);
            //orm.Cascade<ChatNotification, SiteChatHistory>(CascadeOn.Persist);

            //orm.ManyToMany<Visit, Skill>();
            //orm.ManyToMany<Skill, Visit>();

            //orm.ManyToMany<PatUser, Case>();
            //orm.ManyToMany<Case, PatUser>();
            
            //orm.ManyToMany<WorkerType, Skill>();
            //orm.ManyToMany<Skill, WorkerType>();

            //orm.ManyToMany<BaseDocumentPack, SamDTDb>();
            //orm.ManyToMany<SamDTDb, BaseDocumentPack>();

            //orm.ManyToMany<BaseDocumentPack, SamDTOffice>();
            //orm.ManyToMany<SamDTOffice, BaseDocumentPack>();

            //orm.ManyToMany<PatUser, Group>();
            //orm.ManyToMany<Group, PatUser>();

            //orm.ManyToMany<DocTemplate, BaseDocumentPack>();
            //orm.ManyToMany<BaseDocumentPack, DocTemplate>();

            //orm.ManyToMany<BaseDocumentPack, SamDTDb>();
            //orm.ManyToMany<SamDTDb, BaseDocumentPack>();

            //orm.ManyToMany<BaseDocumentPack, SamDTOffice>();
            //orm.ManyToMany<SamDTOffice, BaseDocumentPack>();

            //orm.ManyToMany<Payer, BaseDocumentPack>();
            //orm.ManyToMany<BaseDocumentPack, Payer>();

            //orm.ManyToMany<Visit, Employee>();
            //orm.ManyToMany<Employee, Visit>();

            var mapper = new ConfOrm.NH.Mapper(orm, new CoolPatternsAppliersHolder(orm));
           var englishInflector = new EnglishInflector();
            mapper.PatternsAppliers.Merge(new ClassPluralizedTableApplier(englishInflector));

            //mapper.Class<FileStorage>(map => map.Property(o => o.File, pm => pm.Type(NHibernateUtil.BinaryBlob)));
            //mapper.Class<Document>(map => map.Property(o => o.ViewModelValues, pm => pm.Type(NHibernateUtil.StringClob)));

            //mapper.Class<DocumentPack>(map => map.ManyToOne(o => o.Editor, 
            //    pm => pm.ForeignKey("EditorId")));
            //mapper.Class<DocumentPack>(map => map.ManyToOne(o => o.Submitter, 
            //    mtom => mtom.ForeignKey("SubmitterId")));

            HbmMapping mapping = mapper.CompileMappingFor(allPersistEntities);
            return mapping;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all objects that are inherited from <see cref="BaseEntity"/>.
        /// </summary>
        /// <returns>All entities in the storage assembly</returns>
        private static IEnumerable<Type> GetDomainEntities()
        {
            Assembly domainAssembly = typeof(Account).Assembly;
            IEnumerable<Type> domainEntities =
                domainAssembly.GetTypes().Where(
                    t => (typeof(BaseEntity).IsAssignableFrom(t) && !t.IsGenericType && t != typeof(BaseEntity)));

            return domainEntities;
        }

        #endregion
    }
}