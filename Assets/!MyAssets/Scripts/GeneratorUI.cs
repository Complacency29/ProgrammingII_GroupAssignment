using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorUI : MonoBehaviour
{
    public void GenerateWorldButton()
    {
        ModuleSnapping.Generator.Instance.GenerateModules();
    }
}
