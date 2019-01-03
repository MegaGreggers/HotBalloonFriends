using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    public float maxCameraShake = 1f;
    float trauma; // clamp 0 to 1
    float traumaDiminish = 0.2f;

    float seed = 20f;

    float shake;
    
    float maxRoll = 10f;

    float maxOffsetX = 5f;
    float maxOffsetY = 5f;

    public GameObject smoothFollowCamera;
    private GameObject shakeyCamera;
    private Camera smoothCam;
    private Camera shakeyCam;

    // --- CameraShake Debug ---
    // GameObject shakeValueBox;
    // GameObject traumaValueBox;

    void Start ()
    {
        // smoothFollowCamera = GameObject.FindGameObjectWithTag("FollowCamera");
        shakeyCamera = gameObject;

        smoothCam = smoothFollowCamera.GetComponent<Camera>();
        shakeyCam = shakeyCamera.GetComponent<Camera>();

        shakeyCam.enabled = true;
        smoothCam.enabled = false;
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            AddTrauma(0.15f);

        if (Input.GetKeyDown(KeyCode.Keypad2))
            AddTrauma(0.25f);
            
        if (trauma > 0)
        {
            trauma -= traumaDiminish * Time.deltaTime;
            shake = Mathf.Pow(trauma, 3);
        }

        // Copy the camera size from the smoothCam and apply it to our shakyCam
        shakeyCam.orthographicSize = smoothCam.orthographicSize;

        ApplyRotationalShake();
        ApplyTranslationalShake();
    }

    public void AddTrauma(float addedTrauma)
    {
        trauma += addedTrauma;

        if(trauma > maxCameraShake)
            trauma = maxCameraShake;
    }

    void ApplyRotationalShake()
    {
        float roll = maxRoll * shake * Mathf.PerlinNoise(Time.time * seed, 0.0F);

        shakeyCamera.transform.localEulerAngles = smoothFollowCamera.transform.localRotation.eulerAngles + new Vector3( 0f, 0f, roll );
    }

    void ApplyTranslationalShake()
    {
        float offsetX = maxOffsetX * shake * Mathf.PerlinNoise(Time.time * seed, 0.0F);
        float offsetY = maxOffsetY * shake * Mathf.PerlinNoise(Time.time * seed + 1, 0.0F);

        shakeyCamera.transform.position = smoothFollowCamera.transform.position + new Vector3(offsetX, offsetY, 0f);
    }
}
