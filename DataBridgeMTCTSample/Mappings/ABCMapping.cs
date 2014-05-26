using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBridgeBaseLib.Mappings;
using DataBridgeMTCTSample.Entities;

namespace DataBridgeMTCTSample.Mappings
{
    public class ABCMapping : BaseEntityMap<ABCEntity>
    {
        public ABCMapping()
            : base()
        {
            Map(x => x.A).Not.Nullable().Length(40);
            Map(x => x.B).Nullable().Length(40);
            Map(x => x.C).Not.Nullable();
        }
    
    }
}
