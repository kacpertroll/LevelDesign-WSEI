using UnityEngine;
using TMPro;

/// <summary>
/// Obs³uguje raycast od gracza i wywo³uje OnInteract() na trafiony obiekt.
/// Wyœwietla opcjonalny prompt UI (np. "[E] Otwórz drzwi").
/// </summary>
public class PlayerInteractionController : MonoBehaviour
{
    [Header("Ustawienia")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Camera playerCamera;

    [Header("UI Prompt (opcjonalne)")]
    [SerializeField] private GameObject interactPromptUI;
    [SerializeField] private TextMeshProUGUI interactPromptText;

    private InteractableObject currentTarget;

    private void Update()
    {
        CheckForInteractable();

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.TryInteract();
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                currentTarget = interactable;
                ShowPrompt(interactable.GetPromptText());
                return;
            }
        }

        currentTarget = null;
        HidePrompt();
    }

    private void ShowPrompt(string text)
    {
        if (interactPromptUI != null)
            interactPromptUI.SetActive(true);

        if (interactPromptText != null)
            interactPromptText.text = text;
    }

    private void HidePrompt()
    {
        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);
    }
}