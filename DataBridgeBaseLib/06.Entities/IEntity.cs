using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBridgeBaseLib.Entities
{
    /// <summary>
    /// The interface which all entity object need to implement.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The primary key of the entity, that means each entity class have to include an unique Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// If NOT persisted in the database
        /// </summary>
        bool IsDirty { get; set; }

        /// <summary>
        ///  If New or any value has changed
        /// </summary>
        bool IsNew { get; }
    }
}
