using Colorful;
using GameServer.Model.Units;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when the client wants to swap weapons
    /// </summary>
    public class RequestChangeWeaponset : ClientGameBasePacket
    {
        /// <summary>
        /// The weaponset they wish to switch to
        /// </summary>
        private readonly WeaponSetIndex _desiredWeaponset;
        
        public RequestChangeWeaponset(byte[] data, GameSession client) : base(data, client)
        {
            _desiredWeaponset = (WeaponSetIndex) GetInt();
        }

        public override string GetType()
        {
            return "REQ_CHANGE_WEAPONSET";
        }

        protected override void RunImpl()
        {
            Unit?.TrySwitchWeapons(_desiredWeaponset);
        }
    }
}