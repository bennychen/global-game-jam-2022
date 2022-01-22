using UnityEngine;

namespace HyperCard
{
    public class RotateOverTime : MonoBehaviour
    {
        public float RotationSpeed;

        void Update()
        {
            transform.Rotate(0, Time.deltaTime * RotationSpeed, 0);
        }
    }
}