using UnityEngine;

// By Gurtej S (Mar 23, 2023)
// TODO: fix this undecipherable mess
// TODO: implement crouch, prone?, slide?, wall run?
public class PlayerController : MonoBehaviour
{
    enum Movement { Walk, Run, Sprint };
    Movement mode = Movement.Run;

    [Header("Movement")]
    [SerializeField, Range(1.0f, 20.0f)] float playerSpeed = 5.0f;
    [SerializeField, Range(1.0f, 10.0f)] float jumpHeight = 1.5f;
    [SerializeField, Range(-20.0f, 0.0f)] float gravity = -9.81f;
    [Header("Look")]
    [SerializeField, Range(1.0f, 10.0f)] float lookSensitivity = 5.0f;
    [Header("Head Bob")]
    [SerializeField, Range(0.0f, 0.1f)] float headBobAmplitude = 0.015f;
    [SerializeField, Range(0.0f, 30.0f)] float headBobFrequency = 10.0f;

    CharacterController player;
    Camera cam;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Vector3 motion = Vector3.zero;
    private Vector3 camPos;
    private float camFov;

    void Start()
    {
        player = GetComponent<CharacterController>();
        cam = Camera.main;
        camPos = cam.transform.localPosition;
        camFov = cam.fieldOfView;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // TODO: refactor this weirdness
        if (Input.GetKey("left shift")) mode = Movement.Sprint;
        else if (Input.GetKey("left alt")) mode = Movement.Walk;
        else mode = Movement.Run;

        switch (mode)
        {
            case Movement.Walk:
                Move(0.4f);
                Look(0.8f);
                HeadBob(0.5f);
                break;
            case Movement.Run:
                Move();
                Look();
                HeadBob();
                break;
            case Movement.Sprint:
                Move(1.4f);
                Look(1.2f);
                HeadBob(2.0f);
                break;
        }
    }

    void Move(float modifier = 1.0f)
    {
        if (player.isGrounded)
        {
            if (motion.y < 0) motion.y = 0.0f;
            if (Input.GetButtonDown("Jump")) motion.y = Mathf.Sqrt(-2.0f * gravity * jumpHeight * modifier);
        }
        motion.y += gravity * Time.deltaTime;

        // to the reader, im sorry
        motion = (
                player.transform.right * Input.GetAxisRaw("Horizontal")
              + player.transform.forward * Input.GetAxisRaw("Vertical")
            ).normalized * playerSpeed * modifier + Vector3.up * motion.y;
        player.Move(motion * Time.deltaTime);
    }

    void Look(float modifier = 1.0f)
    {
        yaw += Input.GetAxis("Mouse X") * lookSensitivity;
        pitch += Input.GetAxis("Mouse Y") * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -89.0f, 89.0f); // No front or back flips
        cam.transform.localRotation = Quaternion.Euler(-pitch, 0, 0); // Right-hand rule around Vector3.left
        player.transform.localRotation = Quaternion.Euler(0, yaw, 0); // Right-hand rule around Vector3.down

        // TODO: better structure
        if (motion.z != 0.0f)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camFov * modifier, 10 * Time.deltaTime);
        }
        else if (mode == Movement.Run)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camFov, 10 * Time.deltaTime);
        }
    }

    // TODO: maybe move into another component?
    void HeadBob(float modifier = 1.0f)
    {
        // TODO: Change bobbing amount when not in motion, not on ground, aiming down sights, etc.
        cam.transform.localPosition = camPos + new Vector3(
            modifier * Mathf.Cos(Time.time * headBobFrequency / 2) * headBobAmplitude * 2,
            modifier * Mathf.Sin(Time.time * headBobFrequency) * headBobAmplitude
        );
    }
}
