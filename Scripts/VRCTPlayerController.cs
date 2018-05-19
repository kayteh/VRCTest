using UnityEngine;
using UnityEngine.Rendering;

public class VRCTPlayerController : MonoBehaviour {
    public GameObject m_Avatar;

    public GameObject m_CameraRig;

    public float m_HeadPopinDistance = 1;

    private GameObject Primary;
    private GameObject Shadow; 

    private VRCSDK2.VRC_AvatarDescriptor Descriptor;

    private Transform PrimaryHeadTransform;
    private Transform ShadowHeadTransform;

    private void Awake() {
        Descriptor = m_Avatar.GetComponent<VRCSDK2.VRC_AvatarDescriptor>();
        Primary = GameObject.Instantiate(m_Avatar, transform);
        Shadow = GameObject.Instantiate(m_Avatar, transform);
        m_CameraRig = GameObject.FindWithTag("MainCamera");

        Primary.name = m_Avatar.name + " (Primary)";
        Primary.SetActive(true);
        Primary.layer = LayerMask.NameToLayer("PlayerLocal");

        Shadow.name = m_Avatar.name + " (Shadow)";        
        Shadow.SetActive(true);

        SetupPrimary();
        SetupShadow();
        SetupCamera();
    }

    private void SetupPrimary() {
        SkinnedMeshRenderer[] smrs = Primary.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in smrs) {
            renderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        PrimaryHeadTransform = Primary.transform.Find("Armature/Hips/Spine/Chest/Neck/Head");
    }

    private void SetupCamera() {
        m_CameraRig.transform.position = Descriptor.ViewPosition;
        var oldRig = m_CameraRig;
        m_CameraRig = GameObject.Instantiate(m_CameraRig, transform, true);
        m_CameraRig.name = "Player Camera";
        oldRig.SetActive(false);
    }

    private void SetupShadow() {
        SkinnedMeshRenderer[] smrs = Shadow.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in smrs) {
            var ss = renderer.gameObject;
            var originalName = ss.name;
            ss.name += " (Shadow)";

            var sm = GameObject.Instantiate(ss, Shadow.transform);
            sm.name = originalName + " (Mirror)";
            
            var ssRenderer = renderer;
            var smRenderer = sm.GetComponent<SkinnedMeshRenderer>();

            // setup shadow-only
            ssRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            ssRenderer.receiveShadows = false;

            // setup mirror-only
            smRenderer.shadowCastingMode = ShadowCastingMode.Off; // shadows provided by the other SMR
            sm.layer = 30;
        }

        ShadowHeadTransform = Shadow.transform.Find("Armature/Hips/Spine/Chest/Neck/Head");        
    }

    void LateUpdate() {
        if (Mathf.Abs(Vector3.Distance(m_CameraRig.transform.position, PrimaryHeadTransform.position)) > m_HeadPopinDistance) {
            ScalePrimaryHead(1);
        } else {
            ScalePrimaryHead(0);
        }
    }

    void ScalePrimaryHead(float scale) {
        PrimaryHeadTransform.localScale = new Vector3(scale, scale, scale);
    }
}