using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;

    private bool usandoTuberia;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (usandoTuberia || connection == null || !other.CompareTag("Player"))
            return;

        if (Input.GetKey(enterKeyCode))
            StartCoroutine(Enter(other.transform));
    }

    private IEnumerator Enter(Transform player)
    {
        usandoTuberia = true;

        PlayerMovement movimiento = player.GetComponent<PlayerMovement>();

        if (movimiento != null)
            movimiento.enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;
        Vector3 enteredScale = Vector3.one * 0.5f;

        yield return Move(player, enteredPosition, enteredScale);
        yield return new WaitForSeconds(0.55f);

        Camera camara = Camera.main;

        if (camara == null)
            camara = FindFirstObjectByType<Camera>();

        if (camara != null)
        {
            SideScrolling sideScrolling = camara.GetComponent<SideScrolling>();

            if (sideScrolling != null)
            {
                bool vaSubterraneo = connection.position.y < sideScrolling.undergroundThreshold;
                sideScrolling.SetUnderground(vaSubterraneo);
            }
        }

        if (exitDirection != Vector3.zero)
        {
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }
        else
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        if (movimiento != null)
            movimiento.enabled = true;

        usandoTuberia = false;
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 0.75f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }
}
