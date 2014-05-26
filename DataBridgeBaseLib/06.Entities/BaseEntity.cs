using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataBridgeBaseLib.Entities
{
    [Serializable()]
    //  TODO:  Move the INotifyPropertyChanged and IDataErrorInfo to the model layer
    public abstract partial class BaseEntity : BaseObject, IEntity, IDataErrorInfo
    {
        #region Constructors
        public BaseEntity()
        {
        }
        #endregion Constructors

        #region Properties
        
        /// <summary>
        /// We use RowVersion property to indicate whether this entity object was persisted.
        /// In the database table, the ID field is a auto increated identification field.
        /// </summary>
        public virtual byte[] RowVersion { get; set; }

        private int _nId = 0;
        /// <summary>
        /// the Primary key of the entity.
        /// </summary>
        public virtual int Id
        {
            get { return _nId; }
            set
            {
                if (_nId != value)
                {
                    _nId = value;
                    base.IsDirty = true;
                    OnPropertyChanged("Id");
                }
            }
        }

        /// <summary>
        /// if the id property isn't 0, it means this entity was saved into database.
        /// </summary>
        public virtual bool IsNew
        {
            get { return Id == 0; }
        }

        /// <summary>
        /// If New or any value has changed
        /// </summary>
        public override bool IsDirty
        {
            get { return base.IsDirty; }
            set
            {
                if (base.IsDirty != value)
                {
                    base.IsDirty = value;
                    OnPropertyChanged("IsDirty");
                    OnPropertyChanged("CanSave");
                }
            }
        }

        private bool _bCanCreate = true;
        public virtual bool CanCreate
        {
            get { return _bCanCreate; }
        }

        /// <summary>
        /// Whether this entity can be deleted,
        /// </summary>
        public virtual bool CanDelete
        {
            get
            {   // if this entity is new entity, it is not persisted, it can't be deleted.
                if (IsNew)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// If this entity can be saved?
        /// </summary>
        public virtual bool CanSave
        {
            get
            {
                if (!IsDirty) //if the entity is not dirty (a new entity is alway dirty), that means, it can not be saved into database.
                {
                    return false;
                }

                else if (HasErrors)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private Dictionary<string, string> _validationErrors = new Dictionary<string, string>();
        
        /// <summary>
        /// ToDo: Why this property is private?  this is the collection of validation errors.
        /// </summary>
        private Dictionary<string, string> ValidationErrors
        {
            get { return _validationErrors; }
        }

        /// <summary>
        /// If the validation process gets errors.
        /// </summary>
        public virtual bool HasErrors
        {
            get { return ValidationErrors.Count > 0; }
        }
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// Clear all the errors in _validationErrors collection
        /// </summary>
        public virtual void ClearErrors()
        {
            if (ValidationErrors.Count > 0)
            {
                ValidationErrors.Clear();
                OnPropertyChanged("Error");
                OnPropertyChanged("HasErrors");
                OnPropertyChanged("CanSave");
            }
        }

        /// <summary>
        /// Add error to validationError collection
        /// </summary>
        /// <param name="key">the key value of the error</param>
        /// <param name="value">value part of the error</param>
        public virtual void AddError(string key, string value)
        {
            if (!ValidationErrors.ContainsKey(key))
            {
                ValidationErrors.Add(key, value);
                OnPropertyChanged("Error");

                if (ValidationErrors.Count == 1)
                {
                    OnPropertyChanged("HasErrors");
                    OnPropertyChanged("CanSave");
                }
            }
        }


        /// <summary>
        /// Remove Error from _validationErrors
        /// </summary>
        /// <param name="key">the key value of the error</param>
        public virtual void RemoveError(string key)
        {
            if (ValidationErrors.ContainsKey(key))
            {
                ValidationErrors.Remove(key);
                OnPropertyChanged("ValidationErrors");
                if (ValidationErrors.Count == 0)
                {
                    OnPropertyChanged("HasErrors");
                    OnPropertyChanged("CanSave");
                }
            }
        }

        /// <summary>
        /// For copy exposed properties from other enity with same type
        /// </summary>
        /// <param name="myEntity">the source entity object</param>
        public virtual void MemberwiseCopy(BaseEntity myEntity)
        {
            base.MemberwiseCopy(myEntity);
            IsDirty = false;//ToDo: Why we need to set IsDirty to false?
        }
        #endregion Public Methods

        #region Public overrides

        //ToDo: why we need to overrides angain. we need to test it
        public static bool operator ==(BaseEntity a, BaseEntity b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(BaseEntity a, BaseEntity b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            return PropertiesAreEqual(obj);
        }
        //

        protected override bool PropertiesAreEqual(object obj)
        {
            return Id == ((BaseEntity)obj).Id;
        }

        /// <summary>
        /// Get the unique value of this entity
        /// </summary>
        /// <returns>the id of this entity</returns>
        public override int GetHashCode()
        {
            return Id;
        }
        #endregion Public overrides

        #region IDataErrorInfo Members

        /// <summary>
        /// return the error string, we combine all the errors in the _validataionErros to a string.
        /// </summary>
        public virtual string Error
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var ve in ValidationErrors.Values)
                {
                    sb.AppendLine(ve);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// return the error string with given field name
        /// </summary>
        /// <param name="columnName">given field name</param>
        /// <returns>the error string</returns>
        public virtual string this[string columnName]
        {
            //  TODO: do we want property level validation?
            get { return string.Empty; }
        }
        #endregion IDataErrorInfo Members
    }
}

