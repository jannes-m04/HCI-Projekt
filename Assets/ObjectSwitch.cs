using UnityEngine;

public class ObjectSwitch : MonoBehaviour
{
    public Vector3 position3D;  // Position im 3D-Modus
    public Vector3 position2D;  // Position im 2D-Modus

    private void OnEnable()
    {
        CameraSwitch.OnModeSwitched += HandleModeSwitch;
    }

    private void OnDisable()
    {
        CameraSwitch.OnModeSwitched -= HandleModeSwitch;
    }

    private void HandleModeSwitch(bool is2DMode)
    {
        transform.position = is2DMode ? position2D : position3D;
    }
}
