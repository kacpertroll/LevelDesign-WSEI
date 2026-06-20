using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Death"))
        {
            Debug.Log("You died - Level restart");
            SceneManager.LoadScene("LevelDesign_Task1");
        }
    }
}