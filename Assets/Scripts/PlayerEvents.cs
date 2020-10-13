using System;

public class PlayerEvents
{
    public static event Action<float> PlayerTookDamage;

    public static event Action<float> PlayerRemainingHP;

    public static event Action PlayerDied;

    public static void CallPlayerTookDamage(float damageValue)
    {
        if (PlayerTookDamage != null)
        {
            PlayerTookDamage(damageValue);
        }
    }
    public static void CallPlayerDied()
    {
        if (PlayerDied != null)
        {
            CallPlayerDied();
        }
    }

    public static void CallPlayerRemainingHP(float remainingHP)
    {
        if (PlayerRemainingHP != null)
        {
            PlayerRemainingHP(remainingHP);
        }
    }
}
