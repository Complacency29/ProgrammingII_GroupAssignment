using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorUI : MonoBehaviour
{
    InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();
    }

    private void Start()
    {
        //ModuleSnapping.Generator.Instance.GenerateModules();
    }

    private void Update()
    {
        if (controls.MenuNavigation.Exit.ReadValue<float>() > .1f)
        {
            Application.Quit();
        }
    }

    public void GenerateWorldButton()
    {
        //ModuleSnapping.Generator.Instance.GenerateModules();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
