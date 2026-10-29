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
            camara.orthographic = true;
            camara.orthographicSize = 6.5f;
            camara.transform.position = new Vector3(0f, 6.5f, -10f);
            creada = true;

            if (Object.FindFirstObjectByType<AudioListener>() == null)
                objetoCamara.AddComponent<AudioListener>();
        }
        else
        {
            camara.gameObject.tag = "MainCamera";
            camara.orthographic = true;

            Vector3 posicion = camara.transform.position;
            posicion.z = -10f;
            camara.transform.position = posicion;
        }

        if (escena == "1-2")
            camara.backgroundColor = new Color(0.03f, 0.03f, 0.10f, 1f);

        SideScrolling seguimiento = camara.GetComponent<SideScrolling>();

        if (seguimiento == null)
            seguimiento = camara.gameObject.AddComponent<SideScrolling>();

        if (escena == "1-2")
        {
            seguimiento.height = camara.transform.position.y;
            seguimiento.undergroundHeight = camara.transform.position.y;
        }
        else if (creada)
        {
            seguimiento.height = 6.5f;
        }
    }
}
