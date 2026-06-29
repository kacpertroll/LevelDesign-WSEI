using UnityEngine;

/// <summary>
/// Prosty toggle dla GameObjectów — idealne do świateł, efektów VFX,
/// podświetleń, markerów itp.
///
/// Użycie:
///   Podepnij ToggleTarget() / EnableTarget() / DisableTarget()
///   do Unity Eventów na InteractableObject.
/// </summary>
public class VFXToggler : MonoBehaviour
{
    [Header("Obiekt do sterowania")]
    [Tooltip("GameObject który ma być włączany/wyłączany (np. światło, efekt cząsteczkowy)")]
    [SerializeField] private GameObject target;

    [Header("Ustawienia")]
    [Tooltip("Stan startowy obiektu")]
    [SerializeField] private bool startEnabled = false;

    private void Start()
    {
        if (target != null)
            target.SetActive(startEnabled);
    }

    // -------------------------------------------------------

    /// <summary>Przełącza aktywność obiektu.</summary>
    public void ToggleTarget()
    {
        if (target == null) return;
        target.SetActive(!target.activeSelf);
        Debug.Log($"[VFX] Toggle: {target.name} → {target.activeSelf}");
    }

    /// <summary>Włącza obiekt.</summary>
    public void EnableTarget()
    {
        if (target == null) return;
        target.SetActive(true);
    }

    /// <summary>Wyłącza obiekt.</summary>
    public void DisableTarget()
    {
        if (target == null) return;
        target.SetActive(false);
    }

    /// <summary>Ustaw inny target z kodu jeśli potrzeba.</summary>
    public void SetTarget(GameObject newTarget) => target = newTarget;
}
