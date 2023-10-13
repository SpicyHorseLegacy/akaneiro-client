using UnityEngine;
using System.Collections;

public class SurveillanceCamera : MonoBehaviour
{

    public static SurveillanceCamera Instance = null;

    public LayerMask layers;
    public Vector2 pos = Vector2.zero;

    // switch to turn on camera for test in editor
    public Vector2 testCameraViewSize = new Vector2(120,300f);
    public bool testCameraOn = true;
	public bool testInEditor = false;

    // default config to control the camera
    protected bool isCameraOn = false;
    protected float cameraSize = 99f;
    protected float cameraAspect = 2.5f;

    // component members
    protected Camera cam = null;
    protected Light _light = null;
	
	public bool isPlayerUse = true;

    // Use this for initialization
    void Awake()
    {
		if(isPlayerUse){
        	Instance = this;
		}

        cam = this.GetComponent<Camera>();
        _light = this.GetComponent<Light>();

        // Init & validate cameraSize & culling mask layers
        SetSize(cameraSize);
        SetPosition(pos);
        setVisibleLayers(layers);

        ShutDown();
    }

    // Update is called once per frame
    void Update()
    {

        // Instant update codes for editor( position & size locating)
        if (Application.isEditor && testInEditor)
        {
            SetCameraOn(testCameraOn);

            //SetSize(cameraSize);
            SetViewSize(testCameraViewSize.x, testCameraViewSize.y);
            SetPosition(pos);
            setVisibleLayers(layers);
        }
    }

    protected void SetCameraOn(bool isOn)
    {
        isCameraOn = isOn;

        cam.enabled = isCameraOn;
        if (_light != null)
            _light.enabled = isCameraOn;
    }

    // Set the normalized from 0 to 100 size for the output
    protected void SetSize(float newSize)
    {
        cameraSize = Mathf.Clamp(newSize, 0f, 99f);

        Rect r = cam.rect;
        r.height = cameraSize / 100f;
        r.width = r.height / cameraAspect;

        cam.rect = r;

        SetPosition(pos);
    }

    // Set the culling mask for camera
    public void setVisibleLayers(int layer)
    {
        cam.cullingMask = layer;

        int l = LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("Default");
    }

    // Set the camera view size on screen
    public void SetViewSize(float width, float height)
    {
        height = Mathf.Clamp(height, 0f, Screen.height);
        width = Mathf.Clamp(width, 0f, Screen.width);

        // Reset camera aspect
        cameraAspect = height / width;
        SetSize(100 * height / Screen.height);
    }

    // Set center pos for this camera output, in screen cordinate
    public void SetPosition(Vector2 newPos)
    {
        Rect r = cam.rect;
        pos = newPos;

        float frameWidthHalf = Screen.width * r.width / 2f;
        float frameHeightHalf = Screen.height * r.height / 2f;
        float xMax = Screen.width * (1f - r.width) + frameWidthHalf;
        float yMax = Screen.height * (1f - r.height) + frameHeightHalf;

        pos.x = Mathf.Clamp(pos.x, frameWidthHalf, xMax);
        pos.y = Mathf.Clamp(pos.y, frameHeightHalf, yMax);

        r.x = (pos.x - frameWidthHalf) / Screen.width;
        r.y = (Screen.height * (1f - r.height) - pos.y + frameHeightHalf) / Screen.height;

        cam.rect = r;
        cam.ResetAspect();
    }

    public void Show()
    {
        SetCameraOn(true);
    }

    public void ShowAt(Vector2 screenPosCenter, Vector2 viewSize)
    {
        SetPosition(screenPosCenter);
        SetViewSize(viewSize.x, viewSize.y);
        SetCameraOn(true);
    }

    public void ShutDown()
    {
        SetCameraOn(false);
    }

}
