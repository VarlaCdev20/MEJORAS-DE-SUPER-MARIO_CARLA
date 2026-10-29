using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    private GUIStyle titulo;
    private GUIStyle subtitulo;
    private GUIStyle boton;

    private void OnGUI()
    {
        PrepararEstilos();

        float ancho = Mathf.Min(420f, Screen.width * 0.78f);
        float x = (Screen.width - ancho) * 0.5f;

        GUI.Box(new Rect(x - 35f, Screen.height * 0.18f, ancho + 70f, 390f), "");

        GUI.Label(
            new Rect(x, Screen.height * 0.22f, ancho, 75f),
            "SUPER MARIO",
            titulo
        );

        GUI.Label(
            new Rect(x, Screen.height * 0.32f, ancho, 50f),
            "Elige tu aventura",
            subtitulo
        );

        if (GUI.Button(new Rect(x, Screen.height * 0.43f, ancho, 62f), "NIVEL 1", boton))
            CargarNivel("1-1");

        if (GUI.Button(new Rect(x, Screen.height * 0.55f, ancho, 62f), "NIVEL 2", boton))
            CargarNivel("1-2");

        if (GUI.Button(new Rect(x, Screen.height * 0.67f, ancho, 54f), "SALIR", boton))
            Salir();
    }

    private void PrepararEstilos()
    {
        if (titulo != null)
            return;

        titulo = new GUIStyle(GUI.skin.label);
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.fontStyle = FontStyle.Bold;
        titulo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.07f, 34f, 60f));
        titulo.normal.textColor = new Color(1f, 0.85f, 0.2f);

        subtitulo = new GUIStyle(GUI.skin.label);
        subtitulo.alignment = TextAnchor.MiddleCenter;
        subtitulo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.032f, 18f, 28f));
        subtitulo.normal.textColor = Color.white;

        boton = new GUIStyle(GUI.skin.button);
        boton.fontStyle = FontStyle.Bold;
        boton.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.035f, 20f, 30f));
    }

    private void CargarNivel(string escena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escena);
    }

    private void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
