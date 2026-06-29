using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Baza dla wszystkich interaktywnych obiektów w scenie.
/// Dodaj ten komponent do obiektu, przypisz eventy w Inspectorze.
///
/// Opcjonalnie: ustaw wymagany przedmiot z ekwipunku (requiredItem).
/// Jeœli gracz nie ma przedmiotu — interakcja jest blokowana.
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [Header("Konfiguracja")]
    [Tooltip("Tekst wyœwietlany jako prompt interakcji, np. '[E] Otwórz drzwi'")]
    [SerializeField] private string promptText = "[E] Interakcja";

    [Header("Wymagany przedmiot (zostaw puste jeœli brak wymogu)")]
    [Tooltip("Jeœli gracz musi mieæ przedmiot ¿eby wejœæ w interakcjê")]
    [SerializeField] private ItemData requiredItem;
    [Tooltip("Tekst gdy gracz nie ma wymaganego przedmiotu")]
    [SerializeField] private string lockedPromptText = "[E] Wymagany klucz";

    [Header("Unity Events")]
    [Tooltip("Wywo³any gdy gracz wejdzie w interakcjê (ma przedmiot lub brak wymogu)")]
    public UnityEvent OnInteract;
    [Tooltip("Wywo³any gdy gracz próbuje, ale nie ma wymaganego przedmiotu")]
    public UnityEvent OnInteractFailed;

    // -------------------------------------------------------

    /// <summary>
    /// Wywo³ywane przez PlayerInteractionController po wciœniêciu E.
    /// </summary>
    public void TryInteract()
    {
        if (requiredItem != null)
        {
            InventoryController inventory = FindAnyObjectByType<InventoryController>();

            if (inventory == null || !inventory.HasItem(requiredItem))
            {
                Debug.Log($"[Interactable] Brak wymaganego przedmiotu: {requiredItem.itemName}");
                OnInteractFailed?.Invoke();
                return;
            }
        }

        Debug.Log($"[Interactable] Interakcja z: {gameObject.name}");
        OnInteract?.Invoke();
    }

    /// <summary>
    /// Zwraca tekst promptu zale¿nie od stanu ekwipunku.
    /// </summary>
    public string GetPromptText()
    {
        if (requiredItem != null)
        {
            InventoryController inventory = FindAnyObjectByType<InventoryController>();
            if (inventory == null || !inventory.HasItem(requiredItem))
                return lockedPromptText;
        }

        return promptText;
    }
}