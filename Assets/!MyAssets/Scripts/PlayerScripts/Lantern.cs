using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    private void Awake()
    {
        inventory = GetComponentInParent<PlayerInventory>();
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine == false)
            return;
        view.RPC("LanternRPC", RpcTarget.All);
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
}
