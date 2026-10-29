using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrolling : MonoBehaviour
{
    private new Camera camera;
    private Transform player;

    public float height = 6.5f;
    public float undergroundHeight = -9.5f;
    public float undergroundThreshold = 0f;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        BuscarJugador();
    }

    private void BuscarJugador()
    {
        GameObject encontrado = null;

        try
        {
            encontrado = GameObject.FindWithTag("Player");
        }
        catch
        {
            // Si el tag Player no existe, probamos otras opciones.
        }

        if (encontrado != null)
        {
            player = encontrado.transform;
            return;
        }

        Player componentePlayer = FindFirstObjectByType<Player>();
        if (componentePlayer != null)
        {
            player = componentePlayer.transform;
            return;
        }

        GameObject mario = GameObject.Find("Mario");
        if (mario != null)
            player = mario.transform;
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            BuscarJugador();

            if (player == null)
                return;
        }

        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
        cameraPosition.z = -10f;
        transform.position = cameraPosition;
    }

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        cameraPosition.z = -10f;
        transform.position = cameraPosition;
    }
}
