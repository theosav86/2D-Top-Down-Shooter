using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
    }

    public override void Update(PlayerController player)
    {
        if (!player.isUsing)
        {
            player.TransitionToState(player.normalState);
        }
    }
}
