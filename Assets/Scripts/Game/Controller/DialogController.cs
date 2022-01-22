
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class DialogController : MonoBehaviour
	{
		public NpcDialogue npcDialogue;

		public void NpcDialog(string dialog)
		{
			Debug.Log("dialog:" + dialog);
			if (!npcDialogue)
			{
				npcDialogue = FindObjectOfType<NpcDialogue>(true);
			}
			if (npcDialogue)
			{
				npcDialogue.PopupDialogue(dialog);
			}
		}

		public void CharacterDialog(string dialog)
		{
			Debug.Log("dialog:" + dialog);
			//GameObject.Find("TestCharDialog").GetComponent<Text>().text = dialog;
		}
	}
}