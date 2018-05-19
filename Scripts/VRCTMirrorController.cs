using UnityEngine;

[ExecuteInEditMode]
public class VRCTMirrorController : MonoBehaviour {
    Camera playerCam;
    [SerializeField] Camera m_MirrorCam;

    void Start() {
        playerCam = Camera.main;
        m_MirrorCam.fieldOfView = playerCam.fieldOfView;
    }

    void Update() {
        CalculateRotation();
    }

    void CalculateRotation() {
        Vector3 playerDir = (playerCam.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(playerDir);

        rotation.eulerAngles = transform.eulerAngles - rotation.eulerAngles;
        m_MirrorCam.transform.localRotation = rotation;
    }
}