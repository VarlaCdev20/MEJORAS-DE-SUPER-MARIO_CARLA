using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraNivel2 : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void PrepararCamaraNivel2()
    {
        if (SceneManager.GetActiveScene().name != "1-2")
            return;

        Camera camara = Camera.main;

        if (camara == null)
            camara = Object.FindFirstObjectByType<Camera>();

        if (camara == null)
        {
            GameObject objetoCamara = new GameObject("Main Camera");
            objetoCamara.tag = "MainCamera";
            camara = objetoCamara.AddComponent<Camera>();
            camara.orthographic = true;
            camara.orthographicSize = 6.5f;
            camara.backgroundColor = new Color(0.03f, 0.03f, 0.10f, 1f);
            camara.transform.position = new Vector3(0f, 6.5f, -10f);

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

        SideScrolling seguimiento = camara.GetComponent<SideScrolling>();

        if (seguimiento == null)
            seguimiento = camara.gameObject.AddComponent<SideScrolling>();

        // En 1-2 dejamos la altura que ya tenga la camara para no saltar
        // a coordenadas del nivel 1 cuando usamos tuberias.
        seguimiento.height = camara.transform.position.y;
        seguimiento.undergroundHeight = camara.transform.position.y;
    }
}
