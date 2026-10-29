using UnityEngine;

public class PezAcuatico : MonoBehaviour
{
    public float velocidad = 2.2f;
    public float distancia = 6f;
    public float amplitudVertical = 0.45f;
    public float frecuencia = 1.6f;

    private Vector3 origen;
    private int direccion = -1;

    private void Start()
    {
        origen = transform.position;
    }

    private void Update()
    {
        float limiteIzq = origen.x - distancia * 0.5f;
        float limiteDer = origen.x + distancia * 0.5f;

        transform.Translate(Vector3.right * direccion * velocidad * Time.deltaTime, Space.World);

        Vector3 p = transform.position;
        p.y = origen.y + Mathf.Sin(Time.time * frecuencia + origen.x) * amplitudVertical;
        transform.position = p;

        if (transform.position.x <= limiteIzq)
            direccion = 1;
        else if (transform.position.x >= limiteDer)
            direccion = -1;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = direccion > 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Player player = other.GetComponentInParent<Player>();

        if (player != null)
            player.Hit();
    }
}
