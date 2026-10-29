#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class ConstruirNivelAcuatico
{
    private const string Ruta = "Assets/Scenes/1-3.unity";
    private const string Marcador = "NIVEL_ACUATICO_GENERADO";

    static ConstruirNivelAcuatico()
    {
        EditorApplication.delayCall += ConstruirSiHaceFalta;
    }

    [MenuItem("Mario Carla/Construir nivel 1-3 acuatico")]
    public static void ConstruirSiHaceFalta()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        Scene original = SceneManager.GetActiveScene();
        Scene escena = default;
        bool abierta = false;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (s.path == Ruta)
            {
                escena = s;
                abierta = true;
                break;
            }
        }

        if (!abierta)
            escena = EditorSceneManager.OpenScene(Ruta, OpenSceneMode.Additive);

        foreach (GameObject raiz in escena.GetRootGameObjects())
        {
            if (raiz.name == Marcador)
            {
                CerrarYVolver(escena, original, abierta);
                return;
            }
        }

        SceneManager.SetActiveScene(escena);

        foreach (GameObject raiz in escena.GetRootGameObjects())
            Object.DestroyImmediate(raiz);

        GameObject marcador = new GameObject(Marcador);

        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<GameManager>();

        GameObject marioPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mario.prefab");
        GameObject bloquePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UndergroundBlock.prefab");
        GameObject monedaPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Coin.prefab");

        if (marioPrefab == null || bloquePrefab == null || monedaPrefab == null)
        {
            Debug.LogError("No se pudieron encontrar los prefabs necesarios para 1-3.");
            return;
        }

        GameObject mario = Instanciar(marioPrefab, new Vector3(2.5f, -1.5f, 0f), null);
        mario.name = "Mario";

        PlayerMovement movimientoNormal = mario.GetComponent<PlayerMovement>();
        if (movimientoNormal != null)
            movimientoNormal.enabled = false;

        if (mario.GetComponent<MovimientoAcuatico>() == null)
            mario.AddComponent<MovimientoAcuatico>();

        GameObject camaraGO = new GameObject("Main Camera");
        camaraGO.tag = "MainCamera";
        camaraGO.transform.position = new Vector3(6f, 0f, -10f);

        Camera camara = camaraGO.AddComponent<Camera>();
        camara.orthographic = true;
        camara.orthographicSize = 6.5f;
        camara.backgroundColor = new Color(0.02f, 0.16f, 0.48f, 1f);

        camaraGO.AddComponent<AudioListener>();

        CamaraAcuatica seguimiento = camaraGO.AddComponent<CamaraAcuatica>();
        seguimiento.jugador = mario.transform;
        seguimiento.minimoX = 6f;
        seguimiento.maximoX = 94f;
        seguimiento.minimoY = -0.5f;
        seguimiento.maximoY = 1.5f;

        GameObject escenario = new GameObject("EscenarioAcuatico");
        escenario.transform.SetParent(marcador.transform);

        GameObject monedas = new GameObject("Monedas");
        monedas.transform.SetParent(marcador.transform);

        GameObject peces = new GameObject("Peces");
        peces.transform.SetParent(marcador.transform);

        GameObject corrientes = new GameObject("Corrientes");
        corrientes.transform.SetParent(marcador.transform);

        Color piedra = new Color(0.18f, 0.72f, 0.46f, 1f);
        Color piedraOscura = new Color(0.10f, 0.48f, 0.35f, 1f);
        Color coral = new Color(0.95f, 0.25f, 0.42f, 1f);

        CrearRectangulo(bloquePrefab, escenario.transform, "Suelo A", new Vector2(8f, -5.3f), new Vector2(18f, 2f), piedra);
        CrearRectangulo(bloquePrefab, escenario.transform, "Suelo B", new Vector2(32f, -5.3f), new Vector2(20f, 2f), piedra);
        CrearRectangulo(bloquePrefab, escenario.transform, "Suelo C", new Vector2(59f, -5.3f), new Vector2(24f, 2f), piedra);
        CrearRectangulo(bloquePrefab, escenario.transform, "Suelo D", new Vector2(87f, -5.3f), new Vector2(22f, 2f), piedra);

        CrearRectangulo(bloquePrefab, escenario.transform, "Techo A", new Vector2(14f, 6.3f), new Vector2(28f, 1.5f), piedraOscura);
        CrearRectangulo(bloquePrefab, escenario.transform, "Techo B", new Vector2(48f, 6.3f), new Vector2(24f, 1.5f), piedraOscura);
        CrearRectangulo(bloquePrefab, escenario.transform, "Techo C", new Vector2(80f, 6.3f), new Vector2(32f, 1.5f), piedraOscura);

        CrearRectangulo(bloquePrefab, escenario.transform, "Columna 1", new Vector2(15f, -2.2f), new Vector2(3f, 5f), piedra);
        CrearRectangulo(bloquePrefab, escenario.transform, "Columna 2", new Vector2(28f, 2.6f), new Vector2(3f, 5.5f), piedraOscura);
        CrearRectangulo(bloquePrefab, escenario.transform, "Columna 3", new Vector2(42f, -1.6f), new Vector2(4f, 6f), piedra);
        CrearRectangulo(bloquePrefab, escenario.transform, "Columna 4", new Vector2(57f, 2.8f), new Vector2(3f, 5f), piedraOscura);
        CrearRectangulo(bloquePrefab, escenario.transform, "Columna 5", new Vector2(73f, -1.7f), new Vector2(4f, 6f), piedra);
        CrearRectangulo(bloquePrefab, escenario.transform, "Columna 6", new Vector2(88f, 2.4f), new Vector2(3f, 5.5f), piedraOscura);

        CrearRectangulo(bloquePrefab, escenario.transform, "Coral 1", new Vector2(22f, -3.4f), new Vector2(1.3f, 3.4f), coral);
        CrearRectangulo(bloquePrefab, escenario.transform, "Coral 2", new Vector2(50f, -3.3f), new Vector2(1.3f, 3.6f), coral);
        CrearRectangulo(bloquePrefab, escenario.transform, "Coral 3", new Vector2(79f, -3.1f), new Vector2(1.3f, 4f), coral);

        Vector2[] posicionesMonedas = {
            new Vector2(7f,-1.5f), new Vector2(9f,0f), new Vector2(11f,1.5f),
            new Vector2(19f,3.2f), new Vector2(21f,3.8f), new Vector2(23f,3.2f),
            new Vector2(34f,-1f), new Vector2(36f,0f), new Vector2(38f,1f),
            new Vector2(46f,3.6f), new Vector2(48f,3.6f), new Vector2(50f,3.6f),
            new Vector2(61f,-1.2f), new Vector2(63f,0f), new Vector2(65f,1.2f),
            new Vector2(77f,3.2f), new Vector2(79f,3.8f), new Vector2(81f,3.2f),
            new Vector2(90f,0f), new Vector2(92f,1f), new Vector2(94f,2f)
        };

        foreach (Vector2 p in posicionesMonedas)
            Instanciar(monedaPrefab, new Vector3(p.x, p.y, 0f), monedas.transform);

        CrearPez(peces.transform, new Vector2(12f, 2.5f), 5f, 2.0f);
        CrearPez(peces.transform, new Vector2(25f, 0.2f), 7f, 2.4f);
        CrearPez(peces.transform, new Vector2(37f, 3.2f), 5f, 2.1f);
        CrearPez(peces.transform, new Vector2(53f, -0.4f), 7f, 2.5f);
        CrearPez(peces.transform, new Vector2(68f, 2.8f), 6f, 2.2f);
        CrearPez(peces.transform, new Vector2(84f, 0.5f), 7f, 2.7f);
        CrearPez(peces.transform, new Vector2(93f, 3.0f), 4f, 2.5f);

        CrearCorriente(corrientes.transform, new Vector2(20f, -2f), new Vector2(3.5f, 6f), 8f);
        CrearCorriente(corrientes.transform, new Vector2(46f, -2f), new Vector2(3.5f, 6f), 9f);
        CrearCorriente(corrientes.transform, new Vector2(72f, -2f), new Vector2(3.5f, 6f), 8.5f);

        GameObject salidaVisual = CrearRectangulo(
            bloquePrefab,
            escenario.transform,
            "Tuberia de salida",
            new Vector2(98f, -2.8f),
            new Vector2(3f, 5f),
            new Color(0.08f, 0.82f, 0.20f, 1f)
        );

        GameObject salida = new GameObject("SALIDA NIVEL 1-3");
        salida.transform.SetParent(marcador.transform);
        salida.transform.position = new Vector3(98f, 0f, 0f);
        BoxCollider2D salidaCollider = salida.AddComponent<BoxCollider2D>();
        salidaCollider.isTrigger = true;
        salidaCollider.size = new Vector2(3.5f, 9f);
        salida.AddComponent<SalidaNivelAcuatico>();

        GameObject muerte = new GameObject("Death Barrier");
        muerte.transform.SetParent(marcador.transform);
        muerte.transform.position = new Vector3(50f, -8.2f, 0f);
        BoxCollider2D muerteCollider = muerte.AddComponent<BoxCollider2D>();
        muerteCollider.isTrigger = true;
        muerteCollider.size = new Vector2(120f, 2f);
        muerte.AddComponent<DeathBarrier>();

        GameObject presentacion = new GameObject("Presentacion Nivel 1-3");
        presentacion.transform.SetParent(marcador.transform);
        presentacion.AddComponent<PresentacionNivel3>();

        EditorSceneManager.MarkSceneDirty(escena);
        EditorSceneManager.SaveScene(escena);

        Debug.Log("Nivel 1-3 acuatico creado y guardado. Ya puedes editarlo desde la Hierarchy.");

        CerrarYVolver(escena, original, abierta);
    }

    private static GameObject Instanciar(GameObject prefab, Vector3 posicion, Transform padre)
    {
        GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        go.transform.position = posicion;

        if (padre != null)
            go.transform.SetParent(padre, true);

        return go;
    }

    private static GameObject CrearRectangulo(
        GameObject prefab,
        Transform padre,
        string nombre,
        Vector2 posicion,
        Vector2 tamano,
        Color color)
    {
        GameObject go = Instanciar(prefab, new Vector3(posicion.x, posicion.y, 0f), padre);
        go.name = nombre;

        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.size = tamano;
            sr.color = color;
        }

        BoxCollider2D col = go.GetComponent<BoxCollider2D>();
        if (col != null)
            col.size = tamano;

        return go;
    }

    private static void CrearPez(Transform padre, Vector2 posicion, float distancia, float velocidad)
    {
        GameObject pez = new GameObject("Pez Acuatico");
        pez.transform.SetParent(padre);
        pez.transform.position = posicion;
        pez.transform.localScale = new Vector3(1.5f, 1.5f, 1f);

        SpriteRenderer sr = pez.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 6;

        CircleCollider2D col = pez.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.55f;

        int capaEnemigo = LayerMask.NameToLayer("Enemy");
        if (capaEnemigo >= 0)
            pez.layer = capaEnemigo;

        PezAcuatico movimiento = pez.AddComponent<PezAcuatico>();
        movimiento.distancia = distancia;
        movimiento.velocidad = velocidad;
    }

    private static void CrearCorriente(Transform padre, Vector2 posicion, Vector2 tamano, float fuerza)
    {
        GameObject corriente = new GameObject("Corriente hacia abajo");
        corriente.transform.SetParent(padre);
        corriente.transform.position = posicion;

        BoxCollider2D col = corriente.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = tamano;

        CorrienteAcuatica efecto = corriente.AddComponent<CorrienteAcuatica>();
        efecto.fuerza = fuerza;
    }

    private static void CerrarYVolver(Scene escena, Scene original, bool yaEstabaAbierta)
    {
        if (!yaEstabaAbierta)
            EditorSceneManager.CloseScene(escena, true);

        if (original.IsValid() && original.isLoaded)
            SceneManager.SetActiveScene(original);
    }
}
#endif
