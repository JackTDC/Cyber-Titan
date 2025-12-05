using UnityEngine;
using UnityEngine.UI;

public class FloatingJoystickDynamic : MonoBehaviour
{
    [Header("References")]
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public Canvas canvas;

    [Header("Settings")]
    public float radius = 100f;
    public float handleLimit = 1f;
    public float fadeSpeed = 10f;

    private CanvasGroup canvasGroup;
    private Vector2 input;
    private Vector2 startPos;
    private bool isActive;
    private int activeTouchId = -1;
    private Coroutine fadeRoutine;

    public Vector2 Direction => input;

    void Start()
    {
        // Add or find a CanvasGroup for fading
        canvasGroup = joystickBackground.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = joystickBackground.gameObject.AddComponent<CanvasGroup>();

        // Start hidden
        joystickBackground.gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // For mouse in editor
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Input.mousePosition;
            if (pos.x < Screen.width / 2f)
                ActivateJoystick(pos);
        }
        else if (Input.GetMouseButton(0) && isActive)
        {
            UpdateHandle(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && isActive)
        {
            DeactivateJoystick();
        }
#else
        // For real touch devices
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2f)
            {
                ActivateJoystick(touch.position);
                activeTouchId = touch.fingerId;
            }
            else if (isActive && touch.fingerId == activeTouchId)
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    UpdateHandle(touch.position);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    DeactivateJoystick();
                    activeTouchId = -1;
                }
            }
        }
#endif
    }

    private void ActivateJoystick(Vector2 screenPos)
    {
        isActive = true;

        // Convert screen to canvas local
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out startPos
        );

        joystickBackground.anchoredPosition = startPos;
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickBackground.gameObject.SetActive(true);

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(Fade(1f));

        UpdateHandle(screenPos); // <-- instantly sync handle position
    }

    private void UpdateHandle(Vector2 screenPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        Vector2 clamped = Vector2.ClampMagnitude(localPoint, radius);
        input = clamped / radius;
        joystickHandle.anchoredPosition = clamped * handleLimit;
    }

    private void DeactivateJoystick()
    {
        isActive = false;
        input = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(Fade(0f, true));
    }

    private System.Collections.IEnumerator Fade(float target, bool disableAfter = false)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime * fadeSpeed);
            yield return null;
        }

        if (disableAfter && Mathf.Approximately(target, 0f))
            joystickBackground.gameObject.SetActive(false);
    }

    public static Vector2 GetAxis()
    {
        var j = FindObjectOfType<FloatingJoystickDynamic>();
        return j != null ? j.Direction : Vector2.zero;
    }
}
