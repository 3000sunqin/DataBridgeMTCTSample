using System;
using System.ComponentModel;
using System.Reflection;

namespace DataBridgeBaseLib.Entities
{
    /// <summary>
    /// All the class are derived from this class
    /// </summary>
    [Serializable()]
    public abstract partial class BaseObject : ICloneable, INotifyPropertyChanged
    {
        #region Constructors
        public BaseObject()
        {
        }
        #endregion Constructors

        #region Properties
        private bool _bIsDirty = false;

        /// <summary>
        ///  If New or any value has changed
        /// </summary>
        public virtual bool IsDirty
        {
            get { return _bIsDirty; }
            set
            {
                if (_bIsDirty != value)
                {
                    _bIsDirty = value;
                    OnPropertyChanged("IsDirty");
                }
            }
        }

        /// <summary>
        /// For copy exposed properties from other enity with same type
        /// </summary>
        /// <param name="myEntity">the source entity object</param>
        public virtual void MemberwiseCopy(BaseObject myEntity)
        {
            // Copy the exposed properties instead
            // Only copy the public properties. Fields and Non-public properties are not included.
            PropertyInfo[] myObjectProperties = this.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in myObjectProperties)
            {
                if (propertyInfo.CanWrite)
                {
                    //ToDo: How about the collection property?
                    object value = propertyInfo.GetValue(myEntity, null);
                    propertyInfo.SetValue(this, value, null);
                }
            }

            IsDirty = false;
        }

        /// <summary>
        /// Overrided == operator
        /// </summary>
        /// <param name="a">the first object to be compared</param>
        /// <param name="b">the second object to be compared</param>
        /// <returns>Whether these two object is equal</returns>
        public static bool operator ==(BaseObject a, BaseObject b)
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

        /// <summary>
        /// Overrided != operator
        /// </summary>
        /// <param name="a">the first object to be compared</param>
        /// <param name="b">the second object to be compared</param>
        /// <returns>Whether these two object isn't equal</returns>
        public static bool operator !=(BaseObject a, BaseObject b)
        {
            return !(a == b);
        }
        #endregion Public Methods

        #region Public overrides

        /// <summary>
        /// Override the Equals method to have two object to do member compare.
        /// </summary>
        /// <param name="obj">another object to be compared</param>
        /// <returns>Whether these two object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;
            //needs the concrete class to implement how to compare their properties
            return PropertiesAreEqual(obj);
        }

        /// <summary>
        /// Abstract properties comparing method
        /// </summary>
        /// <param name="obj">another object to be compared</param>
        /// <returns>Wheter these two object are equal</returns>
        protected abstract bool PropertiesAreEqual(object obj);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion Public overrides

        #region ICloneable Members
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion ICloneable Members

        #region INotifyPropertyChanged Members
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion INotifyPropertyChanged Members
    }
}

