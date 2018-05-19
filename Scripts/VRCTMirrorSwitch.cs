using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

[ExecuteInEditMode]
public class VRCTMirrorSwitch : MonoBehaviour {

	bool isVR = false;

	
	[SerializeField] GameObject m_DesktopMirror = null;
	[SerializeField] GameObject m_VRMirror = null;

	bool wasPlayingLastFrame = false;

	// Use this for initialization
	void Start () {
		// None API is blank, thus means desktop mode.
		isVR = VRSettings.loadedDeviceName != "";
		m_DesktopMirror.SetActive(false);
		m_VRMirror.SetActive(false);

		#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying && isVR) {	
		#else
			if (isVR) {
		#endif
			StartVR();
		} else {
			StartDesktop();
		}
	}

	void StartVR () {
		Debug.Log("VR Mode on");
		m_VRMirror.SetActive(true);
		m_DesktopMirror.SetActive(false);		
	}

	void StartDesktop () {
		Debug.Log("Desktop Mode on");		
		m_DesktopMirror.SetActive(true);
		m_VRMirror.SetActive(false);			
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if (wasPlayingLastFrame != UnityEditor.EditorApplication.isPlaying) {
			Start();
		}

		wasPlayingLastFrame = UnityEditor.EditorApplication.isPlaying;
		#endif
	}
}
