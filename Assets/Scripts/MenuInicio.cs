using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    private GUIStyle titulo;
    private GUIStyle sombraTitulo;
    private GUIStyle subtitulo;
    private GUIStyle botonRojo;
    private GUIStyle botonVerde;
    private GUIStyle botonSalir;
    private GUIStyle interrogacion;

    private Texture2D cielo;
    private Texture2D panel;
    private Texture2D rojo;
    private Texture2D rojoHover;
    private Texture2D verde;
    private Texture2D verdeHover;
    private Texture2D gris;
    private Texture2D grisHover;
    private Texture2D nube;
    private Texture2D suelo;
    private Texture2D bloque;

    private void Awake()
    {
        cielo = CrearColor(new Color(0.22f, 0.58f, 0.95f));
        panel = CrearColor(new Color(0.08f, 0.12f, 0.22f, 0.94f));

        rojo = CrearColor(new Color(0.78f, 0.12f, 0.10f));
        rojoHover = CrearColor(new Color(0.96f, 0.22f, 0.16f));

        verde = CrearColor(new Color(0.10f, 0.58f, 0.20f));
        verdeHover = CrearColor(new Color(0.16f, 0.78f, 0.28f));

        gris = CrearColor(new Color(0.22f, 0.24f, 0.28f));
        grisHover = CrearColor(new Color(0.35f, 0.38f, 0.44f));

        nube = CrearColor(new Color(1f, 1f, 1f, 0.92f));
        suelo = CrearColor(new Color(0.48f, 0.23f, 0.08f));
        bloque = CrearColor(new Color(0.96f, 0.66f, 0.08f));
    }

    private Texture2D CrearColor(Color color)
    {
        Texture2D textura = new Texture2D(1, 1);
        textura.SetPixel(0, 0, color);
        textura.Apply();
        return textura;
    }

    private void OnGUI()
    {
        PrepararEstilos();
        DibujarFondo();

        float ancho = Mathf.Min(500f, Screen.width * 0.80f);
        float altoPanel = Mathf.Min(490f, Screen.height * 0.72f);
        float x = (Screen.width - ancho) * 0.5f;
        float y = Screen.height * 0.12f;

        GUI.DrawTexture(
            new Rect(x - 35f, y, ancho + 70f, altoPanel),
            panel
        );

        GUI.Label(
            new Rect(x + 4f, y + 30f, ancho, 78f),
            "SUPER MARIO",
            sombraTitulo
        );

        GUI.Label(
            new Rect(x, y + 25f, ancho, 78f),
            "SUPER MARIO",
            titulo
        );

        GUI.Label(
            new Rect(x, y + 100f, ancho, 44f),
            "MEJORAS DE CARLA",
            subtitulo
        );

        float botonY = y + 175f;

        if (GUI.Button(
            new Rect(x, botonY, ancho, 66f),
            "NIVEL 1  -  MUNDO CLASICO",
            botonRojo))
        {
            IrNivel1();
        }

        if (GUI.Button(
            new Rect(x, botonY + 88f, ancho, 66f),
            "NIVEL 2  -  SUBTERRANEO",
            botonVerde))
        {
            IrNivel2();
        }

        if (GUI.Button(
            new Rect(x + ancho * 0.20f, botonY + 186f, ancho * 0.60f, 52f),
            "SALIR",
            botonSalir))
        {
            Salir();
        }

        GUI.Label(
            new Rect(x, y + altoPanel - 58f, ancho, 36f),
            "Elige un nivel para comenzar",
            subtitulo
        );
    }

    private void DibujarFondo()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), cielo);

        DibujarNube(Screen.width * 0.10f, Screen.height * 0.14f, 1.0f);
        DibujarNube(Screen.width * 0.72f, Screen.height * 0.21f, 0.85f);
        DibujarNube(Screen.width * 0.82f, Screen.height * 0.08f, 0.55f);

        float pisoY = Screen.height - 72f;
        GUI.DrawTexture(new Rect(0, pisoY, Screen.width, 72f), suelo);

        float tamanoBloque = 46f;
        for (float bx = 0; bx < Screen.width; bx += tamanoBloque)
        {
            GUI.DrawTexture(
                new Rect(bx, pisoY - tamanoBloque, tamanoBloque - 2f, tamanoBloque - 2f),
                bloque
            );

            GUI.Label(
                new Rect(bx, pisoY - tamanoBloque - 2f, tamanoBloque - 2f, tamanoBloque),
                "?",
                interrogacion
            );
        }
    }

    private void DibujarNube(float x, float y, float escala)
    {
        float w = 58f * escala;
        float h = 26f * escala;

        GUI.DrawTexture(new Rect(x, y + h * 0.35f, w * 2.2f, h), nube);
        GUI.DrawTexture(new Rect(x + w * 0.35f, y, w, h * 1.25f), nube);
        GUI.DrawTexture(new Rect(x + w * 1.05f, y + h * 0.12f, w * 0.9f, h * 1.1f), nube);
    }

    private void PrepararEstilos()
    {
        if (titulo != null)
            return;

        titulo = new GUIStyle(GUI.skin.label);
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.fontStyle = FontStyle.Bold;
        titulo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.075f, 40f, 68f));
        titulo.normal.textColor = new Color(1f, 0.84f, 0.08f);

        sombraTitulo = new GUIStyle(titulo);
        sombraTitulo.normal.textColor = new Color(0.45f, 0.05f, 0.04f);

        subtitulo = new GUIStyle(GUI.skin.label);
        subtitulo.alignment = TextAnchor.MiddleCenter;
        subtitulo.fontStyle = FontStyle.Bold;
        subtitulo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.027f, 17f, 25f));
        subtitulo.normal.textColor = Color.white;

        botonRojo = CrearEstiloBoton(rojo, rojoHover);
        botonVerde = CrearEstiloBoton(verde, verdeHover);
        botonSalir = CrearEstiloBoton(gris, grisHover);

        interrogacion = new GUIStyle(GUI.skin.label);
        interrogacion.alignment = TextAnchor.MiddleCenter;
        interrogacion.fontStyle = FontStyle.Bold;
        interrogacion.fontSize = 27;
        interrogacion.normal.textColor = Color.white;
    }

    private GUIStyle CrearEstiloBoton(Texture2D normal, Texture2D hover)
    {
        GUIStyle estilo = new GUIStyle(GUI.skin.button);
        estilo.fontStyle = FontStyle.Bold;
        estilo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.032f, 19f, 29f));
        estilo.normal.textColor = Color.white;
        estilo.hover.textColor = Color.white;
        estilo.active.textColor = Color.white;
        estilo.normal.background = normal;
        estilo.hover.background = hover;
        estilo.active.background = hover;
        estilo.padding = new RectOffset(16, 16, 10, 10);
        return estilo;
    }

    public void IrNivel1()
    {
        CargarNivel("1-1");
    }

    public void IrNivel2()
    {
        CargarNivel("1-2");
    }

    private void CargarNivel(string escena)
    {
        Time.timeScale = 1f;

        if (Application.CanStreamedLevelBeLoaded(escena))
        {
            SceneManager.LoadScene(escena, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("No se encontro la escena " + escena + " en el Build.");
        }
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
