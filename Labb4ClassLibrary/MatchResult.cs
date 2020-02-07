using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Labb4ClassLibrary
{
    public class MatchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public string Length { get; set; }
        public string Date { get; set; }

        public byte[] ToBytes()
            => ToBytes(this);

        public static byte[] ToBytes(MatchResult data)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);

                writer.Write(data.Id);
                writer.Write(data.Name);
                writer.Write(data.Result);
                writer.Write(data.Length.ToString());
                writer.Write(data.Date.ToString());

                return stream.ToArray();
            }
        }

        public static MatchResult GetObject(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var reader = new BinaryReader(stream);
                return new MatchResult
                {
                    Id = reader.ReadInt32(),
                    Name = reader.ReadString(),
                    Result = reader.ReadString(),
                    Length = reader.ReadString(),
                    Date = reader.ReadString()
                };
            }
        }
    }
}
