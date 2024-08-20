using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;

public class BallMoverMRTK : MonoBehaviour, IMixedRealityPointerHandler
{
    public GameObject ball; // Assign the ball GameObject in the Inspector
    public LayerMask layerMask; // Define which layers should be considered for raycasting
    public float forceMultiplier = 10f; // Adjust the force applied to move the ball
    public float stoppingThreshold = 0.1f; // The distance at which the ball should stop

    private Rigidbody ballRigidbody;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody == null)
        {
            Debug.LogError("Rigidbody component not found on the ball!");
        }
    }

    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            float distanceToTarget = Vector3.Distance(ball.transform.position, targetPosition);
            if (distanceToTarget <= stoppingThreshold)
            {
                StopBall();
            }
            else
            {
                Vector3 direction = (targetPosition - ball.transform.position).normalized;
                ballRigidbody.AddForce(direction * forceMultiplier, ForceMode.Force);
            }
        }
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        // Perform raycast from the controller's position in its forward direction
        Ray ray = new Ray(eventData.Pointer.Position, eventData.Pointer.Rotation * Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            targetPosition = hit.point;
            Vector3 direction = (targetPosition - ball.transform.position).normalized;
            ballRigidbody.AddForce(direction * forceMultiplier, ForceMode.Impulse);
            isMoving = true;
        }
    }

    private void StopBall()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ball.transform.position = targetPosition; // Ensure the ball snaps to the target position
        isMoving = false;
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData) { }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    public void OnPointerUp(MixedRealityPointerEventData eventData) { }
}