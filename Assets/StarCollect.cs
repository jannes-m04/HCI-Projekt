using UnityEngine;
using TMPro; 

public class StarCollect : MonoBehaviour
{
    [SerializeField] private GameObject goal; // Ziel-GameObject
    [SerializeField] private TextMeshProUGUI starText; // Text-UI-Element für den Sternenzähler
    private static int starCounter = 0; // Zähler für eingesammelte Sterne

    private void Start()
    {
        starCounter = 0;
        UpdateStarText(); // Initialer Text
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false); // Stern verschwindet
            starCounter++; // Zähler erhöhen
            UpdateStarText(); // Text aktualisieren

            if (starCounter >= 3 && goal != null)
            {
                goal.SetActive(true); // Ziel aktivieren
            }
        }
    }

    private void UpdateStarText()
    {
        if (starText != null)
        {
            starText.text = $"Sterne: {starCounter}/3"; // Text aktualisieren
        }
    }
}
