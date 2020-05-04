using Immersal.Samples.Mapping;
using Immersal.Samples.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameObject = UnityEngine.GameObject;

public class PointcloudController : MonoBehaviour {
    public static PointcloudController Instance {
        get { return instance; }
    }
    private static PointcloudController instance;

    private Vector3[] points;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Double singleton error");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        CoroutineJobLoadMap.OnMapRecieved += OnMapRecieved;
    }

    public void OnMapRecieved(Vector3[] points) {
        this.points = points;
    }

    public void OpenViewer() {
        if(points.Length <= 0) {
            Debug.LogError("Missing point data");
        } else {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("PointcloudViewer", LoadSceneMode.Single);
            PointCloudRenderer rnd = gameObject.AddComponent<PointCloudRenderer>();
            rnd.CreateCloud(points, points.Length);
        }
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode mode) {
        if(s.name != "PointcloudViewer") return;

        CameraController cc = Camera.main.GetComponent<CameraController>();
        cc.SetPivot(transform);
        Debug.Log("Pivot set");
    }

    public void ExitPreview() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        instance = null;
        Destroy(gameObject);
    }
}
