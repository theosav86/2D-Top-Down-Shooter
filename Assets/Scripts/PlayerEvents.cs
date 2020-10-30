using System;

public class PlayerEvents
{
    //Stats Events
    public static event Action<float> PlayerTookDamage;

    public static event Action<int> PlayerRemainingHP;

    public static event Action<int> UpdatePlayerScore;

    public static event Action<int> UpdatePlayerScrap;

    public static event Action PlayerDied;

    //Interact and State Events
    public static event Action<PlayerBaseState> PlayerChangedState;




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

    public static void CallPlayerRemainingHP(int remainingHP)
    {
        if (PlayerRemainingHP != null)
        {
            PlayerRemainingHP(remainingHP);
        }
    }

    public static void CallUpdatePlayerScore(int scoreValue)
    {
        if (UpdatePlayerScore != null)
        {
            UpdatePlayerScore(scoreValue);
        }
    }

    public static void CallUpdatePlayerScrap(int scrapValue)
    {
        if (UpdatePlayerScrap != null)
        {
            UpdatePlayerScrap(scrapValue);
        }
    }

    public static void CallPlayerChangedState(PlayerBaseState newPlayerState)
    {
        if (PlayerChangedState != null)
        {
            PlayerChangedState(newPlayerState);
        }
    }
}
