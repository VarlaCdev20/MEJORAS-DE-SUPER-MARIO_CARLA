using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PezAcuatico : MonoBehaviour
{
    public float velocidad = 2.2f;
    public float distancia = 6f;
    public float amplitudVertical = 0.45f;
    public float frecuencia = 1.6f;

    private Vector3 origen;
    private int direccion = -1;
    private static Sprite spritePez;

    private void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr.sprite == null)
        {
            sr.sprite = ObtenerSpritePez();
            sr.sortingOrder = 6;
        }

        sr.color = new Color(1f, 0.38f, 0.18f);
    }

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

    private static Sprite ObtenerSpritePez()
    {
        if (spritePez != null)
            return spritePez;

        const int ancho = 16;
        const int alto = 12;

        Texture2D tex = new Texture2D(ancho, alto, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;

        Color transparente = new Color(0f, 0f, 0f, 0f);
        Color cuerpo = Color.white;
        Color ojo = new Color(0.05f, 0.05f, 0.08f);

        Color[] pixeles = new Color[ancho * alto];

        for (int i = 0; i < pixeles.Length; i++)
            pixeles[i] = transparente;

        for (int y = 3; y <= 8; y++)
        {
            for (int x = 4; x <= 12; x++)
            {
                float nx = (x - 8f) / 5f;
                float ny = (y - 5.5f) / 3.3f;

                if (nx * nx + ny * ny <= 1f)
                    pixeles[y * ancho + x] = cuerpo;
            }
        }

        // cola
        pixeles[4 * ancho + 2] = cuerpo;
        pixeles[5 * ancho + 1] = cuerpo;
        pixeles[5 * ancho + 2] = cuerpo;
        pixeles[6 * ancho + 1] = cuerpo;
        pixeles[6 * ancho + 2] = cuerpo;
        pixeles[7 * ancho + 2] = cuerpo;

        // aleta
        pixeles[9 * ancho + 8] = cuerpo;
        pixeles[9 * ancho + 9] = cuerpo;

        // ojo
        pixeles[6 * ancho + 11] = ojo;

        tex.SetPixels(pixeles);
        tex.Apply();

        spritePez = Sprite.Create(
            tex,
            new Rect(0, 0, ancho, alto),
            new Vector2(0.5f, 0.5f),
            16f
        );

        spritePez.name = "PezAcuaticoPixel";
        return spritePez;
    }
}
