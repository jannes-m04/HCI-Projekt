using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public GameObject moveTextBox;   // Referenz zur WASD-Textbox
    public GameObject jumpTextBox;   // Referenz zur Space-Textbox
    public GameObject rotateTextBox; // Referenz zur Kamera-Textbox

    public float flashDuration = 1.0f; // Dauer des Aufleuchtens (in Sekunden)

    private Coroutine moveCoroutine;   // Speichert laufende Coroutines
    private Coroutine jumpCoroutine;
    private Coroutine rotateCoroutine;

    void Start()
    {
        // Textboxen beim Start deaktivieren
        if (moveTextBox != null) moveTextBox.SetActive(false);
        if (jumpTextBox != null) jumpTextBox.SetActive(false);
        if (rotateTextBox != null) rotateTextBox.SetActive(false);
    }
    void Update()
    {
        // Überprüfung für Bewegung (WASD oder Pfeiltasten)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            TriggerFlash(ref moveCoroutine, moveTextBox);
        }

        // Überprüfung für Sprung (Space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerFlash(ref jumpCoroutine, jumpTextBox);
        }

        // Überprüfung für Kameradrehung (C)
        if (Input.GetKeyDown(KeyCode.C))
        {
            TriggerFlash(ref rotateCoroutine, rotateTextBox);
        }
    }

    private void TriggerFlash(ref Coroutine runningCoroutine, GameObject textBox)
    {
        // Stoppe die vorherige Coroutine, falls sie noch läuft
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }

        // Starte eine neue Coroutine und speichere die Referenz
        runningCoroutine = StartCoroutine(FlashTextBox(textBox));
    }

    private IEnumerator FlashTextBox(GameObject textBox)
    {
        textBox.SetActive(true); // Textbox aktivieren
        yield return new WaitForSeconds(flashDuration); // Warten
        textBox.SetActive(false); // Textbox wieder deaktivieren
    }
}
