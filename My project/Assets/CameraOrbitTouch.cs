using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class CameraOrbitTouch : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    public Transform target;
    public RectTransform joystickRect; // assign your joystick's RectTransform here!

    [Header("Orbit Settings")]
    public float rotateSpeed = 0.12f;
    public float minPitch = 10f;
    public float maxPitch = 80f;
    public Vector3 focusOffset = new Vector3(0, 1.6f, 0);

    [Header("Distance Settings")]
    public float defaultDistance = 4.0f;
    public float minDistance = 0.8f;
    public float collisionSmooth = 0.08f;

    [Header("Smoothing")]
    public bool useSmoothing = true;
    public float smoothTime = 0.08f;

    private float yaw;
    private float pitch;
    private float currentDistance;
    private float desiredDistance;
    private Vector3 velocity = Vector3.zero;

    private bool isDragging = false;
    private Vector2 lastPointerPos;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbitTouch: No target assigned!");
            enabled = false;
            return;
        }

        currentDistance = defaultDistance;
        desiredDistance = defaultDistance;

        Vector3 offset = Camera.main.transform.position - (target.position + focusOffset);
        yaw = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        pitch = Mathf.Asin(offset.y / offset.magnitude) * Mathf.Rad2Deg;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Ignore if pointer starts over joystick
        if (joystickRect != null && RectTransformUtility.RectangleContainsScreenPoint(joystickRect, eventData.position))
            return;

        isDragging = true;
        lastPointerPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // Ignore dragging over joystick
        if (joystickRect != null && RectTransformUtility.RectangleContainsScreenPoint(joystickRect, eventData.position))
            return;

        Vector2 delta = eventData.position - lastPointerPos;
        lastPointerPos = eventData.position;

        yaw += delta.x * rotateSpeed;
        pitch -= delta.y * rotateSpeed;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 orbitCenter = target.position + focusOffset;
        Quaternion targetRot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 idealPos = orbitCenter + targetRot * new Vector3(0f, 0f, -defaultDistance);

        // --- Camera Collision ---
        if (Physics.Linecast(orbitCenter, idealPos, out RaycastHit hit))
        {
            float hitDist = Vector3.Distance(orbitCenter, hit.point) - 0.2f;
            desiredDistance = Mathf.Clamp(hitDist, minDistance, defaultDistance);
        }
        else
        {
            desiredDistance = defaultDistance;
        }

        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime / collisionSmooth);
        Vector3 desiredPos = orbitCenter + targetRot * new Vector3(0f, 0f, -currentDistance);

        if (useSmoothing)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, desiredPos, ref velocity, smoothTime);
            Quaternion desiredRot = Quaternion.LookRotation(orbitCenter - Camera.main.transform.position);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, desiredRot, 1f - Mathf.Exp(-25f * Time.deltaTime));
        }
        else
        {
            Camera.main.transform.position = desiredPos;
            Camera.main.transform.LookAt(orbitCenter);
        }
    }
}
