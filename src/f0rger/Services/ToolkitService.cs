using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace f0rger
{
    public class ToolkitService
    {
        public static Hashtable ConvertList<T>(FileMockEntityList list)
        {
            Hashtable ht = new Hashtable();
            foreach (FileMockEntity item in list)
            {
                ht.Add(item.Path, item);
            }
            return ht;
        }

        public static Hashtable ConvertList(ProfileEntityList list)
        {

            return null;
        }
    }
}
