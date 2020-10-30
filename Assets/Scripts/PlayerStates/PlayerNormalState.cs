using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
    }

    public override void Update(PlayerController player)
    { 
        if(player.isUsing)
        {
            player.TransitionToState(player.useState);
        }
    }
}
