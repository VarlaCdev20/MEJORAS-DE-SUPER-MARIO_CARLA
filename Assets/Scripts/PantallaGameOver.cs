using UnityEngine;
using UnityEngine.SceneManagement;
public class PantallaGameOver : MonoBehaviour
{
    private void Awake() { Time.timeScale = 1f; }
    public void Reintentar()
    {
        if (GameManager.Instance != null) GameManager.Instance.ReintentarDespuesDeGameOver();
        else SceneManager.LoadScene("1-1");
    }
    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.VolverAlMenu();
        else SceneManager.LoadScene("Menu");
    }
    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Reintentar();
        if (Input.GetKeyDown(KeyCode.Escape)) VolverAlMenu();
    }
    private void OnGUI()
    {
        if (GameObject.Find("GameOverEditableCanvas") != null) return;
        float w = Mathf.Min(450f, Screen.width * 0.8f);
        float x = (Screen.width - w) / 2f, y = Screen.height * 0.22f;
        GUI.Box(new Rect(x-25f,y-20f,w+50f,350f),"");
        GUIStyle title = new GUIStyle(GUI.skin.label) { fontSize = 46, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        title.normal.textColor = new Color(1f,.2f,.2f);
        GUI.Label(new Rect(x,y,w,80f),"GAME OVER",title);
        if (GUI.Button(new Rect(x,y+110f,w,55f),"REINTENTAR")) Reintentar();
        if (GUI.Button(new Rect(x,y+180f,w,55f),"VOLVER AL MENU")) VolverAlMenu();
        if (GUI.Button(new Rect(x,y+250f,w,45f),"SALIR")) Salir();
    }
}