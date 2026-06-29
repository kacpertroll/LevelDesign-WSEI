using UnityEngine;

/// <summary>
/// Obsługuje otwieranie i zamykanie drzwi z płynną animacją.
///
/// Użycie:
///   1. Dodaj komponent do obiektu drzwi (pivot musi być przy zawiasach!).
///   2. Ustaw openAngle (np. 90) i animationSpeed.
///   3. Podepnij Toggle() lub Open()/Close() do Unity Eventów
///      na komponencie InteractableObject tego samego obiektu.
/// </summary>
public class DoorController : MonoBehaviour
{
    [Header("Ustawienia drzwi")]
    [Tooltip("Kąt obrotu w osi Y gdy drzwi są otwarte (np. 90 lub -90)")]
    [SerializeField] private float openAngle = 90f;
    [Tooltip("Prędkość animacji otwierania/zamykania")]
    [SerializeField] private float animationSpeed = 3f;

    // Stan
    private bool isOpen = false;
    private bool isAnimating = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Quaternion targetRotation;

    // -------------------------------------------------------

    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
        targetRotation = closedRotation;
    }

    private void Update()
    {
        if (!isAnimating) return;

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * animationSpeed
        );

        // Zakończ animację gdy blisko celu
        if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.5f)
        {
            transform.localRotation = targetRotation;
            isAnimating = false;
        }
    }

    // -------------------------------------------------------
    // Publiczne metody — podepnij do Unity Eventów

    /// <summary>
    /// Przełącza stan drzwi (otwórz / zamknij).
    /// Podepnij do OnInteract w InteractableObject.
    /// </summary>
    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        targetRotation = openRotation;
        isAnimating = true;
        Debug.Log($"[Door] Otwieranie: {gameObject.name}");
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        targetRotation = closedRotation;
        isAnimating = true;
        Debug.Log($"[Door] Zamykanie: {gameObject.name}");
    }

    public bool IsOpen => isOpen;
}
