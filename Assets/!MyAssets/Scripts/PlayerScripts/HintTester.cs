using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTester : MonoBehaviour
{
    [SerializeField] string hint1Text;
    [SerializeField] int hint1Priority;

    [SerializeField] string hint2Text;
    [SerializeField] int hint2Priority;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerHints>().AddHint(hint1Text, hint1Priority);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerHints>().AddHint(hint2Text, hint2Priority);
        }
    }
}
