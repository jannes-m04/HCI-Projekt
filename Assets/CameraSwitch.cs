using UnityEngine;
using System;
//Hmmm

public class CameraSwitch : MonoBehaviour
{
    private Camera _camera;
    public Transform player;
    public Vector3 thirdPersonOffset;
    public Vector3 sideViewOffset;
    public float switchSpeed = 1f;
    public KeyCode switchKey = KeyCode.C;

    public static event Action<bool> OnModeSwitched;  // Ereignis für Moduswechsel

    public bool is2DMode;
    private Vector3 currentOffset;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        currentOffset = thirdPersonOffset;
    }

    private void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            is2DMode = !is2DMode;
            SwitchCameraMode();
            OnModeSwitched?.Invoke(is2DMode);  // Moduswechsel-Event auslösen
        }

        MoveCamera();
    }

    private void MoveCamera()
{
    if (player == null)
        return;

    if (is2DMode)
    {
        // Im 2D-Modus die Kamera direkt auf die Z-Achse des Spielers setzen
        transform.position = new Vector3(sideViewOffset.x, sideViewOffset.y, player.position.z + sideViewOffset.z);

        // Kamera um -90° drehen, sodass sie in die richtige Richtung schaut
        transform.rotation = Quaternion.Euler(0, -90, 0);
    }
    else
    {
        // Im 3D-Modus sanfter Übergang mit Lerp und Ausrichtung auf den Spieler
        transform.position = Vector3.Lerp(transform.position, player.position + currentOffset, Time.deltaTime * switchSpeed);
        transform.LookAt(player);
    }
}

    private void SwitchCameraMode()
    {
        currentOffset = is2DMode ? sideViewOffset : thirdPersonOffset;
        transform.rotation = Quaternion.Euler(0, is2DMode ? 90 : 0, 0);

        // Spielerposition im 2D-Modus auf x=0 setzen
        if (player != null)
        {
            var playerPosition = player.position;
            player.position = new Vector3(is2DMode ? 8f : playerPosition.x, playerPosition.y, playerPosition.z);
        }

        _camera.orthographic = is2DMode;
    }
}
