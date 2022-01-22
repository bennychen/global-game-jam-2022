
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DialogController : MonoBehaviour
    {
        public void NpcDialog(string dialog)
        {
            Debug.Log("dialog:" + dialog);
            //GameObject.Find("TestNPCDialog").GetComponent<Text>().text = dialog;
        }
        
        public void CharacterDialog(string dialog)
        {
            Debug.Log("dialog:" + dialog);
            //GameObject.Find("TestCharDialog").GetComponent<Text>().text = dialog;
        }
    }
}