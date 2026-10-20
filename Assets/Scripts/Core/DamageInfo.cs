using UnityEngine;

namespace Core
{
    public struct DamageInfo
    {
        public int Amount { get; private set; }
        public Vector3 HitPoint { get; private set; }
        public Vector3 HitNormal { get; private set; }
        public GameObject Attacker { get; private set; }

        public DamageInfo(int amount, Vector3 hitPoint = default, Vector3 hitNormal = default, GameObject attacker = null)
        {
            Amount = amount;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
            Attacker = attacker;
        }
    }
}