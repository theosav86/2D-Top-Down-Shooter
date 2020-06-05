using System;

public class ShieldBroker
{
    public static event Action ShieldIsEnabled;

    public static event Action ShieldIsDisabled;

    public static event Action ShieldIsBurning;

    public static event Action<float> ShieldTookDamage;

    public static void CallShieldIsEnabled()
    {
        if(ShieldIsEnabled != null)
        {
            ShieldIsEnabled();
        }
    }

    public static void CallShieldIsDisabled()
    {
        if (ShieldIsDisabled != null)
        {
            ShieldIsDisabled();
        }
    }

    public static void CallShieldIsBurning()
    {
        if(ShieldIsBurning != null)
        {
            ShieldIsBurning();
        }
    }

    public static void CallShieldTookDamage(float damageValue)
    {
        if(ShieldTookDamage != null)
        {
            ShieldTookDamage(damageValue);
        }
    }
}
