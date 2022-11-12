using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace EscapeMaze.Game {

    public class MatchResultPage : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Button continueButton;

        void Start() {
            continueButton.onClick.AddListener(GoToLobby);
        }

        public void ShowMatchResult(GameResult result) {
            gameObject.SetActive(true);
            if (result == GameResult.Victory) {
                resultText.text = "VICTORY";
                resultText.color = Color.green;
            } else {
                resultText.text = "DEFEAT";
                resultText.color = Color.red;
            }
        }

        private void GoToLobby() {
            continueButton.interactable = false;
            Addressables.LoadSceneAsync("LobbyScene");
        }


    }
}
