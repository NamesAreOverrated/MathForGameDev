using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lecture_1.Assignment_2_BouncingLaser
{
    public class BouncingLaser : MonoBehaviour
    {
        [SerializeField]
        Transform _startPosTransform;

        [Header("Laser Properties")]
        [SerializeField]
        int _laserCount;
        [SerializeField]
        float _bounceCount;
        [SerializeField]
        Vector2 _laserSpeedRange;
        Laser[] _lasers;

        [Header("Tester")]
        [SerializeField]
        bool isDebug = false;
        [SerializeField]
        Transform _testDirTransform;
        private void BouncingLase()
        {
            if (_lasers == null)
            {
                _lasers = new Laser[_laserCount];
            }
            for (int i = 0; i < _lasers.Length; i++)
            {
                if (_lasers[i] != null)
                {
                    Laser laser = _lasers[i];
                    if (laser.isBouncing)
                    {
                        laser.pos += laser.dir * laser.speed;

                        if ((laser.pos - laser.targetPos).magnitude <= 0.1f)
                        {
                            laser.isBouncing = false;
                            laser.dir = laser.nextDir;
                            laser.bounceCount += 1;
                            if (laser.bounceCount >= _bounceCount)
                            {
                                _lasers[i] = null;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (!RayCastNextTarget(_lasers[i]))
                        {
                            _lasers[i] = null;
                            continue;
                        }
                    }
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(_lasers[i].pos, (-laser.dir * (0.1f + (laser.bounceCount * 0.7f)) + laser.pos));
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(_lasers[i].pos, 0.1f);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(_lasers[i].targetPos, 0.2f);
                }
                else
                {
                    _lasers[i] = new Laser(_startPosTransform.position, new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), 0), Random.Range(_laserSpeedRange.x, _laserSpeedRange.y));
                    RayCastNextTarget(_lasers[i]);
                }

            }
        }

        public void OnDrawGizmos()
        {

            if (isDebug)
            {
                Vector3 dir = (_testDirTransform.position - _startPosTransform.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_startPosTransform.position, 1f);

                Vector3 dirNormalized = dir / Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(_startPosTransform.position, _startPosTransform.position + dirNormalized);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_testDirTransform.position, 1f);

                int layer = LayerMask.GetMask("BouncingLaser");
                RaycastHit raycastHit;
                if (Physics.Raycast(_startPosTransform.position, dirNormalized, out raycastHit, 999f, layer))
                {

                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(raycastHit.point, 0.5f);
                    Gizmos.DrawLine(raycastHit.point, 3 * (raycastHit.normal) + raycastHit.point);
                    Gizmos.color = Color.green;

                    float scaler = dirNormalized.x * raycastHit.normal.x + dirNormalized.y * raycastHit.normal.y;
                    Vector3 reflectDir = (-(2 * scaler) * raycastHit.normal) + dirNormalized;

                    Gizmos.DrawLine(raycastHit.point, 3 * (reflectDir) + raycastHit.point);
                }
            }
            else
            {
                BouncingLase();
            }


        }
        bool RayCastNextTarget(Laser laser)
        {
            int layer = LayerMask.GetMask("BouncingLaser");
            RaycastHit raycastHit;
            if (Physics.Raycast(laser.pos, laser.dir, out raycastHit, 999f, layer))
            {
                float scaler = laser.dir.x * raycastHit.normal.x + laser.dir.y * raycastHit.normal.y;
                laser.nextDir = ((-(2 * scaler) * raycastHit.normal) + laser.dir);
                laser.targetPos = raycastHit.point;
                laser.isBouncing = true;
                return true;
            }
            return false;
        }




    }

    public class Laser
    {
        public Vector3 pos;
        public Vector3 nextDir;
        public Vector3 dir;
        public float speed;
        public bool isBouncing;
        public Vector3 targetPos;
        public int bounceCount;

        public Laser(Vector3 pos, Vector3 dir, float speed)
        {
            this.pos = pos;
            this.dir = dir;
            this.speed = speed;
            this.isBouncing = false;
        }
    }

}