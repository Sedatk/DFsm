using System;
using System.IO;

namespace DFsm.Infrastructure
{
    static class Extensions
    {
        public static void WriteGuid(this BinaryWriter bw, Guid value)
        {
            var arr = value.ToByteArray();
            bw.Write(arr.Length);
            bw.Write(arr);
        }

        public static Guid ReadGuid(this BinaryReader br)
        {
            return new Guid(br.ReadBytes(br.ReadInt32()));
        }
    }
}
