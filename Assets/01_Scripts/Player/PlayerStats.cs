using UnityEngine;

public static class PlayerStats
{
    [Range(1,9)] private static int _playerHealth = 5;
    [Range(1,15)] private static int _playerDamage = 1;
    [Range(6,10)] private static int _playerSpeed = 6;
    
    public static int SeekDamage    {
        get => _playerDamage;
        set => _playerDamage = value;
    }
    public static int SeekHealth    {
        get => _playerHealth;
        set => _playerHealth = value;
    }
    public static int SeekSpeed    {
        get => _playerSpeed;
        set => _playerSpeed = value;
    }
    
    public static void StatsReset()
    {
        _playerHealth = 5;
        _playerDamage = 1;
        _playerSpeed = 6;
    }
}
