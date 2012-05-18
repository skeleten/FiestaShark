using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapleShark.SHN.Datas
{
    public sealed class ItemInfo
    {
        public ushort ItemID { get; private set; }
        public bool TwoHand { get; private set; }
        public string InxName { get; private set; }
        public byte MaxLot { get; private set; }
        public ushort AttackSpeed { get; private set; }
        public byte Level { get; private set; }
        public byte Type { get; private set; }
        public byte Class { get; private set; }
        public byte EquipType { get; private set; }
        public byte UpgradeLimit { get; private set; }
        public byte Jobs { get; private set; }
        public ushort MinMagic { get; private set; }
        public ushort MaxMagic { get; private set; }
        public ushort MinMelee { get; private set; }
        public ushort MaxMelee { get; private set; }
        public ushort WeaponDef { get; private set; }
        public ushort MagicDef { get; private set; }

        //item upgrade
        public ushort UpSucRation { get; private set; }
        public ushort UpResource { get; private set; }

        /// <summary>
        /// Needs serious fixing in the reader, as it throws invalid casts (files all use uint, but fuck those)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ItemInfo Load(DataTableReaderEx reader)
        {
            ItemInfo itemInfo = new ItemInfo
            {
                ItemID = reader.GetUInt16("id"),
                EquipType = (byte)reader.GetUInt32("equip"),
                InxName = reader.GetString("inxname"),
                MaxLot = (byte)reader.GetUInt32("maxlot"),
                AttackSpeed = (ushort)reader.GetUInt32("atkspeed"),
                Level = (byte)reader.GetUInt32("demandlv"),
                Type = (byte)reader.GetUInt32("type"),
                Class = (byte)reader.GetUInt32("class"),
                UpgradeLimit = reader.GetByte("uplimit"),
                Jobs = (byte)reader.GetUInt32("whoequip"),
                TwoHand = reader.GetBoolean("TwoHand"),
                MinMagic = (ushort)reader.GetUInt32("minma"),
                MaxMagic = (ushort)reader.GetUInt32("maxma"),
                MinMelee = (ushort)reader.GetUInt32("minwc"),
                MaxMelee = (ushort)reader.GetUInt32("maxwc"),
                WeaponDef = (ushort)reader.GetUInt32("ac"),
                MagicDef = (ushort)reader.GetUInt32("mr"),
                UpSucRation = reader.GetUInt16("UpSucRatio"),
                UpResource = reader.GetByte("UpResource")
            };
            return itemInfo;
        }
    }
}