using UnityEngine;

public class CreditsUI : MonoBehaviour
{
	void Start()
	{
		GetComponent<UIOverlay>().Show(this.OnClick);
	}

	void OnClick()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame",
				UnityEngine.SceneManagement.LoadSceneMode.Single);
	}
}
