
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DialogController : MonoBehaviour
    {
        private void Awake()
        {
            ;
        }

        public void NpcDialog(string dialog)
        {
            Debug.Log("dialog:" + dialog);
            GameController.Instance.NpcDialogue.text.text = dialog;
            //GameObject.Find("TestNPCDialog").GetComponent<Text>().text = dialog;
        }
        
        public void CharacterDialog(string dialog)
        {
            GameController.Instance.CharacterDialogue.text.text = dialog;
            Debug.Log("dialog:" + dialog);
            //GameObject.Find("TestCharDialog").GetComponent<Text>().text = dialog;
        }

        public void PopupSummaryDialog()
        {
            CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.SummaryDialog);
        }
        public void PopupAgeOfDeathDialog()
        {
            CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.AgeOfDeathDialog);
        }
        public void PopupDeadReasonDialog()
        {
            CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.DeadReasonDialog);
        }
        public void PopupNumberOfChildDialog()
        {
            CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.NumberOfChildDialog);
        }
        public void PopupNumberOfKilledDialog()
        {
            CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.NumberOfKilledDialog);
        }
        public void PopupRewardDialog()
        {
            CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.RewardDialog);
        }
        public void PopupPenaltyDialog()
        {
            CharacterDialog(GameController.Instance.GameLoopController.LevelModel.CurrentCharacterData.PenaltyDialog);
        }

    }
}