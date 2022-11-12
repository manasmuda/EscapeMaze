using System;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectGenerator : ScriptableWizard {

    public MonoScript soMonoScript;
    public string fileName;
    public string filePath;
    
    [MenuItem("Window/Custom Tools/Scriptable Object Generator")]
    static void CreateWizard() {
        var wizard = DisplayWizard<ScriptableObjectGenerator>("Generate Scriptable Object", "Generator");
        wizard.filePath = "Assets/DataSO/";
    }

    private void OnWizardCreate() {
        if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(filePath) && filePath.Contains("Assets/")) {
            Type scriptableObjectType = soMonoScript.GetClass();
            if (scriptableObjectType != null) {
                var asset = CreateInstance(scriptableObjectType);

                AssetDatabase.CreateAsset(asset, filePath+fileName+".asset");
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