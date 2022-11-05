using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lecture_1.Assignment_2_VectorTransformation
{
    public class WorldToLocal : MonoBehaviour
    {

        Transform _worldTransform;

        private void OnDrawGizmos()
        {

            if (_worldTransform == null)
            {

                _worldTransform = new GameObject("WorldPos").transform;
                _worldTransform.SetParent(this.transform);
                _worldTransform.localPosition = new Vector3(Random.Range(0, 10f), Random.Range(0, 10f), 0);
            }

            Vector3 worldPos = _worldTransform.position;
            Handles.Label(_worldTransform.position, worldPos.ToString());

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_worldTransform.position, 1f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right);


            Vector3 dirVec = _worldTransform.position - transform.position;

            Vector3 localPos = new Vector3((dirVec.x * transform.right.x + dirVec.y * transform.right.y), (dirVec.x * transform.up.x + dirVec.y * transform.up.y));

            Gizmos.DrawWireSphere(localPos + transform.position, 1f);
            
            Handles.Label(transform.position, (localPos).ToString());
            Handles.Label(transform.position + Vector3.down, (_worldTransform.localPosition).ToString());
        }



    }

}