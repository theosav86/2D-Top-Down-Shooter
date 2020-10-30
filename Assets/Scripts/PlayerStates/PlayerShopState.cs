using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShopState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.weaponHolder.enabled = false;
        player.Use();
    }

    public override void Update(PlayerController player)
    {
        if(Input.GetButtonDown("Use"))
        {
            if(player.item != null)
            {
                Debug.LogError("Interactable is " + player.item);

                player.item.StopUseInteractable();
            }

            player.TransitionToState(player.normalState);
        }
    }
}
