using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Restartuje scenê gdy gracz wejdzie w kontakt z obiektem tagowanym "Death".
/// Komponent dodaj do gracza (CharacterController musi byæ na tym samym GO).
/// </summary>
public class DeathTrigger : MonoBehaviour
{
    [Header("Ustawienia")]
    [Tooltip("Nazwa sceny do za³adowania po œmierci. Zostaw puste = prze³aduj aktualn¹.")]
    [SerializeField] private string respawnSceneName = "";
    [SerializeField] private float respawnDelay = 0f;

    [Header("Events")]
    public UnityEvent OnDeath;

    private bool isDead = false;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDead) return;

        if (hit.gameObject.CompareTag("Death"))
        {
            isDead = true;
            Debug.Log("[Death] Gracz zgin¹³");
            OnDeath?.Invoke();

            if (respawnDelay > 0f)
                Invoke(nameof(Respawn), respawnDelay);
            else
                Respawn();
        }
    }

    public void Respawn()
    {
        isDead = false;
        string scene = string.IsNullOrEmpty(respawnSceneName)
            ? SceneManager.GetActiveScene().name
            : respawnSceneName;

        SceneManager.LoadScene(scene);
    }
}