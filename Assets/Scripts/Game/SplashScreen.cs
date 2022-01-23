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
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame",
				UnityEngine.SceneManagement.LoadSceneMode.Single);
	}
}
