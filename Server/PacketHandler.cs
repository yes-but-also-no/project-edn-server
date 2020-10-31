using System;
using GameServer.ClientPackets;
using GameServer.ClientPackets.Bridge;
using GameServer.ClientPackets.Chat;
using GameServer.ClientPackets.Game;
using GameServer.ClientPackets.Hangar;
using GameServer.ClientPackets.Inventory;
using GameServer.ClientPackets.Lobby;
using GameServer.ClientPackets.Room;
using GameServer.ClientPackets.Shop;
using GameServer.ClientPackets.Tutorial;
using Console = Colorful.Console;

namespace GameServer
{
    /// <summary>
    /// This class handles all packets as they come in
    /// </summary>
    public static class PacketHandler
    {
        /// <summary>
        /// The size of the header in bytes
        /// </summary>
        private const int HeaderSize = 3;
        
        public static ClientBasePacket HandlePacket(byte[] data, int offset, GameSession client)
        {
            // Calculate message size
            var size = (short)((data[offset + 1] << 8) + data[offset]);
            
            // Copy the packet to a new byte array
            // Skipping the header
            var packet = new byte[size];
            Array.Copy(data, offset, packet, 0, size);

            ClientBasePacket msg;
            
            // Get the id
            var id = packet[2];
            
            // Handle the packet
            // TODO: Can we group these into login / game / etc?
            switch (id)
            {
                case 0x00:
                    msg = new ProtocolVersion(packet, client);
                    break;
                
                case 0x01:
                    msg = new ValidateClient(packet, client);
                    break;
                
                case 0x03:
                    msg = new ConnectClient(packet, client);
                    break;
                
                case 0x04:
                    msg = new ConnectSwitch(packet, client);
                    break;
                
                case 0x05:
                    msg = new SwitchServer(packet, client);
                    break;
                
                case 0x06:
                    msg = new ServerTime(packet, client);
                    break;
                
                case 0x07:
                    msg = new Message(packet, client);
                    break;
                
                case 0x0d:
                    msg = new Log(packet, client);
                    break;
                
                case 0x0c:
                    msg = new SyncMoney(packet, client);
                    break;
                
                case 0x11:
                    msg = new FactoryModifyUnit(packet, client);
                    break;
                
                case 0x12:
                    msg = new FactoryModifyEnd(packet, client);
                    break;
                
                case 0x16:
                    msg = new IsValidName(packet, client);
                    break;
                
                case 0x17:
                    msg = new FactoryChangeUnitName(packet, client);
                    break;
                
                case 0x18:
                    msg = new RequestInventory(packet, client);
                    break;
                
                case 0x19:
                    msg = new RequestSearchGame(packet, client);
                    break;
                
                case 0x1b:
                    msg = new CreateGame(packet, client);
                    break;
                
                case 0x1c:
                    msg = new EnterGame(packet, client);
                    break;
                
                case 0x1f:
                    msg = new ListUser(packet, client);
                    break;
                
                case 0x20:
                    msg = new Ready(packet, client);
                    break;
                
                case 0x21:
                    msg = new Exit(packet, client);
                    break;
                
                case 0x22:
                    msg = new StartGame(packet, client);
                    break;
                
                case 0x2b:
                    msg = new SelectBase(packet, client);
                    break;
                
                case 0x2c:
                    msg = new ReadyGame(packet, client);
                    break;
                
                case 0x2e:
                    msg = new RequestPalette(packet, client);
                    break;
                
                case 0x2f:
                    msg = new MoveUnit(packet, client);
                    break;
                
                case 0x30:
                    msg = new AimUnit(packet, client);
                    break;
                
                case 0x31:
                    msg = new StartAttack(packet, client);
                    break;
                
                case 0x32:
                    msg = new StopAttack(packet, client);
                    break;
                
                case 0x35:
                    msg = new RequestRegain(packet, client);
                    break;
                
                case 0x38:
                    msg = new ModeSniper(packet, client);
                    break;
                
                case 0x39:
                    msg = new UseSkill(packet, client);
                    break;
                
                case 0x3a:
                    msg = new RequestChangeWeaponset(packet, client);
                    break;
                
                case 0x3b:
                    msg = new RequestQuitBattle(packet, client);
                    break;
                
                case 0x3f:
                    msg = new BuyList(packet, client);
                    break;
                
                case 0x40:
                    msg = new RequestGoodsData(packet, client);
                    break;
                
                case 0x46:
                    msg = new RequestAvatarInfo(packet, client);
                    break;
                
                case 0x47:
                case 0x48:
                case 0x49:
                case 0x4a:
                case 0x4b:
                case 0x4c:
                case 0x4d:
                    msg = new RequestStatsInfo(packet, client);
                    break;
                
                case 0x4e:
                    msg = new RequestBestInfo(packet, client);
                    break;
                
                case 0x57:
                    msg = new TutorialSelect(packet, client);
                    break;
                
                case 0x5a:
                    msg = new UnAimUnit(packet, client);
                    break;
                
                case 0x63:
                    msg = new MsgConnect(packet, client);
                    break;
                
                case 0x64:
                    msg = new MsgUserStateInfo(packet, client);
                    break;
                
                case 0x66:
                    msg = new MsgUserClanInfo(packet, client);
                    break;
                
                case 0x67:
                    msg = new MsgGetBuddyList(packet, client);
                    break;
                
                case 0x69:
                    msg = new MsgGetChannelList(packet, client);
                    break;
                
                case 0x6a:
                    msg = new MsgJoinChannel(packet, client);
                    break;
                
                case 0x6b:
                    msg = new MsgLeaveChannel(packet, client);
                    break;
                
                case 0x6c:
                    msg = new MsgChannelChatting(packet, client);
                    break;
                
                case 0x71:
                    msg = new RequestOperator(packet, client);
                    break;
                
                case 0x72:
                    msg = new SelectOperator(packet, client);
                    break;
                
                default:
                    msg = new UnknownPacket(packet, client);
                    //Console.WriteLine("Unknown packet id [{0}] from user {1}", id, client.GetUserName());
                    break;
            }

            return msg;
        }
    }
}