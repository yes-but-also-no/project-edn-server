namespace Data
{
    /// <summary>
    /// Ifto type codes
    /// </summary>
    public enum IfoType
    {
        ifo_none = 0,
        ifo_rocket = 1,
        ifo_missle = 2,
        ifo_sentinel_driver = 3,
        ifo_simple = 4
    }
    
    /// <summary>
    /// Weapon type codes
    /// Not all are used
    /// </summary>
    public enum WeaponType
    {
        machingun = 0,
        rifle = 1,
        cannon = 2,
        canon = 2,
        gatling = 3,
        rocket = 4,
        enggun = 5,
        shotgun = 6,
        missle = 7,
        blade = 10,
        spear = 11,
        knuckle = 12,
        spanner = 13,
        shield = 20
    }
    
    /// <summary>
    /// Game status code
    /// There is more, i just havent reversed them all
    /// </summary>
    public enum GameStatus : int
    {
        Waiting = 0,
        InPlay = 1
    }
}