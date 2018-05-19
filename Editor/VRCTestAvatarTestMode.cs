using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class VRCTestAvatarTestMode {

    private static Scene originalActiveScene;
    private static Scene testScene;
    private static List<Scene> loadedScenes;
    private static GameObject goSafeCopy;
    

    private static void GetAllScenes(out List<Scene> scenes) {
        scenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            scenes.Add(SceneManager.GetSceneAt(i));
        }
    }

    public static void StartTest(GameObject go) {
        GetAllScenes(out loadedScenes);
        
        #if UNITY_EDITOR
        originalActiveScene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.SaveOpenScenes();
        EnvConfig.SetVRSDKs(new string[] {"OpenVR", "Oculus", "None"});
        EditorApplication.isPlaying = true;
        testScene = EditorSceneManager.OpenScene("Assets/VRCTest/Scenes/AvatarTest.unity", OpenSceneMode.Additive);
        
        #endif


        foreach (var scene in loadedScenes) {
            SceneManager.UnloadSceneAsync(scene);
        }

        var goClone = GameObject.Instantiate(go);
        SceneManager.SetActiveScene(testScene);
        SceneManager.MoveGameObjectToScene(goClone, testScene);
        goClone.name = go.name;
        goClone.transform.position = Vector3.zero;

        // clear player if we re-run (e.g. oops happened or in dev)
        var pGO = GameObject.FindWithTag("Player");
        if (pGO != null) {
            Object.DestroyImmediate(pGO);
        }

        goClone.tag = "Player";
        goSafeCopy = goClone;
        goSafeCopy.SetActive(false);

        var player = new GameObject("Player");
        var pctrl = player.AddComponent<VRCTPlayerController>();
        pctrl.m_Avatar = goSafeCopy;
        SceneManager.MoveGameObjectToScene(player, testScene);
    }

    public static void EndTest() {
        #if UNITY_EDITOR
        EditorSceneManager.SetActiveScene(originalActiveScene);
        foreach (var scene in loadedScenes) {
            SceneManager.LoadSceneAsync(scene.path);
        }
        EditorSceneManager.CloseScene(testScene, true);
        EditorApplication.isPlaying = false;
        #endif
    }
}