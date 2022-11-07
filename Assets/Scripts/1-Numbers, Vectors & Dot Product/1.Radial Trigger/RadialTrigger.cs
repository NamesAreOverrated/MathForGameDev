using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lecture_1.Assignment_1_RadialTrigger
{
    public class RadialTrigger : MonoBehaviour
    {
        [SerializeField]
        Transform _triggerTransform;

        [SerializeField]
        float _radius;
        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            Vector3 triggerToPosVec = pos - _triggerTransform.position;
            float magnitude = Mathf.Sqrt(triggerToPosVec.x * triggerToPosVec.x + triggerToPosVec.y * triggerToPosVec.y);
            Handles.Label((triggerToPosVec / 2) + _triggerTransform.position, $"length: {magnitude}");
            Gizmos.DrawLine(_triggerTransform.position, pos);

            if (magnitude <= _radius)
            {

                Gizmos.color = Color.red;

            }
            else
            {
                Gizmos.color = Color.blue;
            }

            //triggerSphere
            Gizmos.DrawSphere(pos, _radius);



        }
    }
}