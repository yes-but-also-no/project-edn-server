using System.Numerics;

namespace GameServer.Model.Results
{
    /// <summary>
    /// Stores data about the result of a weapon hit
    /// </summary>
    public class HitResult
    {
        public int Damage { get; set; }
        public int VictimId { get; set; }
        public HitResultCode ResultCode { get; set; }
        public Vector3 PushBack { get; set; } = Vector3.Zero;

        public static HitResult Miss = new HitResult
        {
            Damage = 0,
            VictimId = -1,
            ResultCode = HitResultCode.Miss,
            PushBack = Vector3.Zero
        };
    }

    public enum HitResultCode : byte
    {
        Unknown = 0x00,
        Hit = 0x01,
        Miss = 0x02
    }
}