using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Transform cameraTransform;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.7f;
    private float dampingSpeed = 1.0f;
    private Vector3 initialPosition;

    private void Awake()
    {
        instance = this;
        cameraTransform = GetComponent<Transform>();
        initialPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            cameraTransform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            cameraTransform.localPosition = initialPosition;
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        initialPosition = cameraTransform.localPosition;
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
