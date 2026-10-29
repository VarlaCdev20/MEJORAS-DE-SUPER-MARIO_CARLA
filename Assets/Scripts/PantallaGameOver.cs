using UnityEngine;
using UnityEngine.SceneManagement;
public class PantallaGameOver : MonoBehaviour
{
    private RectTransform titulo;
    private float inicio;
    private void Awake() { Time.timeScale = 1f; inicio = Time.unscaledTime; }
    private void Start()
    {
        GameObject canvas = GameObject.Find("GameOverEditableCanvas");
        if (canvas != null)
        {
            Transform t = canvas.transform.Find("PanelGameOver/TituloGameOver");
            if (t != null) titulo = t.GetComponent<RectTransform>();
        }
        AudioSource fuente = GetComponent<AudioSource>();
        if (fuente == null) fuente = gameObject.AddComponent<AudioSource>();
        fuente.playOnAwake = false;
        const int frecuencia = 22050;
        int muestras = frecuencia;
        float[] datos = new float[muestras];
        float[] notas = { 392f, 329.63f, 261.63f };
        for (int i=0; i<muestras; i++)
        {
            float t = (float)i/frecuencia;
            int paso = Mathf.Min(2,(int)(t*3f));
            float envolvente = Mathf.Clamp01((1f/3f - (t-paso/3f))*7f);
            datos[i] = Mathf.Sin(2f*Mathf.PI*notas[paso]*t)*envolvente*0.12f;
        }
        AudioClip clip = AudioClip.Create("DerrotaOriginal",muestras,1,frecuencia,false);
        clip.SetData(datos,0);
        fuente.PlayOneShot(clip);
    }
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
        if (titulo != null)
        {
            float escala = 1f + Mathf.Sin((Time.unscaledTime-inicio)*2.8f)*0.03f;
            titulo.localScale = new Vector3(escala,escala,1f);
        }
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