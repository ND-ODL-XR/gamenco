using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInfo : NetworkBehaviour
{
    public GameObject cameraRig;

    public void MoveCameraRig(Vector3 position) {
        cameraRig = FindFirstObjectByType<OVRCameraRig>().gameObject;
        cameraRig.transform.position = position;
    }

}
