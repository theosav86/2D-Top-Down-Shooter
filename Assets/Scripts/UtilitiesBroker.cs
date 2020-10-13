using System;

public class UtilitiesBroker
{
    #region SHIELD EVENTS
    public static event Action ShieldIsEnabled;

    public static event Action ShieldIsDisabled;

    public static event Action<float> ShieldIsBurning;

    public static event Action<float> ShieldTookDamage;

    public static event Action ShieldIsDepleted;

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

    public static void CallShieldIsBurning(float timeLeft)
    {
        if(ShieldIsBurning != null)
        {
            ShieldIsBurning(timeLeft);
        }
    }

    public static void CallShieldTookDamage(float damageValue)
    {
        if(ShieldTookDamage != null)
        {
            ShieldTookDamage(damageValue);
        }
    }

    public static void CallShieldIsDepleted()
    {
        if (ShieldIsDepleted != null)
        {
            ShieldIsDepleted();
        }
    }

    #endregion

    #region FLASHLIGHT EVENTS

    public static event Action<float> FlashlightIsBurning;

    public static event Action FlashlightIsDepleted;

    public static void CallFlashlightIsBurning(float batteryLeft)
    {
        if(FlashlightIsBurning != null)
        {
            FlashlightIsBurning(batteryLeft);
        }
    }

    public static void CallFlashlightIsDepleted()
    {
        if(FlashlightIsDepleted != null)
        {
            FlashlightIsDepleted();
        }
    }


    #endregion
}
