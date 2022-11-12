using System;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeMaze.Lobby {
    
    [RequireComponent(typeof(Button))]
    public class ExitButton : MonoBehaviour {

        public Button button;

        private void Start() {
            button.onClick.AddListener(Application.Quit);
        }
    }
}
