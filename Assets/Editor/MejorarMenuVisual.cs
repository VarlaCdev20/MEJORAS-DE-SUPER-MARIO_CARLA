#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[InitializeOnLoad]
public static class MejorarMenuVisual
{
    private const string RutaMenu = "Assets/Scenes/Menu.unity";
    private const string NombreCanvas = "MenuEditableCanvas";
    private const string NombreDecoracion = "DecoracionMario";

    static MejorarMenuVisual()
    {
        EditorApplication.delayCall += Aplicar;
    }

    [MenuItem("Mario Carla/Mejorar visual del menu")]
    public static void Aplicar()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        ConstruirMenuEditable.CrearSiHaceFalta();

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

        SceneManager.SetActiveScene(escenaMenu);

        GameObject canvas = BuscarRaiz(escenaMenu, NombreCanvas);

        if (canvas == null)
        {
            Debug.LogWarning("No se encontro el Canvas del menu.");
            CerrarYVolver(escenaMenu, escenaOriginal, yaEstabaAbierta);
            return;
        }

        Transform decoracion = canvas.transform.Find(NombreDecoracion);

        if (decoracion == null)
        {
            GameObject contenedor = new GameObject(NombreDecoracion, typeof(RectTransform));
            contenedor.transform.SetParent(canvas.transform, false);

            RectTransform rt = contenedor.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            decoracion = contenedor.transform;
            decoracion.SetSiblingIndex(Mathf.Min(1, canvas.transform.childCount - 1));
        }

        CrearOActualizarSprite(
            decoracion,
            "NubeIzquierda",
            CargarSprite("Assets/Sprites/Cloud1.png"),
            new Vector2(0.12f, 0.78f),
            new Vector2(210f, 95f)
        );

        CrearOActualizarSprite(
            decoracion,
            "NubeDerecha",
            CargarSprite("Assets/Sprites/Cloud2.png"),
            new Vector2(0.86f, 0.72f),
            new Vector2(230f, 105f)
        );

        CrearOActualizarSprite(
            decoracion,
            "MarioDecorativo",
            CargarSprite("Assets/Sprites/Mario_Small_Idle.png"),
            new Vector2(0.13f, 0.18f),
            new Vector2(105f, 145f)
        );

        CrearOActualizarSprite(
            decoracion,
            "BloquePreguntaIzquierda",
            CargarSprite("Assets/Sprites/MysteryBlock.png"),
            new Vector2(0.24f, 0.20f),
            new Vector2(75f, 75f)
        );

        CrearOActualizarSprite(
            decoracion,
            "BloquePreguntaDerecha",
            CargarSprite("Assets/Sprites/MysteryBlock.png"),
            new Vector2(0.78f, 0.20f),
            new Vector2(75f, 75f)
        );

        Transform panel = canvas.transform.Find("PanelPrincipal");

        if (panel != null)
        {
            Image fondoPanel = panel.GetComponent<Image>();

            if (fondoPanel != null)
                fondoPanel.color = new Color(0.035f, 0.045f, 0.10f, 0.96f);

            MejorarTitulo(panel);
            MejorarBoton(panel, "BotonNivel1", new Color(0.82f, 0.12f, 0.08f));
            MejorarBoton(panel, "BotonNivel2", new Color(0.07f, 0.52f, 0.18f));
            MejorarBoton(panel, "BotonSalir", new Color(0.20f, 0.22f, 0.28f));
        }

        EditorSceneManager.MarkSceneDirty(escenaMenu);
        EditorSceneManager.SaveScene(escenaMenu);

        Debug.Log("Menu visual mejorado con sprites de Mario y sigue siendo editable.");

        CerrarYVolver(escenaMenu, escenaOriginal, yaEstabaAbierta);
    }

    private static void MejorarTitulo(Transform panel)
    {
        Transform titulo = panel.Find("Titulo");

        if (titulo == null)
            return;

        Text texto = titulo.GetComponent<Text>();

        if (texto != null)
        {
            texto.fontSize = 64;
            texto.color = new Color(1f, 0.82f, 0.08f);
        }

        if (titulo.GetComponent<Shadow>() == null)
        {
            Shadow sombra = titulo.gameObject.AddComponent<Shadow>();
            sombra.effectColor = new Color(0.42f, 0.04f, 0.03f, 1f);
            sombra.effectDistance = new Vector2(4f, -4f);
        }
    }

    private static void MejorarBoton(Transform panel, string nombre, Color color)
    {
        Transform t = panel.Find(nombre);

        if (t == null)
            return;

        Image imagen = t.GetComponent<Image>();

        if (imagen != null)
            imagen.color = color;

        Button boton = t.GetComponent<Button>();

        if (boton != null)
        {
            ColorBlock colores = boton.colors;
            colores.normalColor = color;
            colores.highlightedColor = Color.Lerp(color, Color.white, 0.24f);
            colores.pressedColor = Color.Lerp(color, Color.black, 0.18f);
            colores.selectedColor = colores.highlightedColor;
            colores.fadeDuration = 0.10f;
            boton.colors = colores;
        }

        Transform texto = t.Find("Texto");

        if (texto != null && texto.GetComponent<Shadow>() == null)
        {
            Shadow sombra = texto.gameObject.AddComponent<Shadow>();
            sombra.effectColor = new Color(0f, 0f, 0f, 0.55f);
            sombra.effectDistance = new Vector2(2f, -2f);
        }
    }

    private static void CrearOActualizarSprite(
        Transform padre,
        string nombre,
        Sprite sprite,
        Vector2 anchor,
        Vector2 tamano)
    {
        if (sprite == null)
            return;

        Transform existente = padre.Find(nombre);
        GameObject go;

        if (existente == null)
        {
            go = new GameObject(nombre, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(padre, false);
        }
        else
        {
            go = existente.gameObject;
        }

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = tamano;

        Image imagen = go.GetComponent<Image>();
        imagen.sprite = sprite;
        imagen.preserveAspect = true;
        imagen.raycastTarget = false;
        imagen.color = Color.white;
    }

    private static Sprite CargarSprite(string ruta)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(ruta);

        if (sprite != null)
            return sprite;

        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(ruta);

        foreach (Object asset in assets)
        {
            if (asset is Sprite encontrado)
                return encontrado;
        }

        return null;
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

    private static void CerrarYVolver(Scene menu, Scene original, bool yaEstabaAbierta)
    {
        if (!yaEstabaAbierta)
            EditorSceneManager.CloseScene(menu, true);

        if (original.IsValid() && original.isLoaded)
            SceneManager.SetActiveScene(original);
    }
}
#endif
