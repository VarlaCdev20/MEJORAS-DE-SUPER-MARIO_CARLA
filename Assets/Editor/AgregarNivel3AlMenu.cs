#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[InitializeOnLoad]
public static class AgregarNivel3AlMenu
{
    private const string RutaMenu = "Assets/Scenes/Menu.unity";

    static AgregarNivel3AlMenu()
    {
        EditorApplication.delayCall += Aplicar;
    }

    [MenuItem("Mario Carla/Agregar nivel 3 al menu")]
    public static void Aplicar()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        Scene original = SceneManager.GetActiveScene();
        Scene menuScene = default;
        bool abierta = false;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (s.path == RutaMenu)
            {
                menuScene = s;
                abierta = true;
                break;
            }
        }

        if (!abierta)
            menuScene = EditorSceneManager.OpenScene(RutaMenu, OpenSceneMode.Additive);

        SceneManager.SetActiveScene(menuScene);

        ConstruirMenuEditable.CrearSiHaceFalta();

        MenuInicio controlador = Object.FindFirstObjectByType<MenuInicio>();
        GameObject canvas = GameObject.Find("MenuEditableCanvas");

        if (controlador == null || canvas == null)
        {
            Debug.LogWarning("No se pudo actualizar el menu para el nivel 3.");
            Cerrar(menuScene, original, abierta);
            return;
        }

        Transform panel = canvas.transform.Find("PanelPrincipal");
        if (panel == null)
        {
            Cerrar(menuScene, original, abierta);
            return;
        }

        RectTransform panelRT = panel.GetComponent<RectTransform>();
        panelRT.sizeDelta = new Vector2(650f, 650f);

        Mover(panel, "BotonNivel1", new Vector2(0f, 55f));
        Mover(panel, "BotonNivel2", new Vector2(0f, -30f));
        Mover(panel, "BotonSalir", new Vector2(0f, -235f));
        Mover(panel, "Ayuda", new Vector2(0f, -295f));

        Transform existente = panel.Find("BotonNivel3");

        if (existente == null)
        {
            Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject botonGO = new GameObject(
                "BotonNivel3",
                typeof(RectTransform),
                typeof(Image),
                typeof(Button)
            );

            botonGO.transform.SetParent(panel, false);

            RectTransform rt = botonGO.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(0f, -115f);
            rt.sizeDelta = new Vector2(500f, 68f);

            Image image = botonGO.GetComponent<Image>();
            Color azul = new Color(0.04f, 0.38f, 0.82f);
            image.color = azul;

            Button boton = botonGO.GetComponent<Button>();
            ColorBlock colores = boton.colors;
            colores.normalColor = azul;
            colores.highlightedColor = new Color(0.10f, 0.58f, 1f);
            colores.pressedColor = new Color(0.02f, 0.25f, 0.60f);
            colores.selectedColor = colores.highlightedColor;
            boton.colors = colores;

            GameObject textoGO = new GameObject("Texto", typeof(RectTransform), typeof(Text));
            textoGO.transform.SetParent(botonGO.transform, false);

            RectTransform textoRT = textoGO.GetComponent<RectTransform>();
            textoRT.anchorMin = Vector2.zero;
            textoRT.anchorMax = Vector2.one;
            textoRT.offsetMin = Vector2.zero;
            textoRT.offsetMax = Vector2.zero;

            Text texto = textoGO.GetComponent<Text>();
            texto.text = "NIVEL 3 - ACUATICO";
            texto.font = fuente;
            texto.fontSize = 23;
            texto.fontStyle = FontStyle.Bold;
            texto.alignment = TextAnchor.MiddleCenter;
            texto.color = Color.white;

            Shadow sombra = textoGO.AddComponent<Shadow>();
            sombra.effectColor = new Color(0f, 0f, 0f, 0.55f);
            sombra.effectDistance = new Vector2(2f, -2f);

            UnityEventTools.AddPersistentListener(boton.onClick, controlador.IrNivel3);
        }
        else
        {
            RectTransform rt = existente.GetComponent<RectTransform>();
            if (rt != null)
                rt.anchoredPosition = new Vector2(0f, -115f);
        }

        EditorSceneManager.MarkSceneDirty(menuScene);
        EditorSceneManager.SaveScene(menuScene);

        Debug.Log("Nivel 3 acuatico agregado al menu.");

        Cerrar(menuScene, original, abierta);
    }

    private static void Mover(Transform panel, string nombre, Vector2 posicion)
    {
        Transform t = panel.Find(nombre);
        if (t == null)
            return;

        RectTransform rt = t.GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = posicion;
    }

    private static void Cerrar(Scene menu, Scene original, bool yaEstabaAbierta)
    {
        if (!yaEstabaAbierta)
            EditorSceneManager.CloseScene(menu, true);

        if (original.IsValid() && original.isLoaded)
            SceneManager.SetActiveScene(original);
    }
}
#endif
