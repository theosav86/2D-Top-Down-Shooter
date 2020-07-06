using System;


//PUBLISHER SUBSCRIBER PATTERN
//ALL WEAPONS ARE PUBLISHERS
//FOR NOW ONLY THE HUDCONTROLLER CLASS IS A SUBSCRIBER
public class ReloadWeaponBroker
{
    public static event Action WeaponIsReloading;

    public static event Action WeaponFinishedReloading;

    public static void CallWeaponIsReloading()
    {
        if(WeaponIsReloading != null)
        {
            WeaponIsReloading();
        }
    }
    public static void CallWeaponFinishedReloading()
    {
        if (WeaponIsReloading != null)
        {
            WeaponFinishedReloading();
        }
    }

}
