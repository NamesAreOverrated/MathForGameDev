using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lecture_2.Assignment_4_TurretAlignment
{
    public class TurretAlignment : MonoBehaviour
    {
        [SerializeField]
        Transform _targetTransform;
        [SerializeField]
        Transform _turretTransform;

        private void OnDrawGizmos()
        {

            Ray ray = new Ray(_targetTransform.transform.position, _targetTransform.forward);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(ray);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(raycastHit.point, raycastHit.point + raycastHit.normal);

                Vector3 zAxis = (Vector3.Cross(_targetTransform.right, raycastHit.normal)).normalized;
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(raycastHit.point, raycastHit.point + zAxis);

                _turretTransform.transform.position = raycastHit.point;
                _turretTransform.rotation = Quaternion.LookRotation(zAxis, raycastHit.normal);


            }



        }
    }

}