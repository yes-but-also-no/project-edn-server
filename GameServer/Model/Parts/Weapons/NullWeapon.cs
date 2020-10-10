using Colorful;
using GameServer.Model.Units;
using Weapon = GameServer.Model.Parts.Weapons.Weapon;

namespace GameServer.Model.Parts.Weapons
{
    /// <summary>
    /// A dummy weapon used for the right hand when using 2h weapons
    /// </summary>
    public class NullWeapon : Weapon
    {
        public NullWeapon(Unit owner, ArmIndex arm, WeaponSetIndex weaponSet) : base(null, owner, arm, weaponSet)
        {
            // Always zero
            CurrentOverheat = 0.0f;
            IsOverheated = false;
        }

        public override void AimUnit(Unit target)
        {
            Console.WriteLine($"WARNING: {Owner} tried to call AimUnit on NullWeapon!", System.Drawing.Color.Yellow);
        }
        
        public override void UnAimUnit()
        {
            Console.WriteLine($"WARNING: {Owner} tried to call UnAimUnit on NullWeapon!", System.Drawing.Color.Yellow);
        }

        public override bool CanAttack()
        {
            Console.WriteLine($"WARNING: {Owner} tried to call Attack on NullWeapon!", System.Drawing.Color.Yellow);
            return false;
        }

        public override void OnAttack()
        {
            throw new System.NotImplementedException();
        }
    }
}