using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorUI : MonoBehaviour
{
    private void Start()
    {
        ModuleSnapping.Generator.Instance.GenerateModules();
    }

    public void GenerateWorldButton()
    {
        ModuleSnapping.Generator.Instance.GenerateModules();
    }
}
