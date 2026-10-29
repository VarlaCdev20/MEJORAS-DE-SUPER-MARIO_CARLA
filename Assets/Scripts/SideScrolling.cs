using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrolling : MonoBehaviour
{
    private new Camera camera;
    private Transform player;

    private float velocidadX;
    private float ultimoPlayerX;

    public float height = 6.5f;
    public float undergroundHeight = -9.5f;
    public float undergroundThreshold = 0f;

    [Header("Seguimiento suave")]
    public float smoothTime = 0.12f;
    public float lookAhead = 2.2f;

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
            ultimoPlayerX = player.position.x;
            return;
        }

        Player componentePlayer = FindFirstObjectByType<Player>();
        if (componentePlayer != null)
        {
            player = componentePlayer.transform;
            ultimoPlayerX = player.position.x;
            return;
        }

        GameObject mario = GameObject.Find("Mario");
        if (mario != null)
        {
            player = mario.transform;
            ultimoPlayerX = player.position.x;
        }
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            BuscarJugador();

            if (player == null)
                return;
        }

        float movimiento = player.position.x - ultimoPlayerX;
        float adelanto = movimiento > 0.01f ? lookAhead : 0f;

        float objetivoX = Mathf.Max(
            transform.position.x,
            player.position.x + adelanto
        );

        float nuevoX = Mathf.SmoothDamp(
            transform.position.x,
            objetivoX,
            ref velocidadX,
            smoothTime
        );

        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(transform.position.x, nuevoX);
        cameraPosition.z = -10f;
        transform.position = cameraPosition;

        ultimoPlayerX = player.position.x;
    }

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        cameraPosition.z = -10f;
        transform.position = cameraPosition;

        velocidadX = 0f;
    }
}
