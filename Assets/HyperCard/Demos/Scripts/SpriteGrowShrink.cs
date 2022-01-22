using UnityEngine;

namespace HyperCard
{
    public class SpriteGrowShrink : MonoBehaviour
    {
        public float SizeMin = 0.1f;
        public float SizeMax = 0.2f;
        public float Speed;

        void Start()
        {

        }

        void Update()
        {
            var r = SizeMax - SizeMin;
            var scale = SizeMin + Mathf.PingPong(Time.time * Speed, r);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}