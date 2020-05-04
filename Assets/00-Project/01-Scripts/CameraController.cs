using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    

    [SerializeField] private EventSystem eventsystem;
    [SerializeField] private float speed;
    [SerializeField] private Slider distSlider;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;

    private Vector2 rotation;
    private Vector2 touchStart;
    private Vector2 rotStart;

    private Transform pivot;
    private Transform xJoint;
    private Transform yJoint;

    private bool processRotation;
    [SerializeField] private Transform focus;

    private void Awake() {
        if(focus != null) {
            SetPivot(focus);
        }
    }

    public void SetPivot(Transform focusObject) {
        xJoint = transform.parent;
        yJoint = xJoint.parent;
        pivot = yJoint.parent;

        focus = focusObject;
        focus.parent = pivot;
        focus.transform.localPosition = Vector3.zero;
        focus.transform.localRotation = Quaternion.identity;

        rotation = Vector3.zero;
    }

    private void Update() {
        if(pivot == null) return;

        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
            if(eventsystem.currentSelectedGameObject == null) {
                processRotation = true;
            } else {
                processRotation = false;
            }
        }

        if(Input.touchCount > 0 && processRotation) {
            Touch t = Input.touches[0];

            Vector2 input;
            input = t.deltaPosition;
            
            Debug.Log(input);

            //input -= touchStart;
            Vector2 velocity = new Vector2(input.x * Time.deltaTime * speed, input.y * Time.deltaTime * speed);
            rotation += velocity;

            yJoint.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, rotation.x, 0.0f));
            xJoint.transform.localRotation = Quaternion.Euler(new Vector3(-rotation.y, 0.0f, 0.0f));
        }

        float z = minDist + (maxDist - minDist) * distSlider.value;
        transform.localPosition = new Vector3(0.0f, 0.0f, -z);
    }

    public void ExitPreview()
    {
        if(PointcloudController.Instance != null) {
            PointcloudController.Instance.ExitPreview();
        }

        SceneManager.LoadScene("MappingApp", LoadSceneMode.Single);
    }
}
