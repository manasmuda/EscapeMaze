using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace EscapeMaze.Lobby {

    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour {

        public Button button;

        void Start() {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(LoadGameScene);
        }

        private async void LoadGameScene() {
            button.interactable = false;
            await Addressables.LoadSceneAsync("GameScene");
        }

    }
}
