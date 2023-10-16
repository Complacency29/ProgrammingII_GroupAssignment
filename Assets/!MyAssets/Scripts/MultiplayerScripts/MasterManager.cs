using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singletons/Master Manager")]
public class MasterManager : ScriptableObjectSingleton<MasterManager>
{
    public string gameVersion = "0.0.0";

}
