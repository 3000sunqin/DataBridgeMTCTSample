using System;
using System.Linq;

namespace DataBridgeBaseLib.Entities
{
    /// <summary>
    /// the Class helps us to Get the row version of an entity object
    /// </summary>
    public static class RowVersionUtil
    {

        /// <summary>
        /// Get the row version of an entity object.
        /// </summary>
        /// <param name="item">the entity object which we need to get row version</param>
        /// <returns>the row version</returns>
        public static ulong GetRowVersion(BaseEntity item)
        {
            return GetRowVersion(item.RowVersion);
        }


        /// <summary>
        /// Get the row version of an entity object.
        /// </summary>
        /// <param name="rowVersion">the byte array style of a row version</param>
        /// <returns>the row version</returns>
        public static ulong GetRowVersion(byte[] rowVersion)
        {
            byte[] myBytes;
            if (BitConverter.IsLittleEndian)
            {
                myBytes = ReverseBytes(rowVersion);
            }
            else
            {
                myBytes = rowVersion;
            }

            ulong version = BitConverter.ToUInt64(myBytes, 0);
            return version;

        }

        /// <summary>
        /// Get the maxium row version from a list of entity.
        /// </summary>
        /// <param name="ItemList">the list of entity</param>
        /// <returns>the maxium row version</returns>
        public static ulong GetMaxRowVersion(BaseEntityCollection ItemList)
        {
            ulong maxRowVersion = 0;

            foreach (BaseEntity item in ItemList)
            {
                ulong version = GetRowVersion(item);
                if (version > maxRowVersion)
                {
                    maxRowVersion = version;
                }
            }

            return maxRowVersion;
        }

        /// <summary>
        /// Reverse a byte array
        /// </summary>
        /// <param name="inArray">the byte array we need to reverse</param>
        /// <returns>reversed byte array</returns>
        private static byte[] ReverseBytes(byte[] inArray)
        {
            byte[] myBytes = new byte[8];
            Buffer.BlockCopy(inArray, 0, myBytes, 0, 8);

            byte temp;
            int highCtr = myBytes.Length - 1;

            for (int ctr = 0; ctr < myBytes.Length / 2; ctr++)
            {
                temp = myBytes[ctr];
                myBytes[ctr] = myBytes[highCtr];
                myBytes[highCtr] = temp;
                highCtr -= 1;
            }
            return myBytes;
        }
    }
}
