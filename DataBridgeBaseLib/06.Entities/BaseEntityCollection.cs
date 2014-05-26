using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace DataBridgeBaseLib.Entities
{
    /// <summary>
    /// This class helps us to get notification on two dimension, collection changed and item properties changed
    /// </summary>
    public class BaseEntityCollection : ObservableKeyedCollection<BaseEntity>, INotifyPropertyChanged
    {
        #region Public Methods
        public BaseEntityCollection()
        {
        }

        public BaseEntityCollection(List<BaseEntity> collection)
            : base(collection)
        {
            foreach (BaseEntity entity in collection)
            {
                entity.IsDirty = false;
                Add(entity);
            }
        }

        public BaseEntityCollection(IEnumerable<BaseEntity> collection)
        {
            foreach (BaseEntity entity in collection)
            {
                entity.IsDirty = false;
                Add(entity);
            }
        }
        #endregion Public Methods

        #region IList Members
        public new int Add(BaseEntity item)
        {
            item.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            return base.Add(item);
        }

        public new void Clear()
        {
            foreach (BaseEntity myEntity in this)
            {
                myEntity.PropertyChanged -= OnPropertyChanged;
            }
            base.Clear();
        }

        public new void Insert(int index, BaseEntity item)
        {
            item.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            base.Insert(index, item);
        }

        public override void Remove(object item)
        {
            BaseEntity myEntity = item as BaseEntity;
            myEntity.PropertyChanged -= OnPropertyChanged;
            base.Remove(item);
        }

        public new void RemoveAt(int nIndex)
        {
            BaseEntity myEntity = base[nIndex];
            myEntity.PropertyChanged -= OnPropertyChanged;
            base.RemoveAt(nIndex);
        }
        #endregion IList Members

        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BaseEntity myEntity = (BaseEntity)sender;

            // If the Id changed it was saved in the database
            if (e.PropertyName == "Id")
            {
                // Update the Id from 0 to the new value
                ChangeItemKey(0, myEntity.Id);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, myEntity, myEntity, IndexOf(myEntity.Id)));
        }
    }
}
