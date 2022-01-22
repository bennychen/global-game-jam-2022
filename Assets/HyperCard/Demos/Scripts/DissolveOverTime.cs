using System.Collections;
using UnityEngine;

namespace HyperCard
{
    public class DissolveOverTime : MonoBehaviour
    {
        public float FadeTime;

        private void Start()
        {
            StartCoroutine(FadeInOut());
        }

        IEnumerator FadeInOut()
        {
            if (FadeTime != 0)
            {
                var t = 0f;

                while (GetComponent<Card>().Properties.DissolveAmount < 1)
                {
                    t += Time.deltaTime / FadeTime;

                    GetComponent<Card>().Properties.DissolveAmount = Mathf.Lerp(0, 1, Mathf.SmoothStep(0.0f, 1.0f, t));

                    yield return new WaitForEndOfFrame();
                }

                t = 0f;

                while (GetComponent<Card>().Properties.DissolveAmount > 0)
                {
                    t += Time.deltaTime / FadeTime;

                    GetComponent<Card>().Properties.DissolveAmount = Mathf.Lerp(1, 0, Mathf.SmoothStep(0.0f, 1.0f, t));

                    yield return new WaitForEndOfFrame();
                }
            }

            yield return FadeInOut();
        }
    }
}