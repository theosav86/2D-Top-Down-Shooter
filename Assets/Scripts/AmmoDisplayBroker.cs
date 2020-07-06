using System;

public class AmmoDisplayBroker
{
    public static event Action<int, int> UpdateAmmoOnHud; // Update current bullets in magazine on hud. The HUDController subscribes to all these events.

    public static event Action<int> UpdateMagazinesOnHud; //Update how many many magazines left.

    public static event Action CriticalAmmoOnHUD; // if current ammo in magazine = 1/3 * Total magazine capacity then make the numbers RED on hud. // The rocket launcher invokes the event when total rockets is less than 3.

    
    public static void CallUpdateAmmoOnHud(int ammoInMagazineValue, int magazineSize)
    {
        if(UpdateAmmoOnHud != null)
        {
            UpdateAmmoOnHud(ammoInMagazineValue, magazineSize);
        }
    }

    public static void CallUpdateMagazinesOnHud(int magazinesLeftValue)
    {
        if (UpdateAmmoOnHud != null)
        {
            UpdateMagazinesOnHud(magazinesLeftValue);
        }
    }

    public static void CallCriticalAmmoOnHud()
    {
        if (CriticalAmmoOnHUD != null)
        {
            CriticalAmmoOnHUD();
        }
    }
}
