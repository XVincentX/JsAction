﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("SiteDbModel", "ToDoTasks", "ToDo", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(Mvc_Examples.Models.ToDo), "SubTasks", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(Mvc_Examples.Models.SubTasks), true)]

#endregion

namespace Mvc_Examples.Models
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class SiteDbEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new SiteDbEntities object using the connection string found in the 'SiteDbEntities' section of the application configuration file.
        /// </summary>
        public SiteDbEntities() : base("name=SiteDbEntities", "SiteDbEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new SiteDbEntities object.
        /// </summary>
        public SiteDbEntities(string connectionString) : base(connectionString, "SiteDbEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new SiteDbEntities object.
        /// </summary>
        public SiteDbEntities(EntityConnection connection) : base(connection, "SiteDbEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<ToDo> ToDo
        {
            get
            {
                if ((_ToDo == null))
                {
                    _ToDo = base.CreateObjectSet<ToDo>("ToDo");
                }
                return _ToDo;
            }
        }
        private ObjectSet<ToDo> _ToDo;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<SubTasks> SubTasks
        {
            get
            {
                if ((_SubTasks == null))
                {
                    _SubTasks = base.CreateObjectSet<SubTasks>("SubTasks");
                }
                return _SubTasks;
            }
        }
        private ObjectSet<SubTasks> _SubTasks;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the ToDo EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToToDo(ToDo toDo)
        {
            base.AddObject("ToDo", toDo);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the SubTasks EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToSubTasks(SubTasks subTasks)
        {
            base.AddObject("SubTasks", subTasks);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SiteDbModel", Name="SubTasks")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class SubTasks : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new SubTasks object.
        /// </summary>
        /// <param name="code">Initial value of the code property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="workingDays">Initial value of the WorkingDays property.</param>
        public static SubTasks CreateSubTasks(global::System.Int32 code, global::System.String name, global::System.Int32 workingDays)
        {
            SubTasks subTasks = new SubTasks();
            subTasks.code = code;
            subTasks.Name = name;
            subTasks.WorkingDays = workingDays;
            return subTasks;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 code
        {
            get
            {
                return _code;
            }
            set
            {
                if (_code != value)
                {
                    OncodeChanging(value);
                    ReportPropertyChanging("code");
                    _code = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("code");
                    OncodeChanged();
                }
            }
        }
        private global::System.Int32 _code;
        partial void OncodeChanging(global::System.Int32 value);
        partial void OncodeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 WorkingDays
        {
            get
            {
                return _WorkingDays;
            }
            set
            {
                OnWorkingDaysChanging(value);
                ReportPropertyChanging("WorkingDays");
                _WorkingDays = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("WorkingDays");
                OnWorkingDaysChanged();
            }
        }
        private global::System.Int32 _WorkingDays;
        partial void OnWorkingDaysChanging(global::System.Int32 value);
        partial void OnWorkingDaysChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> C_Order_
        {
            get
            {
                return _C_Order_;
            }
            set
            {
                OnC_Order_Changing(value);
                ReportPropertyChanging("C_Order_");
                _C_Order_ = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("C_Order_");
                OnC_Order_Changed();
            }
        }
        private Nullable<global::System.Int32> _C_Order_;
        partial void OnC_Order_Changing(Nullable<global::System.Int32> value);
        partial void OnC_Order_Changed();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> idTask
        {
            get
            {
                return _idTask;
            }
            set
            {
                OnidTaskChanging(value);
                ReportPropertyChanging("idTask");
                _idTask = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("idTask");
                OnidTaskChanged();
            }
        }
        private Nullable<global::System.Int32> _idTask;
        partial void OnidTaskChanging(Nullable<global::System.Int32> value);
        partial void OnidTaskChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SiteDbModel", "ToDoTasks", "ToDo")]
        public ToDo ToDo
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ToDo>("SiteDbModel.ToDoTasks", "ToDo").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ToDo>("SiteDbModel.ToDoTasks", "ToDo").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<ToDo> ToDoReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ToDo>("SiteDbModel.ToDoTasks", "ToDo");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<ToDo>("SiteDbModel.ToDoTasks", "ToDo", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SiteDbModel", Name="ToDo")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class ToDo : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new ToDo object.
        /// </summary>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="id">Initial value of the id property.</param>
        /// <param name="description">Initial value of the Description property.</param>
        /// <param name="dueDate">Initial value of the DueDate property.</param>
        public static ToDo CreateToDo(global::System.String name, global::System.Int32 id, global::System.String description, global::System.DateTime dueDate)
        {
            ToDo toDo = new ToDo();
            toDo.Name = name;
            toDo.id = id;
            toDo.Description = description;
            toDo.DueDate = dueDate;
            return toDo;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    OnidChanging(value);
                    ReportPropertyChanging("id");
                    _id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("id");
                    OnidChanged();
                }
            }
        }
        private global::System.Int32 _id;
        partial void OnidChanging(global::System.Int32 value);
        partial void OnidChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                OnDescriptionChanging(value);
                ReportPropertyChanging("Description");
                _Description = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Description");
                OnDescriptionChanged();
            }
        }
        private global::System.String _Description;
        partial void OnDescriptionChanging(global::System.String value);
        partial void OnDescriptionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime DueDate
        {
            get
            {
                return _DueDate;
            }
            set
            {
                OnDueDateChanging(value);
                ReportPropertyChanging("DueDate");
                _DueDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DueDate");
                OnDueDateChanged();
            }
        }
        private global::System.DateTime _DueDate;
        partial void OnDueDateChanging(global::System.DateTime value);
        partial void OnDueDateChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SiteDbModel", "ToDoTasks", "SubTasks")]
        public EntityCollection<SubTasks> SubTasks
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<SubTasks>("SiteDbModel.ToDoTasks", "SubTasks");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<SubTasks>("SiteDbModel.ToDoTasks", "SubTasks", value);
                }
            }
        }

        #endregion
    }

    #endregion
    
}
