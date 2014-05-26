using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataBridgeBaseLib.Entities
{
    /// <summary>
    /// A generic collection class which can be retrieved by index and fire events when the collection is changed.
    /// </summary>
    /// <typeparam name="T">any type</typeparam>
    public class ObservableKeyedCollection<T> : ObservableCollection<T>
    {
        #region Member Variables
        private Dictionary<int, int> _myDictionary = null;
        #endregion Member Variables

        #region Constructors
        public ObservableKeyedCollection()
        {
            _myDictionary = new Dictionary<int, int>();
        }

        /// <summary>
        /// Create this object from List(T) by add the list contents
        /// </summary>
        /// <param name="collection">the list of objects</param>
        public ObservableKeyedCollection(List<T> collection)
        {
            _myDictionary = new Dictionary<int, int>();
            foreach (T myObj in collection)    //TODO: Why do we just add items to the base collection and let _myDicionary empty?
            {
                base.Add(myObj);
            }
        }

        /// <summary>
        /// Create this object from IEnumerable(T) by add the list contents
        /// </summary>
        /// <param name="collection">the list of objects</param>
        public ObservableKeyedCollection(IEnumerable<T> collection)
        {
            _myDictionary = new Dictionary<int, int>();
            foreach (T myObj in collection)
            {
                base.Add(myObj);
            }
        }
        #endregion Constructors

        #region IList Members

        /// <summary>
        /// Overwrite the ancestor's methods
        /// </summary>
        /// <param name="item">the item object</param>
        /// <returns>-1: failed to add, return index after succeed to add item</returns>
        public new int Add(T item)
        {
            if (item == null)
            {
                return -1;
            }
            else
            {
                int nIndex = base.Count;

                if (Contains(item))
                {
                    throw new ApplicationException(item.ToString());
                }
                else
                {
                    //Key is the hash code of the item, value is the index of item in the base collection
                    _myDictionary.Add(item.GetHashCode(), nIndex);
                }

                // Add to the internal list
                base.Add(item);

                return nIndex;
            }
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public new void Clear()
        {
            _myDictionary.Clear();
            base.Clear();
        }

        public virtual bool Contains(int nKey)
        {
            return _myDictionary.ContainsKey(nKey);
        }

        public virtual bool Contains(object item)
        {
            if (item == null)
            {
                return false;
            }
            else
            {
                return _myDictionary.ContainsKey(item.GetHashCode());
            }
        }

        public virtual int IndexOf(int nKey)
        {
            return _myDictionary[nKey];
        }

        public virtual int IndexOf(object item)
        {
            if (item == null)
            {
                return -1;
            }
            else
            {
                return _myDictionary[item.GetHashCode()];
            }
        }

        public new void Insert(int nIndex, T item)
        {
            if (item == null)
            {
                return;
            }

            // Since we can't iterate over the keys directly
            List<int> myKeys = new List<int>(_myDictionary.Keys);
            foreach (int myKey in myKeys)
            {
                int nPosition = _myDictionary[myKey];
                if (nPosition >= nIndex)
                {
                    // Increment the index for all subsequent items > the position we are inserting at
                    _myDictionary[myKey] = nPosition + 1;
                }
            }

            _myDictionary.Add(item.GetHashCode(), nIndex);
            base.Insert(nIndex, item);
        }

        public virtual void Remove(int nKey)
        {
            RemoveAt(_myDictionary[nKey]);
        }

        public virtual void Remove(object item)
        {
            if (item == null)
            {
                return;
            }

            RemoveAt(IndexOf(item));
        }

        public new void RemoveAt(int nIndex)
        {
            bool bFound = false;
            int nHashCode = 0;

            // Since we can't iterate over the keys directly
            List<int> myKeys = new List<int>(_myDictionary.Keys);
            foreach (int myKey in myKeys)
            {
                int nPosition = _myDictionary[myKey];
                if (nPosition == nIndex)
                {
                    nHashCode = myKey;
                    bFound = true;
                }

                // Decrement the index for all subsequent items > the position we are removing at
                else if (nPosition > nIndex)
                {
                    _myDictionary[myKey] = nPosition - 1;
                }
            }

            if (bFound)
            {
                _myDictionary.Remove(nHashCode);

                object myObj = base[nIndex];
                base.RemoveAt(nIndex);
            }
        }
        #endregion IList Members


        public virtual T GetByKey(int nKey)
        {
            return base[_myDictionary[nKey]];
        }

        public virtual void SetByKey(int nKey, T value)
        {
            base[_myDictionary[nKey]] = value;
        }

        public virtual void ChangeItemKey(int nOldKey, int nNewKey)
        {
            int nIndex = _myDictionary[nOldKey];
            _myDictionary.Remove(nOldKey);
            _myDictionary.Add(nNewKey, nIndex);
        }
    }
}