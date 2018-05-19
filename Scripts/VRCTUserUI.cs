using UnityEngine;

public class VRCTUserUI : MonoBehaviour {
    Canvas canvas = null;

    private void Awake() {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    void OnGUI() {
        
    }
}