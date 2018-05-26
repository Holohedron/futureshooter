using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    const int DEFAULTPRIORITY = 10;
    const int TOPPRIORITY = 90;
    static CameraManager _instance;

    public CinemachineVirtualCameraBase FreeCam;
    public CinemachineVirtualCameraBase AimCam;

    public static CameraManager Instance()
    {
        return _instance;
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(_instance);
        }
    }

    public void PrioritizeFreeCam()
    {
        FreeCam.Priority = TOPPRIORITY;
        AimCam.Priority = DEFAULTPRIORITY;
    }

    public void PrioritizeAimCam()
    {
        FreeCam.Priority = DEFAULTPRIORITY;
        AimCam.Priority = TOPPRIORITY;
    }
}
