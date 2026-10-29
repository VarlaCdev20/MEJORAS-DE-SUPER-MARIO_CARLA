#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[InitializeOnLoad]
public static class ConstruirGameOver
{
    private const string Ruta = "Assets/Scenes/GameOver.unity";
    static ConstruirGameOver() { EditorApplication.delayCall += Crear; }

    [MenuItem("Mario Carla/Crear o reparar Game Over")]
    public static void Crear()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling) return;
        Scene previa = SceneManager.GetActiveScene();
        Scene escena = SceneManager.GetSceneByPath(Ruta);
        bool abierta = escena.IsValid() && escena.isLoaded;
        if (!abierta) escena = EditorSceneManager.OpenScene(Ruta, OpenSceneMode.Additive);
        SceneManager.SetActiveScene(escena);
        if (BuscarRaiz(escena,"GameOverEditableCanvas") == null)
        {
            GameObject controlador = BuscarRaiz(escena,"PantallaGameOver");
            if (controlador == null) controlador = new GameObject("PantallaGameOver");
            PantallaGameOver gameOver = controlador.GetComponent<PantallaGameOver>();
            if (gameOver == null) gameOver = controlador.AddComponent<PantallaGameOver>();

            if (BuscarRaiz(escena,"Main Camera") == null)
            {
                GameObject cam = new GameObject("Main Camera",typeof(Camera),typeof(AudioListener));
                cam.tag = "MainCamera";
                cam.transform.position = new Vector3(0,0,-10);
                cam.GetComponent<Camera>().orthographic = true;
                cam.GetComponent<Camera>().backgroundColor = new Color(.025f,.025f,.08f);
            }

            GameObject events = new GameObject("EventSystem",typeof(EventSystem),typeof(StandaloneInputModule));
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasGO = new GameObject("GameOverEditableCanvas",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
            Canvas canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280,720);

            Image fondo = Imagen(canvasGO.transform,"FondoOscuro",new Color(.035f,.035f,.10f),Vector2.zero,Vector2.one,Vector2.zero,Vector2.zero);
            Image panel = Imagen(canvasGO.transform,"PanelGameOver",new Color(.11f,.09f,.16f),new Vector2(.5f,.5f),new Vector2(.5f,.5f),Vector2.zero,new Vector2(620,565));
            Texto(panel.transform,"TituloGameOver","GAME OVER",font,66,FontStyle.Bold,new Color(1f,.25f,.2f),new Vector2(0,178),new Vector2(570,100));
            Texto(panel.transform,"Subtitulo","Mario se quedo sin vidas",font,26,FontStyle.Bold,Color.white,new Vector2(0,100),new Vector2(570,60));
            Texto(panel.transform,"TextoAyuda","Presiona R para volver a intentar",font,19,FontStyle.Normal,new Color(1f,.82f,.35f),new Vector2(0,45),new Vector2(570,45));
            Boton(panel.transform,"BotonReintentar","REINTENTAR",font,new Vector2(0,-45),new Color(.14f,.58f,.23f),gameOver.Reintentar);
            Boton(panel.transform,"BotonMenu","MENU PRINCIPAL",font,new Vector2(0,-135),new Color(.73f,.16f,.11f),gameOver.VolverAlMenu);
            Boton(panel.transform,"BotonSalir","SALIR",font,new Vector2(0,-220),new Color(.25f,.27f,.34f),gameOver.Salir);
            EditorSceneManager.MarkSceneDirty(escena);
            EditorSceneManager.SaveScene(escena);
        }
        if (!abierta) EditorSceneManager.CloseScene(escena,true);
        if (previa.IsValid() && previa.isLoaded) SceneManager.SetActiveScene(previa);
    }
    private static GameObject BuscarRaiz(Scene s,string n) { foreach(GameObject g in s.GetRootGameObjects()) if(g.name==n)return g; return null; }
    private static Image Imagen(Transform p,string n,Color c,Vector2 min,Vector2 max,Vector2 pos,Vector2 size)
    {
        GameObject g=new GameObject(n,typeof(RectTransform),typeof(Image));g.transform.SetParent(p,false);
        RectTransform r=g.GetComponent<RectTransform>();r.anchorMin=min;r.anchorMax=max;r.anchoredPosition=pos;r.sizeDelta=size;
        Image i=g.GetComponent<Image>();i.color=c;return i;
    }
    private static Text Texto(Transform p,string n,string str,Font font,int size,FontStyle style,Color col,Vector2 pos,Vector2 dim)
    {
        GameObject g=new GameObject(n,typeof(RectTransform),typeof(Text));g.transform.SetParent(p,false);
        RectTransform r=g.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=new Vector2(.5f,.5f);r.anchoredPosition=pos;r.sizeDelta=dim;
        Text t=g.GetComponent<Text>();t.font=font;t.text=str;t.fontSize=size;t.fontStyle=style;t.color=col;t.alignment=TextAnchor.MiddleCenter;return t;
    }
    private static void Boton(Transform p,string n,string str,Font font,Vector2 pos,Color color,UnityEngine.Events.UnityAction action)
    {
        Image img=Imagen(p,n,color,new Vector2(.5f,.5f),new Vector2(.5f,.5f),pos,new Vector2(460,66));
        Button b=img.gameObject.AddComponent<Button>();
        ColorBlock colors=b.colors; colors.normalColor=color;colors.highlightedColor=Color.Lerp(color,Color.white,.25f);colors.pressedColor=Color.Lerp(color,Color.black,.2f);b.colors=colors;
        Text t=Texto(img.transform,"Texto",str,font,26,FontStyle.Bold,Color.white,Vector2.zero,new Vector2(460,66));t.raycastTarget=false;
        UnityEventTools.AddPersistentListener(b.onClick,action);
    }
}
#endif