using System.Linq;
using GameServer.Game;

namespace GameServer.ServerPackets.Game
{
    public class ScoreUpdate : ServerBasePacket
    {
        private readonly GameInstance _game;

        public ScoreUpdate(GameInstance game)
        {
            _game = game;
        }
        
        public override string GetType()
        {
            return "SCORE_UPDATE";
        }

        public override byte GetId()
        {
            return 0x7e;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            
            WriteInt(_game.RoomInstance.Users.Count());

            foreach (var user in _game.RoomInstance.Users)
            {
                WriteInt(user.Id);
                WriteUInt(user.Team);
                WriteString(user.Callsign);
                
                WriteInt(user.Scores.Kills); // Kills
                WriteInt(user.Scores.Deaths); // Deaths
                WriteInt(user.Scores.Assists); // Assists
                WriteInt(user.Scores.Points); // Points
                WriteInt(5);
                WriteInt(user.Scores.Credits); // Credits
                WriteInt(7);
                WriteInt(8);

                WriteByte(0);
                WriteByte(0);
                WriteByte(0);
                WriteByte(0);
                WriteByte(0);
                WriteByte(0);
                WriteByte(0);
                WriteByte(0);
                WriteByte(0);
            }
        }
    }
}