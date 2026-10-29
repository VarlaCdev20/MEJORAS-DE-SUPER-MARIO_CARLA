using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public void IrNivel1()
    {
        CargarNivel("1-1");
    }

    public void IrNivel2()
    {
        CargarNivel("1-2");
    }

    public void IrNivel3()
    {
        CargarNivel("1-3");
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void CargarNivel(string escena)
    {
        Time.timeScale = 1f;

        if (Application.CanStreamedLevelBeLoaded(escena))
        {
        {
            string[] partes = escena.Split('-');
            if (GameManager.Instance != null &&
                partes.Length == 2 &&
                int.TryParse(partes[0], out int mundo) &&
                int.TryParse(partes[1], out int nivel))
            {
                GameManager.Instance.IniciarPartida(mundo, nivel);
            }
            else
            {
                SceneManager.LoadScene(escena, LoadSceneMode.Single);
            }
        }
        }
        else
        {
            Debug.LogError("No se encontro la escena " + escena + " en el Build.");
        }
    }
}
