using System;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectGenerator : ScriptableWizard {

    public UnityEngine.Object obj;
    public string name;
    public string path;
    
    [MenuItem("Window/Custom Tools/Scriptable Object Generator")]
    static void CreateWizard() {
        var wizard = DisplayWizard<ScriptableObjectGenerator>("Generate Scriptable Object", "Generator");
        wizard.path = "Assets/DataSO/";
    }

    private void OnWizardCreate() {
        if (obj is MonoScript && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path) && path.Contains("Assets/")) {
            Type scriptableObjectType = ((MonoScript) obj).GetClass();
            if (scriptableObjectType != null) {
                var asset = ScriptableObject.CreateInstance(scriptableObjectType);

                AssetDatabase.CreateAsset(asset, path+name+".asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;
            }
        }
        else {
            Debug.LogError("Failed to create SO");
        }
    }
}