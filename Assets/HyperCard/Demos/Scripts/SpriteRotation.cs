using UnityEngine;

namespace HyperCard
{
    public class SpriteRotation : MonoBehaviour
    {
        public float RotationSpeed;

        void Start()
        {

        }

        void Update()
        {
            transform.Rotate(Vector3.back * (RotationSpeed * Time.deltaTime));
        }
    }
}