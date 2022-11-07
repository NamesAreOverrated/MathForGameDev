using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Lecture_2.Assignment_5_TurretTrigger
{

    public class TurretTrigger : MonoBehaviour
    {
        [SerializeField]
        Transform _turretBarrel;
        [SerializeField]
        Transform _target;
        [SerializeField]
        float _radius;
        [SerializeField]
        float _height;
        [SerializeField]
        float _range;

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(_turretBarrel.transform.position, _turretBarrel.transform.position + _turretBarrel.forward);

            float magnitude = (_turretBarrel.transform.position - _target.transform.position).magnitude;
            Vector3 localPos = _turretBarrel.transform.InverseTransformPoint(_target.transform.position);
            float forwardDot = Vector3.Dot(_turretBarrel.forward, localPos);
            Gizmos.color = Color.white;
            Handles.Label(_target.transform.position + new Vector3(0, 0.1f, 0), magnitude.ToString());
            if (forwardDot > 0 && magnitude <= _range)
            {
                Gizmos.color = Color.blue;
                float heightDot = Vector3.Dot(_turretBarrel.up, localPos);
                Handles.Label(_target.transform.position + new Vector3(0, 0.2f, 0), heightDot.ToString());
                if (heightDot <= _height && heightDot >= -_height)
                {
                    Gizmos.color = Color.green;
                    float radiusDot = Vector3.Dot(_turretBarrel.right, localPos);

                    Handles.Label(_target.transform.position + new Vector3(0, 0.3f, 0), radiusDot.ToString());
                    if (radiusDot <= _radius && radiusDot >= -_radius)
                    {

                        Gizmos.color = Color.red;
                    }
                }

            }
            Gizmos.DrawSphere(_target.transform.position, 0.05f);

        }
        
    }

    

}
