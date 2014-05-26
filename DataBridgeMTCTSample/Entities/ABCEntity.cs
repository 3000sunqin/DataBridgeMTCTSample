using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBridgeBaseLib.Entities;

namespace DataBridgeMTCTSample.Entities
{
    /// <summary>
    /// The entity represents one row of ABCEntity table
    /// </summary>
    public class ABCEntity : BaseEntity
    {
        #region Properties
        string _a;

        public virtual string A
        {
            get { return _a; }
            set 
            {
                if (_a != value)
                {
                    IsDirty = true;
                    _a = value;
                    OnPropertyChanged("A");
                }
            }
        }


        string _b;
        public virtual string B
        {
            get { return _b; }
            set
            {
                if (_b != value)
                {
                    IsDirty = true;
                    _b = value;
                    OnPropertyChanged("B");
                }
            }
        }
        double _c;
        public virtual double C
        {
            get { return _c; }
            set
            {
                IsDirty = true;
                _c = value;
                OnPropertyChanged("C");
            }
        }
        #endregion

        #region Override
        protected override bool PropertiesAreEqual(object obj)
        {
            if (base.PropertiesAreEqual(obj))
            {
                ABCEntity obj1 = obj as ABCEntity;
                return obj1.A == this.A &&
                    obj1.B == this.B &&
                    obj1.C == this.C;
            }
            else
                return false;
        }
        #endregion
    }
}
