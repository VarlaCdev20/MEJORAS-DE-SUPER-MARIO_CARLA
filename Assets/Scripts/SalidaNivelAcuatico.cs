using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaNivelAcuatico : MonoBehaviour
{
    private bool saliendo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (saliendo || !other.CompareTag("Player"))
            return;

        saliendo = true;

        if (Application.CanStreamedLevelBeLoaded("1-4"))
            SceneManager.LoadScene("1-4");
        else
            SceneManager.LoadScene("Menu");
    }
}
