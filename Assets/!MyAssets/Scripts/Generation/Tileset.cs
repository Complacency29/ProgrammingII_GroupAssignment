using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModuleSnapping
{
    [CreateAssetMenu(menuName = "Module Snapping/Tileset")]
    public class Tileset : ScriptableObject
    {
        public GameObject[] startingModules;
        public GameObject[] allModules;
        public GameObject[] endModules;
    }
}
