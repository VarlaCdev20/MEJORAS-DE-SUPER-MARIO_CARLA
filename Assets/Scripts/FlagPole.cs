using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;

    private bool nivelCompletado;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (nivelCompletado || !other.CompareTag("Player"))
            return;

        nivelCompletado = true;

        if (flag != null && poleBottom != null)
            StartCoroutine(MoveTo(flag, poleBottom.position));

        StartCoroutine(LevelCompleteSequence(other.transform));
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        PlayerMovement movimiento = player.GetComponent<PlayerMovement>();

        if (movimiento != null)
            movimiento.enabled = false;

        if (poleBottom != null)
        {
            yield return MoveTo(player, poleBottom.position);
            yield return MoveTo(player, player.position + Vector3.right);
            yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        }

        if (castle != null)
        {
            yield return MoveTo(player, castle.position);
        }
        else
        {
            // Si el castillo pierde su referencia, el nivel igual termina.
            yield return MoveTo(player, player.position + Vector3.right * 2f);
        }

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.25f);

        CargarSiguienteNivel();
    }

    private void CargarSiguienteNivel()
    {
        string escenaActual = SceneManager.GetActiveScene().name;

        // La entrega necesita que 1-1 conecte siempre con 1-2.
        int mundoDestino = nextWorld;
        int nivelDestino = nextStage;

        if (escenaActual == "1-1")
        {
            mundoDestino = 1;
            nivelDestino = 2;
        }
        else if (escenaActual == "1-2")
        {
            mundoDestino = 1;
            nivelDestino = 3;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadLevel(mundoDestino, nivelDestino);
        }
        else
        {
            string nombreEscena = $"{mundoDestino}-{nivelDestino}";

            if (Application.CanStreamedLevelBeLoaded(nombreEscena))
                SceneManager.LoadScene(nombreEscena);
            else
                Debug.LogError("No se encontro la escena " + nombreEscena + " en el Build.");
        }
    }

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        if (subject == null)
            yield break;

        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(
                subject.position,
                position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        subject.position = position;
    }
}
