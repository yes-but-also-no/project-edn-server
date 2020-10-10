namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Contains the users messenger buddies
    /// </summary>
    public class MsgBuddyList : ServerBasePacket
    {
        public override string GetType()
        {
            return "MSG_BUDDY_LIST";
        }

        public override byte GetId()
        {
            return 0xd4;
        }

        protected override void WriteImpl()
        {
            // Test data below
            
            WriteInt(1); // List size
            
            // Item
            WriteInt(999999); // UserId
            WriteString("DatBoi"); // Name
            WriteInt(1); // Rank
            
            // States:
            // 0 Offline
            // 1 Lobby [Chan Name]
            // 2 In Battle
            // 3 Individual Maintenance?
            // 4 Operator
            // 5 Shop
            // 6 Hangar
            // 7 Options
            // 8 Tutorial
            // 9 Training
            // 10+ Unknown
            WriteByte(1); // State? 0 offline 1 lobby 2 battle 3 maintenance? 4 operator 5 shop 6 hangar 7
            WriteString("IDONTEXIST");
            
            WriteInt(0); // ClanId
            WriteBool(false); // Is master
            WriteString(""); // No clan
        }
    }
}