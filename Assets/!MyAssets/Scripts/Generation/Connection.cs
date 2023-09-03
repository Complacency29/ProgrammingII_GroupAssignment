using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModuleSnapping
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private ModuleType[] validConnections; // Set which type of rooms this connection can connect with

        [Header("Gizmo Customization")]
        [SerializeField, Range(1, 5)] private float gizmoScale = 1f; // Sets length of the gizmo lines
        [SerializeField, Range(0, 1)] private float gizmoSphereRelativeSize = .2f; // Sets size of the gizmo sphere RELATIVE to the gizmo scale
        [SerializeField] private bool isDefault;

        public ModuleType[] GetValidConnections { get { return validConnections; } } // a getter property for the validConnections
        public bool IsDefault { get { return isDefault; } }

        private void OnDrawGizmos()
        {
            //Draw a blue line for the z axis
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * gizmoScale);

            //Draw a red line for the x axis
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * gizmoScale);
            Gizmos.DrawLine(transform.position, transform.position - transform.right * gizmoScale);

            //Draw a green line for the y axis
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * gizmoScale);

            //Draw a yellow sphere to mark the connection point
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, gizmoScale * gizmoSphereRelativeSize);
        }
    }
}
