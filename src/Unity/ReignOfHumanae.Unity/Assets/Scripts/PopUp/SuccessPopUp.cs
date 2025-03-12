using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PopUp
{
    public class SuccessPopUp : MonoBehaviour
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
    }
}
