using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsAction.mvcct
{

    /// <summary>
    /// Attribute to specify a changeset in MVCCT JsQueryable
    /// </summary>

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class JsqChangesetAttribute : Attribute
    {
        /// <summary>
        /// Object .ctor
        /// </summary>
        public JsqChangesetAttribute()
        {
            InsertedEntities = "Inserted";
            ModifiedEntities = "Modified";
            DeletedEntities = "Deleted";
            FatherReferencesEntities = "FatherReferences";
        }

        /// <summary>
        /// Inserted Entities name
        /// </summary>
        public string InsertedEntities { get; set; }

        /// <summary>
        ///  Modified Entities name
        /// </summary>
        public string ModifiedEntities { get; set; }
        /// <summary>
        /// Deleted Entities name
        /// </summary>
        public string DeletedEntities { get; set; }
        /// <summary>
        /// Father Reference Entities name
        /// </summary>
        public string FatherReferencesEntities { get; set; }
    }
}
