using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraNivel2 : MonoBehaviour
{
    private Transform jugador;
    private float yFijo;
    private float xMinimo;
    private const float OffsetX = 3f;
    private const float Suavizado = 8f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void PrepararCamaraNivel2()
    {
        if (SceneManager.GetActiveScene().name != "1-2")
            return;

        GameObject mario = GameObject.FindGameObjectWithTag("Player");
        if (mario == null)
        {
            Debug.LogWarning("Nivel 1-2: no se encontro un objeto con tag Player.");
            return;
        }

        Camera camara = Camera.main;
        bool camaraCreada = false;

        if (camara == null)
        {
            camara = Object.FindFirstObjectByType<Camera>();
        }

        if (camara == null)
        {
            GameObject objetoCamara = new GameObject("Main Camera");
            objetoCamara.tag = "MainCamera";
            camara = objetoCamara.AddComponent<Camera>();
            camaraCreada = true;

            if (Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None).Length == 0)
                objetoCamara.AddComponent<AudioListener>();
        }
        else
        {
            camara.gameObject.tag = "MainCamera";
        }

        camara.orthographic = true;
        camara.orthographicSize = 6.5f;

        if (camaraCreada)
        {
            camara.backgroundColor = new Color(0.03f, 0.03f, 0.10f, 1f);
            camara.transform.position = new Vector3(
                mario.transform.position.x + OffsetX,
                mario.transform.position.y + 3.5f,
                -10f
            );
        }
        else
        {
            Vector3 posicion = camara.transform.position;
            posicion.z = -10f;
            camara.transform.position = posicion;
        }

        // Si la escena ya tiene la camara original con SideScrolling,
        // no agregamos un segundo seguimiento.
        if (camara.GetComponent<SideScrolling>() != null)
            return;

        CamaraNivel2 seguimiento = camara.GetComponent<CamaraNivel2>();
        if (seguimiento == null)
            seguimiento = camara.gameObject.AddComponent<CamaraNivel2>();

        seguimiento.Configurar(mario.transform);
    }

    private void Configurar(Transform objetivo)
    {
        jugador = objetivo;
        yFijo = transform.position.y;
        xMinimo = transform.position.x;
    }

    private void LateUpdate()
    {
        if (jugador == null)
            return;

        float objetivoX = Mathf.Max(xMinimo, jugador.position.x + OffsetX);

        Vector3 destino = new Vector3(
            objetivoX,
            yFijo,
            -10f
        );

        transform.position = Vector3.Lerp(
            transform.position,
            destino,
            Suavizado * Time.deltaTime
        );
    }
}
