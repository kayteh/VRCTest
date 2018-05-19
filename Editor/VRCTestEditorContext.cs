using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using VRCSDK2;

[InitializeOnLoad]
public static class VRCTestEditorContext {

	// [MenuItem("GameObject/VRCTest/Start Testing", true, -10)]
	// public static bool AvatarDescriptorTest (MenuCommand mc) {
	// 	var go = (GameObject)mc.context;
	// 	bool o = go.GetComponent<VRC_AvatarDescriptor>() != null && go.GetComponent<VRC_SceneDescriptor>() != null;

	// 	if (o == false) {
	// 		Debug.LogError("Only Avatars and Worlds with descriptors can be tested.");
	// 	}

	// 	return o;
	// }

	[MenuItem("Tools/VRCTest/Activate VR Mode")]
	public static void ActivateVRMode () {
		EnvConfig.SetVRSDKs(new string[] {"OpenVR", "Oculus", "None"});
	}

	[MenuItem("Tools/VRCTest/Activate Desktop Mode")]
	public static void ActivateDesktopMode () {
		EnvConfig.SetVRSDKs(new string[] {"None", "OpenVR", "Oculus"});
	}

	[MenuItem("GameObject/VRCTest/Start Testing", false, -10)]
	public static void StartTesting (MenuCommand mc) {
		var go = (GameObject)mc.context;
		
		if (go.GetComponent<VRC_AvatarDescriptor>() != null) {
			HandleAvatar(go);
		} else if (go.GetComponent<VRC_SceneDescriptor>() != null) {
			HandleWorld(go);
		} else {
			Debug.LogError("Only Avatars and Worlds with descriptors can be tested.");			
		}
	}

	static void HandleWorld(GameObject go) {
		Debug.LogError("VRC World testing is not implemented yet.");
	}

	static void HandleAvatar(GameObject go) {
		VRCTestAvatarTestMode.StartTest(go);
	}
}
