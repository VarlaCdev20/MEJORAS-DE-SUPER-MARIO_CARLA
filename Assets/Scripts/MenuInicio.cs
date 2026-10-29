using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    private GUIStyle titulo;
    private GUIStyle sombraTitulo;
    private GUIStyle subtitulo;
    private GUIStyle etiqueta;
    private GUIStyle botonRojo;
    private GUIStyle botonVerde;
    private GUIStyle botonSalir;
    private GUIStyle interrogacion;

    private Texture2D cielo;
    private Texture2D panel;
    private Texture2D panelSecundario;
    private Texture2D rojo;
    private Texture2D rojoHover;
    private Texture2D verde;
    private Texture2D verdeHover;
    private Texture2D gris;
    private Texture2D grisHover;
    private Texture2D nube;
    private Texture2D suelo;
    private Texture2D bloque;
    private Texture2D amarillo;

    private void Awake()
    {
        cielo = CrearColor(new Color(0.20f, 0.55f, 0.94f));
        panel = CrearColor(new Color(0.06f, 0.08f, 0.16f, 0.96f));
        panelSecundario = CrearColor(new Color(0.10f, 0.14f, 0.25f, 0.96f));

        rojo = CrearColor(new Color(0.78f, 0.10f, 0.08f));
        rojoHover = CrearColor(new Color(0.97f, 0.22f, 0.14f));

        verde = CrearColor(new Color(0.08f, 0.55f, 0.18f));
        verdeHover = CrearColor(new Color(0.14f, 0.76f, 0.25f));

        gris = CrearColor(new Color(0.20f, 0.22f, 0.27f));
        grisHover = CrearColor(new Color(0.34f, 0.37f, 0.44f));

        nube = CrearColor(new Color(1f, 1f, 1f, 0.94f));
        suelo = CrearColor(new Color(0.47f, 0.22f, 0.07f));
        bloque = CrearColor(new Color(0.95f, 0.62f, 0.05f));
        amarillo = CrearColor(new Color(1f, 0.82f, 0.08f));
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

        float ancho = Mathf.Min(560f, Screen.width * 0.82f);
        float altoPanel = Mathf.Min(535f, Screen.height * 0.78f);
        float x = (Screen.width - ancho) * 0.5f;
        float y = Mathf.Max(38f, Screen.height * 0.08f);

        float flotacion = Mathf.Sin(Time.unscaledTime * 1.8f) * 3f;

        GUI.DrawTexture(
            new Rect(x - 38f, y + flotacion, ancho + 76f, altoPanel),
            panel
        );

        GUI.DrawTexture(
            new Rect(x + ancho * 0.31f, y + 16f + flotacion, ancho * 0.38f, 34f),
            amarillo
        );

        GUI.Label(
            new Rect(x + ancho * 0.31f, y + 16f + flotacion, ancho * 0.38f, 34f),
            "CARLA EDITION",
            etiqueta
        );

        GUI.Label(
            new Rect(x + 5f, y + 55f + flotacion, ancho, 82f),
            "SUPER MARIO",
            sombraTitulo
        );

        GUI.Label(
            new Rect(x, y + 49f + flotacion, ancho, 82f),
            "SUPER MARIO",
            titulo
        );

        GUI.Label(
            new Rect(x, y + 126f + flotacion, ancho, 42f),
            "ELIGE TU AVENTURA",
            subtitulo
        );

        float tarjetaY = y + 185f + flotacion;

        DibujarTarjeta(
            new Rect(x, tarjetaY, ancho, 92f),
            "NIVEL 1",
            "Mundo clasico",
            botonRojo,
            IrNivel1
        );

        DibujarTarjeta(
            new Rect(x, tarjetaY + 108f, ancho, 92f),
            "NIVEL 2",
            "Subterraneo",
            botonVerde,
            IrNivel2
        );

        if (GUI.Button(
            new Rect(x + ancho * 0.24f, tarjetaY + 226f, ancho * 0.52f, 52f),
            "SALIR",
            botonSalir))
        {
            Salir();
        }

        GUI.Label(
            new Rect(x, y + altoPanel - 48f, ancho, 34f),
            "Puedes volver al menu desde cada nivel",
            subtitulo
        );
    }

    private void DibujarTarjeta(Rect area, string nivel, string descripcion, GUIStyle estilo, System.Action accion)
    {
        GUI.DrawTexture(area, panelSecundario);

        Rect boton = new Rect(area.x + 12f, area.y + 12f, area.width - 24f, 68f);

        if (GUI.Button(boton, nivel + "  -  " + descripcion.ToUpper(), estilo))
            accion?.Invoke();
    }

    private void DibujarFondo()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), cielo);

        float movimiento = Mathf.Repeat(Time.unscaledTime * 12f, Screen.width + 220f);

        DibujarNube(Screen.width * 0.08f + movimiento * 0.08f, Screen.height * 0.14f, 1.0f);
        DibujarNube(Screen.width * 0.68f - movimiento * 0.05f, Screen.height * 0.20f, 0.82f);
        DibujarNube(Screen.width * 0.80f, Screen.height * 0.08f, 0.55f);

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
        titulo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.078f, 42f, 72f));
        titulo.normal.textColor = new Color(1f, 0.84f, 0.08f);

        sombraTitulo = new GUIStyle(titulo);
        sombraTitulo.normal.textColor = new Color(0.42f, 0.04f, 0.03f);

        subtitulo = new GUIStyle(GUI.skin.label);
        subtitulo.alignment = TextAnchor.MiddleCenter;
        subtitulo.fontStyle = FontStyle.Bold;
        subtitulo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.024f, 16f, 23f));
        subtitulo.normal.textColor = Color.white;

        etiqueta = new GUIStyle(GUI.skin.label);
        etiqueta.alignment = TextAnchor.MiddleCenter;
        etiqueta.fontStyle = FontStyle.Bold;
        etiqueta.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.020f, 13f, 18f));
        etiqueta.normal.textColor = new Color(0.15f, 0.08f, 0.02f);

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
        estilo.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.030f, 18f, 28f));
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
