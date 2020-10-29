using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StoreManager : MonoBehaviour, IInteractable
{
    //Stock or Unlimited supply

    public GameObject storeHudPanel;

    public TextMeshProUGUI pistolMagCountText;
    public TextMeshProUGUI smgMagCountText;
    public TextMeshProUGUI rocketMagCountText;

    private int weaponIndexToUpdate;

    private void Start()
    {
        weaponIndexToUpdate = 0;
        AmmoDisplayBroker.UpdateMagazinesOnHud += UpdateMagazinesOnStore;
    }


    public void UpdateMagCountForWeaponIndex(int gunIndex)
    {
        weaponIndexToUpdate = gunIndex;
    }

    private void UpdateMagazinesOnStore(int currentMagCount)
    {
        switch (weaponIndexToUpdate)
        {
            case 0:
                pistolMagCountText.text = currentMagCount.ToString("D2");
                break;
            case 1:
                smgMagCountText.text = currentMagCount.ToString("D2");
                break;
            case 2:
                rocketMagCountText.text = currentMagCount.ToString("D2");
                break;
            default:
                break;
        }
    }

    public void UseInteractable()
    {
        Debug.Log("Store Used");

        storeHudPanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void StopUseInteractable()
    {
        Time.timeScale = 1;
    }

    
}
