using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraNivel2 : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void PrepararCamaras()
    {
        string escena = SceneManager.GetActiveScene().name;

        if (escena != "1-1" && escena != "1-2")
            return;

        Camera camara = Camera.main;

        if (camara == null)
            camara = Object.FindFirstObjectByType<Camera>();

        bool creada = false;

        if (camara == null)
        {
            GameObject objetoCamara = new GameObject("Main Camera");
            objetoCamara.tag = "MainCamera";

            camara = objetoCamara.AddComponent<Camera>();
            camara.transform.position = new Vector3(0f, 6.5f, -10f);
            creada = true;

            if (Object.FindFirstObjectByType<AudioListener>() == null)
                objetoCamara.AddComponent<AudioListener>();
        }
        else
        {
            camara.gameObject.tag = "MainCamera";

            Vector3 posicion = camara.transform.position;
            posicion.z = -10f;
            camara.transform.position = posicion;
        }

        camara.orthographic = true;

        if (escena == "1-2")
        {
            // Un poco mas abierta para poder leer mejor los obstaculos.
            camara.orthographicSize = 7f;
            camara.backgroundColor = new Color(0.025f, 0.025f, 0.085f, 1f);
        }
        else
        {
            camara.orthographicSize = 6.5f;
        }

        SideScrolling seguimiento = camara.GetComponent<SideScrolling>();

        if (seguimiento == null)
            seguimiento = camara.gameObject.AddComponent<SideScrolling>();

        seguimiento.smoothTime = escena == "1-2" ? 0.10f : 0.12f;
        seguimiento.lookAhead = escena == "1-2" ? 2.6f : 2.2f;

        if (escena == "1-2")
        {
            // Mantenemos la altura real de tu nivel subterraneo.
            seguimiento.height = camara.transform.position.y;
            seguimiento.undergroundHeight = camara.transform.position.y;
        }
        else if (creada)
        {
            seguimiento.height = 6.5f;
        }
    }
}
