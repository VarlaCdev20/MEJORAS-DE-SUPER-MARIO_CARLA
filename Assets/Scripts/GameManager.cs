using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }

    private bool procesandoMuerte;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        // Permite iniciar una escena directamente en Unity para probar.
        if (lives <= 0)
        {
            lives = 3;
            coins = 0;
        }

        DetectarNivelActual();
    }

    private void DetectarNivelActual()
    {
        string nombre = SceneManager.GetActiveScene().name;
        string[] partes = nombre.Split('-');

        if (partes.Length == 2 &&
            int.TryParse(partes[0], out int mundoActual) &&
            int.TryParse(partes[1], out int nivelActual))
        {
            world = mundoActual;
            stage = nivelActual;
        }
    }

    public void NewGame()
    {
        IniciarPartida(1, 1);
    }

    public void IniciarPartida(int mundo, int nivel)
    {
        CancelInvoke(nameof(ResetLevel));
        procesandoMuerte = false;
        lives = 3;
        coins = 0;
        LoadLevel(mundo, nivel);
    }

    public void GameOver()
    {
        CancelInvoke(nameof(ResetLevel));
        procesandoMuerte = false;
        lives = 0;
        Time.timeScale = 1f;

        if (Application.CanStreamedLevelBeLoaded("GameOver"))
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        else
            Debug.LogError("Falta agregar GameOver.unity en Build Profiles.");
    }

    public void ReintentarDespuesDeGameOver()
    {
        int mundo = world > 0 ? world : 1;
        int nivel = stage > 0 ? stage : 1;
        IniciarPartida(mundo, nivel);
    }

    public void VolverAlMenu()
    {
        CancelInvoke(nameof(ResetLevel));
        procesandoMuerte = false;
        Time.timeScale = 1f;

        if (Application.CanStreamedLevelBeLoaded("Menu"))
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        else
            Debug.LogError("Falta agregar Menu.unity en Build Profiles.");
    }

    public void LoadLevel(int mundo, int nivel)
    {
        CancelInvoke(nameof(ResetLevel));
        procesandoMuerte = false;
        world = mundo;
        stage = nivel;
        Time.timeScale = 1f;

        string escena = $"{mundo}-{nivel}";
        if (Application.CanStreamedLevelBeLoaded(escena))
            SceneManager.LoadScene(escena, LoadSceneMode.Single);
        else
            Debug.LogError("Falta agregar " + escena + " en Build Profiles.");
    }

    public void NextLevel()
    {
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        // Evita descontar varias vidas por una misma muerte.
        if (procesandoMuerte)
            return;

        procesandoMuerte = true;
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), Mathf.Max(0f, delay));
    }

    public void ResetLevel()
    {
        CancelInvoke(nameof(ResetLevel));
        lives = Mathf.Max(0, lives - 1);

        if (lives > 0)
            LoadLevel(world, stage);
        else
            GameOver();
    }

    public void AddCoin()
    {
        coins++;

        if (coins >= 100)
        {
            coins -= 100;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
    }
}
