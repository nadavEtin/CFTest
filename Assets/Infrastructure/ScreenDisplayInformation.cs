using UnityEngine;

public class ScreenDisplayInformation
{
    private static ScreenDisplayInformation _instance;
    public static ScreenDisplayInformation Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScreenDisplayInformation();
                _instance.CalculateScreenBounds();
                _instance._mainUiCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
                _instance.CalculateCanvasBounds();
            }
            return _instance;
        }
    }

    private Canvas _mainUiCanvas;

    //world-space screen borders
    private float _leftBorder;
    private float _rightBorder;
    private float _topBorder;
    private float _bottomBorder;

    //canvas screen borders
    private float _leftCanvasBorder;
    private float _rightCanvasBorder;
    private float _topCanvasBorder;
    private float _bottomCanvasBorder;

    public float LeftBorder => _leftBorder;
    public float RightBorder => _rightBorder; 
    public float TopBorder => _topBorder;
    public float BottomBorder => _bottomBorder;

    public float LeftCanvasBorder => _leftCanvasBorder;
    public float RightCanvasBorder => _rightCanvasBorder;
    public float TopCanvasBorder => _topCanvasBorder;
    public float BottomCanvasBorder => _bottomCanvasBorder;
    

    private void CalculateScreenBounds()
    {
        //get the main camera
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene!");
            return;
        }

        //calculate screen bounds in world coordinates
        Vector2 screenBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector2 screenTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        //set the border values
        _leftBorder = screenBottomLeft.x;
        _rightBorder = screenTopRight.x;
        _bottomBorder = screenBottomLeft.y;
        _topBorder = screenTopRight.y;
    }

    private void CalculateCanvasBounds()
    {
        var canvasRect = _mainUiCanvas.GetComponent<RectTransform>();
        var canvasSize = canvasRect.rect.size;

        //calculate edges relative to center
        _leftCanvasBorder = -canvasSize.x / 2;
        _rightCanvasBorder = canvasSize.x / 2;
        _topCanvasBorder = canvasSize.y / 2;
        _bottomCanvasBorder = -canvasSize.y / 2;
    }

    public static void UpdateScreenBounds()
    {
        Instance.CalculateScreenBounds();
    }
}