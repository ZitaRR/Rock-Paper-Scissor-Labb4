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
        public bool ShouldDisconnect { get; set; }
        public int PlayerScore { get; set; }
        public int OpponentScore { get; set; }

        public byte[] ToBytes()
            => ToBytes(this);

        public static byte[] ToBytes(GameData data)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);

                writer.Write(data.PlayerName ?? "");
                writer.Write(data.OpponentName ?? "");
                writer.Write((int)data.SelectedMove);
                writer.Write(data.CanPlay);
                writer.Write(data.HasFoundGame);
                writer.Write(data.ShouldDisconnect);
                writer.Write(data.PlayerScore);
                writer.Write(data.OpponentScore);

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
                    ShouldDisconnect = reader.ReadBoolean(),
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
