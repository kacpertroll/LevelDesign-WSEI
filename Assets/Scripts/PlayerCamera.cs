using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerBody;
    public Transform cameraHolder;
    public Transform cameraPosition;
    [Space]
    [Header("Sensitivity")]
    public float mouseSensitivity = 100f;
    [Header("Camera Smoothing")]
    public float smoothTime = 0.05f; // Czas wyg³adzania (ni¿sza = szybsza reakcja)
    private Vector2 currentMouseDelta;
    private Vector2 mouseDeltaVelocity;
    [Space]
    [Header("Field of View")]
    [Range(60f, 120f)]
    public float normalFOV = 60f;
    [Space]
    [Header("Headbob Base Settings")]
    public float baseHeadBobSpeed = 10f;
    public float baseHeadBobAmount = 0.05f;
    public float baseHeadTiltAmount = 2f;
    [Space]
    [Header("Headbob Multipliers")]
    public float sprintMultiplier = 1.5f;
    public float crouchMultiplier = 0.5f;
    [Space]
    [Header("Move Tilt Settings")]
    [SerializeField] private float tiltAngle = 2f;
    [SerializeField] private float tiltSpeed = 2f;

    private float currentTilt = 0f;
    private float targetFov;

    private bool isSprinting = false;
    private float xRotation = 0f;
    private Vector3 originalCamPosition;
    private Quaternion originalCamRotation;
    private CinemachineCamera playerCamera;

    private float bobTimer = 0f;

    private PlayerMovement playerMovement;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalCamPosition = cameraHolder.localPosition;
        originalCamRotation = cameraHolder.localRotation;
        playerCamera = gameObject.GetComponentInChildren<CinemachineCamera>();
        playerMovement = playerBody.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        LookAround();
        if (isSprinting)
            targetFov = normalFOV * 1.2f;
        else
            targetFov = normalFOV;

        playerCamera.Lens.FieldOfView = Mathf.Lerp(playerCamera.Lens.FieldOfView, targetFov, Time.deltaTime * 5f);
    }

    private void FixedUpdate()
    {
        HeadBobAndTilt();
        CameraMoveTilt();
        FovManipulation();
    }


    void LookAround()
    {
        Vector2 targetMouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        ) * mouseSensitivity * Time.deltaTime;

        // SmoothDamp do p³ynnego przejœcia
        currentMouseDelta = Vector2.SmoothDamp(
            currentMouseDelta,
            targetMouseDelta,
            ref mouseDeltaVelocity,
            smoothTime
        );

        xRotation -= currentMouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * currentMouseDelta.x);
    }

    void CameraMoveTilt()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float targetTilt = -moveInput * tiltAngle;

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        cameraPosition.localRotation = Quaternion.Euler(cameraPosition.localRotation.eulerAngles.x, cameraPosition.localRotation.eulerAngles.y, currentTilt);
    }

    void FovManipulation()
    {
        if (playerMovement.IsMoving() && Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else isSprinting = false;
    }

    void HeadBobAndTilt()
    {
        if (playerMovement.IsMoving() && playerMovement.IsGrounded())
        {
            // Ustalanie mno¿nika w zale¿noœci od ruchu
            float speedMultiplier = 1f;

            if (Input.GetKey(KeyCode.LeftShift)) speedMultiplier = sprintMultiplier; // Bieg
            if (Input.GetKey(KeyCode.C)) speedMultiplier = crouchMultiplier; // Kucanie

            // Zmienna prêdkoœæ head-bobbingu
            float finalHeadBobSpeed = baseHeadBobSpeed * speedMultiplier;
            float finalHeadBobAmount = baseHeadBobAmount * speedMultiplier;
            float finalHeadTiltAmount = baseHeadTiltAmount * speedMultiplier;

            bobTimer += Time.deltaTime * finalHeadBobSpeed;

            // Bobbing góra-dó³
            float bobOffset = Mathf.Sin(bobTimer) * finalHeadBobAmount;

            // Przechylanie na boki
            float tiltOffset = Mathf.Sin(bobTimer * 0.5f) * finalHeadTiltAmount;

            // Pozycja i rotacja kamery
            Vector3 targetPos = originalCamPosition + new Vector3(0f, bobOffset, 0f);
            Quaternion targetRot = Quaternion.Euler(xRotation, 0f, tiltOffset);

            // Smooth transition
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetPos, Time.deltaTime * finalHeadBobSpeed);
            cameraHolder.localRotation = Quaternion.Lerp(cameraHolder.localRotation, targetRot, Time.deltaTime * finalHeadBobSpeed);
        }
        else
        {
            // Reset do neutralnej pozycji
            bobTimer = 0f;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, originalCamPosition, Time.deltaTime * baseHeadBobSpeed);
            cameraHolder.localRotation = Quaternion.Lerp(cameraHolder.localRotation, Quaternion.Euler(xRotation, 0f, 0f), Time.deltaTime * baseHeadBobSpeed);
        }
    }
}