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
            SceneManager.LoadScene(escena, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("No se encontro la escena " + escena + " en el Build.");
        }
    }
}
