using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModuleSnapping
{
    [RequireComponent(typeof(BoxCollider))] // this will be used to determine the bounds of the room, to determine if it will fit in a location
    public class Module : MonoBehaviour
    {
        [SerializeField] private ModuleType[] moduleTypes;
        [SerializeField] private BoxCollider roomBoundsCollider;
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private bool ignoreCollision = false; // only going to be used if we have a module where it doesn't matter where it goes.

        [SerializeField] private bool _drawGizmos = true;
        [SerializeField] private Color gizmosColor = Color.red;

        private Vector3 halfSize;

        public Connection[] GetConnections { get { return GetComponentsInChildren<Connection>(); } }
        public ModuleType[] GetModuleTypes { get { return moduleTypes; } }

        public bool GetIgnoreCollision { get { return ignoreCollision; } }

        public Bounds GetBounds
        {
            get
            {
                if(roomBoundsCollider != null)
                {
                    roomBoundsCollider = GetComponent<BoxCollider>();
                }
                return roomBoundsCollider.bounds;
            }
        }

        /// <summary>
        /// Initializes the half size of the room bounds
        /// If the roomBoundsCollider is not set, it sets it to the box collider for the object
        /// </summary>

        private void InitHalfSize()
        {
            if(roomBoundsCollider == null)
            {
                roomBoundsCollider = GetComponent<BoxCollider>();
            }

            float x = roomBoundsCollider.size.x * .5f;
            float y = roomBoundsCollider.size.y * .5f;
            float z = roomBoundsCollider.size.z * .5f;

            halfSize = new Vector3(x, y, z);
        }
        /// <summary>
        /// Checks for collisions within the room bounds
        /// If ignore collision is set to true, this returns false
        /// </summary>
        /// <returns>Returns true if a collision is detected</returns>
        public bool CollisionCheck()
        {
            //If this object ignores collisions, return false
            if(ignoreCollision == true)
                return false;

            //Make sure half size is ready for use
            InitHalfSize();

            //Storing array of collisions found within the bounds of the half size
            Collider[] collidersFound = Physics.OverlapBox(transform.TransformPoint(roomBoundsCollider.center), halfSize, transform.rotation, collisionLayer);

            //Cycle through all found colliders, and if we find one that IS NOT our own, return true (collision was found)
            for (int i = 0; i < collidersFound.Length; i++)
            {
                if (collidersFound[i] != roomBoundsCollider)
                    return true;
            }

            //If we make it here, no collisions were found
            return false;
        }

        private void OnDrawGizmos()
        {
            //If draw gizmos is set to false, do nothing
            if (_drawGizmos == false)
                return;

            Gizmos.matrix = transform.localToWorldMatrix;

            //Ensure half size is ready for use
            InitHalfSize();

            //Draw a wirecube the size of the colliders bounds, of the selected color
            Gizmos.color = gizmosColor;
            //Gizmos.DrawWireCube(transform.TransformPoint(roomBoundsCollider.center), transform.rotation * roomBoundsCollider.size);

            Gizmos.DrawWireCube(roomBoundsCollider.center, roomBoundsCollider.size);
        }
    }


    /*
    [Flags]
    public enum ModuleType
    {
        None = 0,
        Room = 1,
        Hallway = 2,
        StartPoint = 4,
        ExitPoint = 8,
        MonsterStart = 16
    }
    */

    public enum ModuleType
    {
        Room,
        Hallway,
        StartPoint,
        GroundProp
    }
}