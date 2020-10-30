using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.weaponHolder.enabled = true;
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetButtonDown("Use") && player.canInteract)
        {
            player.TransitionToState(player.useState);
        }
    }
}
