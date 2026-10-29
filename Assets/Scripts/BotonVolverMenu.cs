using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonVolverMenu : MonoBehaviour
{
    private GUIStyle boton;
    private Texture2D fondo;
    private Texture2D fondoHover;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CrearSiHaceFalta()
    {
        string escena = SceneManager.GetActiveScene().name;

        if (escena != "1-1" && escena != "1-2")
            return;

        if (Object.FindFirstObjectByType<BotonVolverMenu>() != null)
            return;

        GameObject go = new GameObject("BotonVolverMenu");
        Object.DontDestroyOnLoad(go);
        go.AddComponent<BotonVolverMenu>();
    }

    private void Awake()
    {
        fondo = CrearColor(new Color(0.12f, 0.12f, 0.18f, 0.92f));
        fondoHover = CrearColor(new Color(0.28f, 0.28f, 0.38f, 0.96f));
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
        string escena = SceneManager.GetActiveScene().name;

        if (escena != "1-1" && escena != "1-2")
        {
            Destroy(gameObject);
            return;
        }

        if (boton == null)
        {
            boton = new GUIStyle(GUI.skin.button);
            boton.fontStyle = FontStyle.Bold;
            boton.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.026f, 16f, 23f));
            boton.normal.textColor = Color.white;
            boton.hover.textColor = Color.white;
            boton.active.textColor = Color.white;
            boton.normal.background = fondo;
            boton.hover.background = fondoHover;
            boton.active.background = fondoHover;
        }

        float ancho = Mathf.Min(150f, Screen.width * 0.18f);

        if (GUI.Button(
            new Rect(Screen.width - ancho - 18f, 18f, ancho, 46f),
            "MENU",
            boton))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
