using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoAcuatico : MonoBehaviour
{
    public float velocidadHorizontal = 5.5f;
    public float impulsoNado = 6.5f;
    public float descenso = 2.2f;
    public float velocidadMaximaVertical = 6.5f;

    private Rigidbody2D rb;
    private PlayerMovement movimientoNormal;
    private float horizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movimientoNormal = GetComponent<PlayerMovement>();

        if (movimientoNormal != null)
            movimientoNormal.enabled = false;

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2 v = rb.linearVelocity;
            v.y = impulsoNado;
            rb.linearVelocity = v;
        }

        if (horizontal > 0.05f)
            transform.eulerAngles = Vector3.zero;
        else if (horizontal < -0.05f)
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }

    private void FixedUpdate()
    {
        Vector2 v = rb.linearVelocity;

        v.x = Mathf.MoveTowards(v.x, horizontal * velocidadHorizontal, 14f * Time.fixedDeltaTime);
        v.y = Mathf.MoveTowards(v.y, -descenso, 5f * Time.fixedDeltaTime);
        v.y = Mathf.Clamp(v.y, -velocidadMaximaVertical, velocidadMaximaVertical);

        rb.linearVelocity = v;
    }

    private void OnDisable()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}
