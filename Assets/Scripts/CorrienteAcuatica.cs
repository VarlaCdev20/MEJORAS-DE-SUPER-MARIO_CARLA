using UnityEngine;

public class CorrienteAcuatica : MonoBehaviour
{
    public float fuerza = 7f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Rigidbody2D rb = other.attachedRigidbody;

        if (rb != null)
            rb.AddForce(Vector2.down * fuerza, ForceMode2D.Force);
    }
}
