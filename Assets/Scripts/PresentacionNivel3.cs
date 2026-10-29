using UnityEngine;
using UnityEngine.SceneManagement;

public class PresentacionNivel3 : MonoBehaviour
{
    private float inicio;
    private Texture2D fondo;
    private GUIStyle titulo;
    private GUIStyle subtitulo;
    private GUIStyle ayuda;

    private void Awake()
    {
        inicio = Time.unscaledTime;
        fondo = new Texture2D(1, 1);
        fondo.SetPixel(0, 0, new Color(0.01f, 0.08f, 0.24f, 0.90f));
        fondo.Apply();
    }

    private void OnGUI()
    {
        if (SceneManager.GetActiveScene().name != "1-3")
            return;

        PrepararEstilos();

        float tiempo = Time.unscaledTime - inicio;
        float alpha = tiempo > 4.5f ? Mathf.Clamp01(1f - (tiempo - 4.5f)) : 1f;

        if (alpha <= 0f)
            return;

        Color anterior = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, alpha);

        Rect panel = new Rect(24f, 24f, 430f, 132f);
        GUI.DrawTexture(panel, fondo);

        GUI.Label(new Rect(44f, 34f, 390f, 44f), "MUNDO 1-3", titulo);
        GUI.Label(new Rect(44f, 76f, 390f, 34f), "NIVEL ACUATICO", subtitulo);
        GUI.Label(new Rect(44f, 112f, 390f, 26f), "A/D mover   ESPACIO nadar   ESC menu", ayuda);

        GUI.color = anterior;
    }

    private void PrepararEstilos()
    {
        if (titulo != null)
            return;

        titulo = new GUIStyle(GUI.skin.label);
        titulo.fontStyle = FontStyle.Bold;
        titulo.fontSize = 30;
        titulo.normal.textColor = new Color(1f, 0.85f, 0.16f);

        subtitulo = new GUIStyle(GUI.skin.label);
        subtitulo.fontStyle = FontStyle.Bold;
        subtitulo.fontSize = 21;
        subtitulo.normal.textColor = Color.white;

        ayuda = new GUIStyle(GUI.skin.label);
        ayuda.fontSize = 15;
        ayuda.normal.textColor = new Color(0.72f, 0.90f, 1f);
    }
}
