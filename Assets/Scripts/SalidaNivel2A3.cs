using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaNivel2A3 : MonoBehaviour
{
    private bool entrando;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void PrepararSalida()
    {
        if (SceneManager.GetActiveScene().name != "1-2")
            return;

        if (FindFirstObjectByType<SalidaNivel2A3>() != null)
            return;

        GameObject salida = new GameObject("Salida a Nivel 1-3");
        salida.transform.position = new Vector3(209f, 0f, 0f);

        BoxCollider2D col = salida.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = new Vector2(5f, 30f);

        salida.AddComponent<SalidaNivel2A3>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (entrando || !other.CompareTag("Player"))
            return;

        entrando = true;

        if (Application.CanStreamedLevelBeLoaded("1-3"))
            SceneManager.LoadScene("1-3");
        else
            Debug.LogError("No se encontro la escena 1-3 en el Build.");
    }
}
