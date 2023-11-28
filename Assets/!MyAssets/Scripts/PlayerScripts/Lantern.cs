using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

/// <summary>
/// This script will be placed on the lantern, which is a child object of the player
/// </summary>
public class Lantern : MonoBehaviour
{
    [SerializeField] AnimationCurve oilLightCurve;
    [SerializeField, Range(0, 5)] float oilBurnRate = 1f;
    [SerializeField, Range(1, 5)] float maxLanternBrightness = 3f;
    //[SerializeField, Range(0, .5f)] float flickerIntesity = .2f;
    [SerializeField] Light lanternLight;

    bool lanternIsOn = true;
    PlayerInventory inventory;
    PhotonView view;
    InputMaster controls;

    private void Awake()
    {
        inventory = GetComponentInParent<PlayerInventory>();
        view = GetComponent<PhotonView>();

        if(view.IsMine)
        {
            controls = new InputMaster();
            controls.PlayerActions.UseOilRefill.performed += context => UseOilRefill();
            controls.PlayerActions.ToggleLamp.performed += context => ToggleLamp();
        }
    }

    private void ToggleLamp()
    {
        //if this is not our photon view, do nothing
        if (view.IsMine == false)
            return;

        if(lanternIsOn)
        {
            //lantern is on, turn it off
            lanternIsOn = false;
            lanternLight.enabled = false;
        }
        else
        {
            //lantern is off, turn it on
            lanternIsOn = true;
            lanternLight.enabled = true;
        }
    }

    private void Update()
    {
        if (view.IsMine == false)
            return;
        view.RPC("LanternRPC", RpcTarget.All);
    }

    public void UseOilRefill()
    {
        view.RPC("OilRefillRPC", RpcTarget.All);
    }
    [PunRPC]
    private void OilRefillRPC()
    {
        //check if we have an oil refill
        if (inventory.CurOilRefill > 0)
        {
            inventory.CurOilRefill--;
            inventory.CurLampOil += inventory.OilRefillAmount;
            if (inventory.CurLampOil > inventory.MaxLampOil)
            {
                inventory.CurLampOil = inventory.MaxLampOil;
            }
        }
        else
        {
            Debug.Log("Player has no oil refills.");
        }
    }

    [PunRPC]
    private void LanternRPC()
    {
        if (lanternIsOn)
        {
            //the lamp is on, so we need to burn oil
            //first check if we have oil
            if (inventory?.CurLampOil > 0)
            {
                //set the light level based on the percentage of remaining oil
                float oilLevelPercentage = (float)(inventory.CurLampOil / inventory.MaxLampOil);
                float lightLevelPercentage = oilLightCurve.Evaluate(oilLevelPercentage);
                lanternLight.intensity = lightLevelPercentage * maxLanternBrightness;

                //adjust the brightness based on the flicker intensity
                //float flickerAdjustment = Random.Range(-flickerIntesity, flickerIntesity);
                //lanternLight.intensity += flickerAdjustment;

                //burn some oil based on our set oil burn rate
                inventory.CurLampOil -= oilBurnRate * Time.deltaTime;
            }
        }
    }
    private void OnEnable()
    {
        if (view.IsMine == false)
            return;

        controls.Enable();
    }
    private void OnDisable()
    {
        if (view.IsMine == false)
            return;

        controls.PlayerActions.UseOilRefill.performed -= context => UseOilRefill();
        controls.PlayerActions.ToggleLamp.performed -= context => ToggleLamp();
        controls.Disable();
    }
}
