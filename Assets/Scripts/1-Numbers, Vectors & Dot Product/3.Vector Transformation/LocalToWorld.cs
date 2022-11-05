using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Lecture_1.Assignment_2_VectorTransformation
{
    public class LocalToWorld : MonoBehaviour
    {
        Transform _localTransform;

        private void OnDrawGizmos()
        {

            if (_localTransform == null)
            {

                _localTransform = new GameObject("LocalPos").transform;
                _localTransform.SetParent(this.transform);
                _localTransform.localPosition = new Vector3(Random.Range(0, 10f), Random.Range(0, 10f), 0);

            }

            Vector3 localPos = _localTransform.localPosition;
            Handles.Label(_localTransform.position, localPos.ToString());

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_localTransform.position, 1f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right);

            Vector3 worldPos = (_localTransform.localPosition.x * transform.right + _localTransform.localPosition.y * transform.up) + transform.position;
            Gizmos.DrawWireSphere(worldPos, 1f);

            Handles.Label(transform.position, (worldPos).ToString());
            Handles.Label(transform.position + Vector3.down, (_localTransform.position).ToString());
        }

    }

}