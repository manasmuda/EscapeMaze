using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class EntryScene : MonoBehaviour
{
    
    void Start() {
        Addressables.LoadSceneAsync("LobbyScene");
    }

    
}
