using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Kończy zagadkę lub poziom gdy gracz wejdzie w interakcję z obiektem
/// (np. dźwignią, przyciskiem, finalnym artefaktem).
///
/// Użycie:
///   1. Dodaj komponent do obiektu kończącego zagadkę.
///   2. Podepnij Solve() do OnInteract w InteractableObject.
///   3. Ustaw co ma się stać: załaduj nową scenę, wywołaj event, itp.
/// </summary>
public class PuzzleSolver : MonoBehaviour
{
    [Header("Zakończenie")]
    [Tooltip("Opóźnienie przed wykonaniem akcji końcowej (czas na animacje, dźwięk itp.)")]
    [SerializeField] private float solveDelay = 1.5f;

    [Header("Akcja po rozwiązaniu")]
    [SerializeField] private SolveAction solveAction = SolveAction.FireEvent;
    [Tooltip("Nazwa sceny do załadowania (jeśli SolveAction = LoadScene)")]
    [SerializeField] private string nextSceneName = "";

    [Header("Unity Events")]
    [Tooltip("Wywołany natychmiast po wciśnięciu E (dobry moment na dźwięk / animację)")]
    public UnityEvent OnSolved;
    [Tooltip("Wywołany po opóźnieniu (solveDelay) — ładuj scenę, pokazuj ekran końca itp.")]
    public UnityEvent OnSolvedDelayed;

    private bool isSolved = false;

    // -------------------------------------------------------

    public enum SolveAction
    {
        FireEvent,      // Tylko wywołaj eventy — logikę obsłuż sam
        LoadNextScene,  // Załaduj scenę po nazwie (nextSceneName)
        ReloadScene,    // Przeładuj aktualną scenę
    }

    // -------------------------------------------------------

    /// <summary>
    /// Rozwiązuje zagadkę. Podepnij do OnInteract w InteractableObject.
    /// </summary>
    public void Solve()
    {
        if (isSolved) return;
        isSolved = true;

        Debug.Log($"[Puzzle] Rozwiązano: {gameObject.name}");
        OnSolved?.Invoke();

        StartCoroutine(SolveRoutine());
    }

    private IEnumerator SolveRoutine()
    {
        yield return new WaitForSeconds(solveDelay);

        OnSolvedDelayed?.Invoke();

        switch (solveAction)
        {
            case SolveAction.LoadNextScene:
                if (!string.IsNullOrEmpty(nextSceneName))
                    SceneManager.LoadScene(nextSceneName);
                else
                    Debug.LogWarning("[Puzzle] Brak nazwy sceny do załadowania!");
                break;

            case SolveAction.ReloadScene:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            case SolveAction.FireEvent:
                // Logika w OnSolvedDelayed — nic więcej nie robimy
                break;
        }
    }

    /// <summary>Czy zagadka jest już rozwiązana?</summary>
    public bool IsSolved => isSolved;

    /// <summary>Reset — np. po restarcie poziomu.</summary>
    public void Reset() => isSolved = false;
}
