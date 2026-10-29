#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[InitializeOnLoad]
public static class ConstruirMenuEditable
{
    private const string RutaMenu = "Assets/Scenes/Menu.unity";
    private const string NombreCanvas = "MenuEditableCanvas";

    static ConstruirMenuEditable()
    {
        EditorApplication.delayCall += CrearSiHaceFalta;
    }

    [MenuItem("Mario Carla/Crear o reparar menu editable")]
    public static void CrearSiHaceFalta()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        Scene escenaOriginal = SceneManager.GetActiveScene();
        Scene escenaMenu = default;
        bool yaEstabaAbierta = false;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene escena = SceneManager.GetSceneAt(i);
            if (escena.path == RutaMenu)
            {
                escenaMenu = escena;
                yaEstabaAbierta = true;
                break;
            }
        }

        if (!yaEstabaAbierta)
            escenaMenu = EditorSceneManager.OpenScene(RutaMenu, OpenSceneMode.Additive);

        GameObject canvasExistente = BuscarRaiz(escenaMenu, NombreCanvas);

        if (canvasExistente != null)
        {
            if (!yaEstabaAbierta)
                EditorSceneManager.CloseScene(escenaMenu, true);

            if (escenaOriginal.IsValid() && escenaOriginal.isLoaded)
                SceneManager.SetActiveScene(escenaOriginal);

            return;
        }

        SceneManager.SetActiveScene(escenaMenu);

        GameObject controlador = BuscarRaiz(escenaMenu, "MenuInicio");
        if (controlador == null)
            controlador = new GameObject("MenuInicio");

        MenuInicio menu = controlador.GetComponent<MenuInicio>();
        if (menu == null)
            menu = controlador.AddComponent<MenuInicio>();

        CrearCamara();
        CrearEventSystem();
        CrearCanvas(menu);

        EditorSceneManager.MarkSceneDirty(escenaMenu);
        EditorSceneManager.SaveScene(escenaMenu);

        Debug.Log("Menu editable creado: ahora puedes modificar textos y botones desde la Hierarchy.");

        if (!yaEstabaAbierta)
            EditorSceneManager.CloseScene(escenaMenu, true);

        if (escenaOriginal.IsValid() && escenaOriginal.isLoaded)
            SceneManager.SetActiveScene(escenaOriginal);
    }

    private static GameObject BuscarRaiz(Scene escena, string nombre)
    {
        foreach (GameObject raiz in escena.GetRootGameObjects())
        {
            if (raiz.name == nombre)
                return raiz;
        }

        return null;
    }

    private static void CrearCamara()
    {
        Camera camara = Object.FindFirstObjectByType<Camera>();

        if (camara != null)
        {
            camara.gameObject.tag = "MainCamera";
            return;
        }

        GameObject go = new GameObject("Main Camera");
        go.tag = "MainCamera";

        Camera nueva = go.AddComponent<Camera>();
        nueva.orthographic = true;
        nueva.orthographicSize = 5f;
        nueva.backgroundColor = new Color(0.18f, 0.52f, 0.90f);
        go.transform.position = new Vector3(0f, 0f, -10f);

        if (Object.FindFirstObjectByType<AudioListener>() == null)
            go.AddComponent<AudioListener>();
    }

    private static void CrearEventSystem()
    {
        if (Object.FindFirstObjectByType<EventSystem>() != null)
            return;

        GameObject go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();
        go.AddComponent<StandaloneInputModule>();
    }

    private static void CrearCanvas(MenuInicio menu)
    {
        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        GameObject canvasGO = new GameObject(
            NombreCanvas,
            typeof(RectTransform),
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster)
        );

        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.matchWidthOrHeight = 0.5f;

        CrearImagen(
            canvasGO.transform,
            "FondoAzul",
            new Color(0.18f, 0.52f, 0.90f),
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero
        );

        GameObject panel = CrearImagen(
            canvasGO.transform,
            "PanelPrincipal",
            new Color(0.04f, 0.06f, 0.12f, 0.94f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 15f),
            new Vector2(620f, 540f)
        );

        CrearTexto(
            panel.transform,
            "Titulo",
            "SUPER MARIO",
            fuente,
            58,
            FontStyle.Bold,
            new Color(1f, 0.82f, 0.08f),
            new Vector2(0f, 185f),
            new Vector2(560f, 90f)
        );

        CrearTexto(
            panel.transform,
            "Subtitulo",
            "ELIGE TU AVENTURA",
            fuente,
            24,
            FontStyle.Bold,
            Color.white,
            new Vector2(0f, 125f),
            new Vector2(500f, 50f)
        );

        CrearTexto(
            panel.transform,
            "Firma",
            "CARLA EDITION",
            fuente,
            18,
            FontStyle.Bold,
            new Color(1f, 0.64f, 0.05f),
            new Vector2(0f, 85f),
            new Vector2(300f, 40f)
        );

        CrearBoton(
            panel.transform,
            "BotonNivel1",
            "NIVEL 1 - MUNDO CLASICO",
            fuente,
            new Color(0.78f, 0.12f, 0.10f),
            new Vector2(0f, 10f),
            menu.IrNivel1
        );

        CrearBoton(
            panel.transform,
            "BotonNivel2",
            "NIVEL 2 - SUBTERRANEO",
            fuente,
            new Color(0.08f, 0.56f, 0.18f),
            new Vector2(0f, -80f),
            menu.IrNivel2
        );

        CrearBoton(
            panel.transform,
            "BotonSalir",
            "SALIR",
            fuente,
            new Color(0.22f, 0.24f, 0.28f),
            new Vector2(0f, -180f),
            menu.Salir,
            new Vector2(300f, 58f)
        );

        CrearTexto(
            panel.transform,
            "Ayuda",
            "Selecciona cualquier texto en la Hierarchy para editarlo",
            fuente,
            16,
            FontStyle.Normal,
            new Color(0.85f, 0.88f, 0.95f),
            new Vector2(0f, -245f),
            new Vector2(550f, 35f)
        );
    }

    private static GameObject CrearImagen(
        Transform padre,
        string nombre,
        Color color,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 posicion,
        Vector2 tamano)
    {
        GameObject go = new GameObject(nombre, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(padre, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.anchoredPosition = posicion;
        rt.sizeDelta = tamano;

        Image imagen = go.GetComponent<Image>();
        imagen.color = color;

        return go;
    }

    private static Text CrearTexto(
        Transform padre,
        string nombre,
        string contenido,
        Font fuente,
        int tamanoFuente,
        FontStyle estilo,
        Color color,
        Vector2 posicion,
        Vector2 tamano)
    {
        GameObject go = new GameObject(nombre, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(padre, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = posicion;
        rt.sizeDelta = tamano;

        Text texto = go.GetComponent<Text>();
        texto.text = contenido;
        texto.font = fuente;
        texto.fontSize = tamanoFuente;
        texto.fontStyle = estilo;
        texto.alignment = TextAnchor.MiddleCenter;
        texto.color = color;
        texto.horizontalOverflow = HorizontalWrapMode.Overflow;
        texto.verticalOverflow = VerticalWrapMode.Overflow;

        return texto;
    }

    private static void CrearBoton(
        Transform padre,
        string nombre,
        string textoBoton,
        Font fuente,
        Color color,
        Vector2 posicion,
        UnityEngine.Events.UnityAction accion,
        Vector2? tamano = null)
    {
        GameObject go = new GameObject(nombre, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(padre, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = posicion;
        rt.sizeDelta = tamano ?? new Vector2(500f, 68f);

        Image imagen = go.GetComponent<Image>();
        imagen.color = color;

        Button boton = go.GetComponent<Button>();
        ColorBlock colores = boton.colors;
        colores.normalColor = color;
        colores.highlightedColor = Color.Lerp(color, Color.white, 0.18f);
        colores.pressedColor = Color.Lerp(color, Color.black, 0.15f);
        colores.selectedColor = colores.highlightedColor;
        boton.colors = colores;

        Text texto = CrearTexto(
            go.transform,
            "Texto",
            textoBoton,
            fuente,
            23,
            FontStyle.Bold,
            Color.white,
            Vector2.zero,
            rt.sizeDelta
        );

        RectTransform trTexto = texto.rectTransform;
        trTexto.anchorMin = Vector2.zero;
        trTexto.anchorMax = Vector2.one;
        trTexto.offsetMin = Vector2.zero;
        trTexto.offsetMax = Vector2.zero;

        UnityEventTools.AddPersistentListener(boton.onClick, accion);
    }
}
#endif
