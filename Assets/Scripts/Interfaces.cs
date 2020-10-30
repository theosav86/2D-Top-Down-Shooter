using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    void UseInteractable();

    void StopUseInteractable();
}

public interface IDamagable
{
    void TakeDamage(int damageValue);
}

