using UnityEngine;
using UnityEngine.SceneManagement;

public class PresentacionNivel2 : MonoBehaviour
{
    private float inicio;
    private Texture2D fondo;
    private GUIStyle titulo;
    private GUIStyle subtitulo;
    private GUIStyle ayuda;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CrearEnNivel2()
    {
        if (SceneManager.GetActiveScene().name != "1-2")
            return;

        if (FindFirstObjectByType<PresentacionNivel2>() != null)
            return;

        GameObject go = new GameObject("PresentacionNivel2");
        go.AddComponent<PresentacionNivel2>();
    }

    private void Awake()
    {
        inicio = Time.unscaledTime;

        fondo = new Texture2D(1, 1);
        fondo.SetPixel(0, 0, new Color(0.035f, 0.025f, 0.11f, 0.92f));
        fondo.Apply();
    }

    private void OnGUI()
    {
        PrepararEstilos();

        float tiempo = Time.unscaledTime - inicio;
        float alpha = 1f;

        if (tiempo > 4.2f)
            alpha = Mathf.Clamp01(1f - ((tiempo - 4.2f) / 1.0f));

        if (alpha <= 0f)
            return;

        Color anterior = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, alpha);

        float ancho = Mathf.Clamp(Screen.width * 0.34f, 330f, 500f);
        Rect panel = new Rect(24f, 24f, ancho, 126f);

        GUI.DrawTexture(panel, fondo);

        GUI.Label(
            new Rect(panel.x + 18f, panel.y + 12f, panel.width - 36f, 42f),
            "MUNDO 1-2",
            titulo
        );

        GUI.Label(
            new Rect(panel.x + 18f, panel.y + 48f, panel.width - 36f, 34f),
            "NIVEL SUBTERRANEO",
            subtitulo
        );

        GUI.Label(
            new Rect(panel.x + 18f, panel.y + 84f, panel.width - 36f, 26f),
            "A/D mover   ESPACIO saltar   ESC menu",
            ayuda
        );

        GUI.color = anterior;
    }

    private void PrepararEstilos()
    {
        if (titulo != null)
            return;

        titulo = new GUIStyle(GUI.skin.label);
        titulo.fontStyle = FontStyle.Bold;
        titulo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.040f, 24f, 34f));
        titulo.normal.textColor = new Color(1f, 0.78f, 0.10f);

        subtitulo = new GUIStyle(GUI.skin.label);
        subtitulo.fontStyle = FontStyle.Bold;
        subtitulo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.025f, 16f, 22f));
        subtitulo.normal.textColor = Color.white;

        ayuda = new GUIStyle(GUI.skin.label);
        ayuda.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.019f, 12f, 17f));
        ayuda.normal.textColor = new Color(0.78f, 0.82f, 0.95f);
    }
}
