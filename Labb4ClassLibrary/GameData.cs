using System.IO;

namespace Labb4ClassLibrary
{
    public enum Move
    {
        None = 0,
        Stone = 1,
        Scissors = 2,
        Paper = 3
    }

    public class GameData
    {
        public string PlayerName { get; set; }
        public string OpponentName { get; set; }
        public Move SelectedMove { get; set; }
        public bool CanPlay { get; set; }
        public bool HasFoundGame { get; set; }
        public int PlayerScore { get; set; }
        public int OpponentScore { get; set; }

        public byte[] ToBytes()
            => ToBytes(this);

        public static byte[] ToBytes(GameData data)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                var properties = data.GetType().GetProperties();

                foreach (var property in properties)
                {
                    if (property.GetValue(data) == null)
                        continue;

                    switch (property.PropertyType.Name.ToLower())
                    {
                        case "move":
                            writer.Write((int)property.GetValue(data));
                            break;
                        case "string":
                            writer.Write((string)property.GetValue(data));
                            break;
                        case "boolean":
                            writer.Write((bool)property.GetValue(data));
                            break;
                        case "int32":
                            writer.Write((int)property.GetValue(data));
                            break;
                    }
                }
                return stream.ToArray();
            }
        }

        public static GameData GetObject(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var reader = new BinaryReader(stream);
                return new GameData
                {
                    PlayerName = reader.ReadString(),
                    OpponentName = reader.ReadString(),
                    SelectedMove = (Move)reader.ReadInt32(),
                    CanPlay = reader.ReadBoolean(),
                    HasFoundGame = reader.ReadBoolean(),
                    PlayerScore = reader.ReadInt32(),
                    OpponentScore = reader.ReadInt32()
                };
            }
        }

        public override string ToString()
        {
            return $"{PlayerName} vs {OpponentName}\n" +
                $"Move: {SelectedMove.ToString()}\n" +
                $"Can Play: {CanPlay}\n" +
                $"Has Found Game: {HasFoundGame}\n" +
                $"Player Score: {PlayerScore}\n" +
                $"Opponent Score: {OpponentScore}";
        }
    }
}
