using UnityEngine;

public class CamaraAcuatica : MonoBehaviour
{
    public Transform jugador;
    public Vector2 offset = new Vector2(3.2f, 0.5f);
    public float suavizado = 0.16f;
    public float minimoX = 0f;
    public float maximoX = 95f;
    public float minimoY = -1.5f;
    public float maximoY = 6.5f;

    private Vector3 velocidad;

    private void LateUpdate()
    {
        if (jugador == null)
        {
            Player player = FindFirstObjectByType<Player>();
            if (player != null)
                jugador = player.transform;

            if (jugador == null)
                return;
        }

        Vector3 objetivo = new Vector3(
            Mathf.Clamp(jugador.position.x + offset.x, minimoX, maximoX),
            Mathf.Clamp(jugador.position.y + offset.y, minimoY, maximoY),
            -10f
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            objetivo,
            ref velocidad,
            suavizado
        );
    }
}
