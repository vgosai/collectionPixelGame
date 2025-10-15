using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraAttachToSpawnedPlayer : MonoBehaviour
{
    ICinemachineCamera vCam;
    UnityAction<Transform> setCameraTargetAction;
    void Awake()
    {
        vCam = GetComponent<ICinemachineCamera>();
        setCameraTargetAction = new UnityAction<Transform>(SetCameraTarget);
    }
    private void SetCameraTarget(Transform cameraTarget)
    {
        vCam.Follow = cameraTarget;
        vCam.VirtualCameraGameObject.transform.parent = cameraTarget;
    }

    void OnEnable()
    {
        PlayerEvents.onPlayerSpawned += setCameraTargetAction;
    }

    void OnDisable()
    {
        PlayerEvents.onPlayerSpawned -= setCameraTargetAction;
    }
}
