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

    private void Start()
    {
        AmmoDisplayBroker.UpdateMagazinesOnStore += UpdateMagazinesOnStore;
    }

    private void UpdateMagazinesOnStore(int gunIndex, int currentMagCount)
    {
        switch (gunIndex)
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
        storeHudPanel.gameObject.SetActive(true);
    }

    //Shuts down Store
    //Turns off the Store Panel on the HUD
    public void StopUseInteractable()
    {
        storeHudPanel.gameObject.SetActive(false);
    }

}
