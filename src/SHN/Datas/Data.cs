using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleShark.SHN.Datas;
using System.IO;
using System.Windows.Forms;

namespace MapleShark.SHN.Datas
{
    public class Data
    {
        public Dictionary<ushort, ItemInfo> ItemsByID { get; private set; }
        public Dictionary<string, ItemInfo> ItemsByName { get; private set; }
        public bool ItemsLoadet { get; private set; }
        public void UnloadItems()
        {
      
            ItemsByID = null;
            ItemsByName = null;
            ItemsLoadet = false;
            MessageBox.Show("Unload ItemInfo.shn successful");
        }
        public void LoadItemInfo()
        {
            if (File.Exists(@"ItemInfo.shn"))
            {
                if (!ItemsLoadet)
                {
                    ItemsByID = new Dictionary<ushort, ItemInfo>();
                    ItemsByName = new Dictionary<string, ItemInfo>();
                    using (var file = new SHNFile(@"ItemInfo.shn"))
                    {
                        using (DataTableReaderEx reader = new DataTableReaderEx(file))
                        {

                            while (reader.Read())
                            {
                                ItemInfo info = ItemInfo.Load(reader);
                                if (ItemsByID.ContainsKey(info.ItemID) || ItemsByName.ContainsKey(info.InxName))
                                {
                                    
                                    continue;
                                }
                                ItemsByID.Add(info.ItemID, info);
                                ItemsByName.Add(info.InxName, info);
                            }
                        }
                    }
                    ItemsLoadet = true;
                    MessageBox.Show("ItemInfo Load successful");
                }
            }
            else
            {
                MessageBox.Show("ItemInfo not Found in The Directory from the Program");
            }
        }
    }
}
