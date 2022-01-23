using System.Collections;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(DelayLoad());
	}

	IEnumerator DelayLoad()
	{
		yield return new WaitForSeconds(1);
		var sprite = GetComponent<SpriteRenderer>();
		// sprite.color
		float time = 1;
		while (time > 0)
		{
			time -= Time.deltaTime;
			sprite.color = new Color(1, 1, 1, time);
			yield return new WaitForEndOfFrame();
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame",
				UnityEngine.SceneManagement.LoadSceneMode.Single);
	}
}
