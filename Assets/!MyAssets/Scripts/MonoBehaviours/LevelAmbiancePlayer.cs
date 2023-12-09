using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAmbiancePlayer : MonoBehaviour
{
    [SerializeField] AudioClip ambianceClip;
    [SerializeField, Range(0,1)] float volume = .75f;

    private void Start()
    {
        AudioManager.instance.PlayBGM(ambianceClip, 2f, true, volume);
    }
}
