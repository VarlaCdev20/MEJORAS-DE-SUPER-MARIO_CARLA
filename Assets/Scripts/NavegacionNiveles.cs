using UnityEngine;
using UnityEngine.SceneManagement;

public class NavegacionNiveles : MonoBehaviour
{
    private GUIStyle botonMenu;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void PrepararNavegacion()
    {
        string escena = SceneManager.GetActiveScene().name;

        if (escena != "1-1" && escena != "1-2" && escena != "1-3")
            return;

        if (FindFirstObjectByType<NavegacionNiveles>() != null)
            return;

        GameObject control = new GameObject("NavegacionNiveles");
        control.AddComponent<NavegacionNiveles>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            VolverAlMenu();
    }

    private void OnGUI()
    {
        if (botonMenu == null)
        {
            botonMenu = new GUIStyle(GUI.skin.button);
            botonMenu.fontStyle = FontStyle.Bold;
            botonMenu.fontSize = Mathf.RoundToInt(Mathf.Clamp(Screen.height * 0.022f, 14f, 20f));
            botonMenu.normal.textColor = Color.white;
        }

        float ancho = Mathf.Clamp(Screen.width * 0.13f, 110f, 165f);

        if (GUI.Button(
            new Rect(Screen.width - ancho - 18f, 18f, ancho, 42f),
            "MENU",
            botonMenu))
        {
            VolverAlMenu();
        }
    }

    private void VolverAlMenu()
    {
        Time.timeScale = 1f;

        if (Application.CanStreamedLevelBeLoaded("Menu"))
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        else
            Debug.LogError("La escena Menu no esta agregada al Build.");
    }
}
