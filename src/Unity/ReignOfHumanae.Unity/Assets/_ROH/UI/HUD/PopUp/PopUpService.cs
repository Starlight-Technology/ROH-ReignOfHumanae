using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PopUp
{
    public class PopUpService : MonoBehaviour
    {
        public GameObject PopupPanel;
        public Text PopupMessage;

        public void ShowPopup(string message)
        {
            PopupMessage.text = message;
            PopupPanel.SetActive(true);
        }

        public void HidePopup()
        {
            PopupPanel.SetActive(false);
        }

        public static bool QuestionNo() => false;
        public static bool QuestionYes() => true;

    }
}
