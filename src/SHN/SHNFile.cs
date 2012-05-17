using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Threading;


namespace MapleShark.SHN
{
    public delegate void DOnSaveFinished(SHNFile file);
    public delegate void DOnSaveError(SHNFile file, string error);

    public class SHNFile : DataTable
    {
        public static Encoding Encoding = Encoding.UTF8;

        public string FileName { get; private set; }
        public uint Header { get; private set; }
        public uint RecordCount { get; private set; }
        public uint ColumnCount { get; private set; }
        public uint DefaultRecordLength { get; private set; }

        private byte[] CryptHeader;

        public SHNFile(string pPath)
        {
            this.FileName = pPath;
            Load();
        }

        public SHNFile()
        {

        }

        public void Load()
        {
            if (!File.Exists(FileName))
            {
                throw new FileNotFoundException(string.Format("Could not find SHN File {0}.", FileName));
            }
            byte[] data;
            using (var file = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryReader reader = new BinaryReader(file);
                CryptHeader = reader.ReadBytes(32);
                int Length = reader.ReadInt32() - 36; //minus int + header
                data = reader.ReadBytes(Length);
                FileCrypto.Crypt(data, 0, Length);
                //File.WriteAllBytes("Output.dat", data); //debug purpose
            }

            using (var stream = new MemoryStream(data))
            {
                SHNReader reader = new SHNReader(stream);
                this.Header = reader.ReadUInt32();
                this.RecordCount = reader.ReadUInt32();
                this.DefaultRecordLength = reader.ReadUInt32();
                this.ColumnCount = reader.ReadUInt32();
                GenerateColumns(reader);
                GenerateRows(reader);
            }
            data = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) CryptHeader = null;
            base.Dispose(disposing);
        }

        private void UpdateDefaultRecordLength()
        {
            uint len = 2;
            for (int i = 0; i < base.Columns.Count; ++i)
            {
                SHNColumn col = (SHNColumn)base.Columns[i];
                len += (uint)col.Length;
            }
            this.DefaultRecordLength = len;
        }

        public void GenerateRows(SHNReader reader)
        {
            object[] values = new object[this.ColumnCount];
            for (uint i = 0; i < RecordCount; ++i)
            {
                uint RowLength = reader.ReadUInt16();
                for (int j = 0; j < this.ColumnCount; ++j)
                {
                    switch (((SHNColumn)this.Columns[j]).TypeByte)
                    {
                        case 1:
                        case 12:
                        case 16:
                            values[j] = reader.ReadByte();
                            break;
                        case 2:
                            values[j] = reader.ReadUInt16();
                            break;
                        case 3:
                        case 11:
                        case 18:
                        case 27:
                            values[j] = reader.ReadUInt32();
                            break;
                        case 5:
                            values[j] = reader.ReadSingle();
                            break;
                        case 9:
                        case 24:
                            values[j] = reader.ReadPaddedString(((SHNColumn)this.Columns[j]).Length);
                            break;
                        case 13:
                        case 21:
                            values[j] = reader.ReadInt16();
                            break;
                        case 20:
                            values[j] = reader.ReadSByte();
                            break;
                        case 22:
                            values[j] = reader.ReadInt32();
                            break;
                        case 26:       // TODO: Should be read until first null byte, to support more than 1 this kind of column
                            values[j] = reader.ReadPaddedString((int)(RowLength - DefaultRecordLength + 1));
                            break;
                        default:
                            throw new Exception("New column type found");
                    }
                }
                base.Rows.Add(values);
            }
        }

        public void GenerateColumns(SHNReader reader)
        {
            int unkcolumns = 0;
            int Length = 2;
            for (int i = 0; i < ColumnCount; ++i)
            {
                SHNColumn col = new SHNColumn();
                col.Load(reader, ref unkcolumns);
                Length += col.Length;
                base.Columns.Add(col);
            }
            if (Length != DefaultRecordLength)
            {
                throw new Exception("Default record Length does not fit.");
            }
        }
    }

}
