using UnityEngine;

namespace HyperCard
{
    public class SpriteAlphaPingPong : MonoBehaviour
    {
        public float AlphaMin = 0f;
        public float AlphaMax = 1f;
        public float Duration;

        void Start()
        {

        }

        void Update()
        {
            var lerp = Mathf.PingPong(Time.time, Duration) / Duration;
            var alpha = Mathf.Lerp(AlphaMin, AlphaMax, Mathf.SmoothStep(AlphaMin, AlphaMax, lerp));
            var color = GetComponent<SpriteRenderer>().material.color;

            GetComponent<SpriteRenderer>().material.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}