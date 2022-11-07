using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lecture_2.Assignment_6_TurretLookFire
{
    public class TurretFire : MonoBehaviour
    {

        [SerializeField]
        Transform _turretHead;
        [SerializeField]
        Transform _turretGunPoint;
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
        [SerializeField]
        float _fireRate;
        [SerializeField]
        float _fireTimer = 0;

        [Header("Laser Properties")]
        [SerializeField]
        int _laserCount;
        [SerializeField]
        float _bounceCount;
        [SerializeField]
        float _laserSpeed;
        [SerializeField]
        List<Laser> _lasers = new List<Laser>();

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(_turretBarrel.transform.position, _turretBarrel.transform.position + _turretBarrel.forward);

            float magnitude = (_turretGunPoint.transform.position - _target.transform.position).magnitude;
            Vector3 localPos = _turretGunPoint.transform.InverseTransformPoint(_target.transform.position);
            float forwardDot = Vector3.Dot(_turretGunPoint.forward, localPos);
            Gizmos.color = Color.white;
            if (forwardDot > 0 && magnitude <= _range)
            {
                Gizmos.color = Color.blue;

                float heightDot = Vector3.Dot(_turretGunPoint.up, localPos);
                if (heightDot <= _height && heightDot >= -_height)
                {
                    Gizmos.color = Color.green;
                    float radiusDot = Vector3.Dot(_turretGunPoint.right, localPos);

                    if (radiusDot <= _radius && radiusDot >= -_radius)
                    {

                        Vector3 zAxis = (_turretBarrel.position - _target.position).normalized;
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawLine(_turretHead.transform.position, _turretHead.transform.position + zAxis);
                        Vector3 xAxis = Vector3.Cross(_turretHead.up, zAxis);

                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(_turretHead.transform.position, _turretHead.transform.position + xAxis);
                        Vector3 yAxis = Vector3.Cross(zAxis, xAxis);
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(_turretHead.transform.position, _turretHead.transform.position + yAxis);
                        zAxis = (_target.position - _turretBarrel.position).normalized;
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(_turretHead.transform.position, _turretHead.transform.position + zAxis);
                        Gizmos.color = Color.red;
                        _turretHead.rotation = Quaternion.LookRotation(zAxis, yAxis);

                        if (_fireTimer <= 0 && _lasers.Count < _laserCount)
                        {
                            _lasers.Add(new Laser(_turretBarrel.position, _turretBarrel.forward, _laserSpeed));
                            _fireTimer = _fireRate;
                        }
                    }

                }
            }
            Gizmos.DrawSphere(_target.transform.position, 0.05f);

            _fireTimer -= Time.unscaledDeltaTime;
            FireLaser();

        }

        [System.Serializable]
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
        bool RayCastNextTarget(Laser laser)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(laser.pos, laser.dir, out raycastHit))
            {
                laser.nextDir = Vector3.Reflect(laser.dir, raycastHit.normal);
                laser.targetPos = raycastHit.point;
                laser.isBouncing = true;
                return true;
            }
            return false;
        }
        void FireLaser()
        {
            for (int i = 0; i < _lasers.Count; i++)
            {
                if (_lasers[i] != null)
                {
                    Laser laser = _lasers[i];
                    if (laser.isBouncing)
                    {
                        laser.pos += laser.dir * laser.speed;

                        if ((laser.pos - laser.targetPos).magnitude <= laser.speed)
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
                    Gizmos.DrawLine(_lasers[i].pos, (-laser.dir * (0.01f + (laser.bounceCount * 0.07f)) + laser.pos));
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(_lasers[i].pos, 0.01f);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(_lasers[i].targetPos, 0.02f);
                }

            }

            _lasers.RemoveAll(l => l == null);
        }

    }

}