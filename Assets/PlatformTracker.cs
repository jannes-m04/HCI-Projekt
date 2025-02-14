using UnityEngine;

public class PlatformTracker : MonoBehaviour
{
    private Transform currentPlatform;  // Referenz zur aktuellen Plattform
    private Transform playerTransform;
    private SimplePlayerController playerController;
    private bool isModeSwitchComplete = false; // Flag um sicherzustellen, dass der Moduswechsel abgeschlossen ist

    private void Start()
    {
        playerTransform = GetComponent<Transform>();  // Spieler Transform speichern
        playerController = FindObjectOfType<SimplePlayerController>();
    }

    private void OnEnable()
    {
        // Abonnieren des Moduswechsel-Events
        CameraSwitch.OnModeSwitched += HandleModeSwitch;
    }

    private void OnDisable()
    {
        // Vom Moduswechsel-Event abmelden
        CameraSwitch.OnModeSwitched -= HandleModeSwitch;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Prüfen, ob der Spieler eine Plattform-Triggerbox betritt
        if (other.CompareTag("PlatformTrigger"))
        {
            currentPlatform = other.transform.parent;  // Elternobjekt der Triggerbox als aktuelle Plattform speichern
            Debug.Log($"[PlatformTracker] Spieler betritt Plattform-Trigger: {currentPlatform.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Prüfen, ob der Spieler die aktuelle Plattform-Triggerbox verlässt
        if (other.CompareTag("PlatformTrigger") && currentPlatform == other.transform.parent)
        {
            Collider platformCollider = currentPlatform.GetComponent<Collider>();
        if (platformCollider != null)
        {
            // Berechne die rechte Kante der Plattform
            Vector3 platformSize = platformCollider.bounds.size; // Größe der Plattform
            Vector3 platformPosition = platformCollider.bounds.center; // Zentrum der Plattform
            
            float rightEdgeX = platformPosition.x + (platformSize.x / 2); // Rechte Kante der Plattform

            if(playerController._is2DMode == true){
            // Spielerposition an die rechte Kante setzen
            playerTransform.position = new Vector3(rightEdgeX - 0.3f, playerTransform.position.y, playerTransform.position.z);

            Debug.Log($"Position der currentPlatform: {platformPosition}, rechte Kante X: {rightEdgeX}");
            } else {
                playerTransform.position = new Vector3(platformPosition.x, playerTransform.position.y, playerTransform.position.z);
            }
        }
        else
        {
            Debug.LogWarning("Die Plattform hat keinen Collider. Position kann nicht berechnet werden.");
        }
        }
    }

    private void HandleModeSwitch(bool is2DMode)
    {
        // Wenn der Modus auf 3D gewechselt wird, warten wir, bis der Wechsel abgeschlossen ist
        if (!is2DMode && currentPlatform != null)
        {
            // Setze das Flag, dass der Wechsel abgeschlossen ist
            isModeSwitchComplete = true;
            Debug.Log("[PlatformTracker] Moduswechsel abgeschlossen. Plattform wird jetzt abgefragt.");

            

            // Plattformposition nach dem Wechsel holen
            Vector3 platformPosition = currentPlatform.position;

            // Debug-Ausgabe der aktuellen Position der Plattform
            Debug.Log($"[PlatformTracker] Aktuelle Plattformposition nach Wechsel: {platformPosition}");

            // Sobald der Wechsel abgeschlossen ist, setzen wir den Spieler auf die Plattform
            if (isModeSwitchComplete)
            {
                playerTransform.position = new Vector3(platformPosition.x, playerTransform.position.y, playerTransform.position.z);
                Debug.Log($"[PlatformTracker] Spieler wird auf die Plattform verschoben: {playerTransform.position}");
            }
        }
    }
}
