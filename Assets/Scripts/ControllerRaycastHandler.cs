using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ControllerRaycastHandler : MonoBehaviour
{
    public Transform controllerTransform; // Reference to the left controller's transform
    public LayerMask layerMask;           // Specify the layers to raycast against
    public Transform rightControllerTransform; // Reference to the right controller (car)
    public CommandSender commandSender;   // Reference to the CommandSender script
    public int flag = 0;
    private bool isHolding = false;       // To track if the button is being held
    private Coroutine raycastCoroutine;
    private Coroutine vibrationCoroutine;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) // Button pressed
        {
            if (!isHolding)
            {
                isHolding = true;
                raycastCoroutine = StartCoroutine(PerformRaycastContinuously());
                vibrationCoroutine = StartCoroutine(KeepControllerAwake());
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) // Button released
        {
            if (isHolding)
            {
                isHolding = false;
                StopCoroutine(raycastCoroutine);
                StopCoroutine(vibrationCoroutine);
                StopVibration();
                commandSender.SendCommand("stop");
            }
        }
    }

    private IEnumerator PerformRaycastContinuously()
    {
        while (isHolding)
        {
            Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Vector3 targetPosition = hit.point; // Where the left controller is pointing
                targetPosition.y = 0; // Normalize Y to ignore height differences

                Vector3 carPosition = rightControllerTransform.position; // Car's current position
                carPosition.y = 0; // Normalize Y to ensure we are calculating on a horizontal plane

                Vector3 carForwardDirection = rightControllerTransform.forward; // Car's forward direction
                carForwardDirection.y = 0; // Normalize forward direction to horizontal

                float distance = Vector3.Distance(carPosition, targetPosition);
                Vector3 directionToTarget = (targetPosition - carPosition).normalized;
                float turnAngle = Vector3.SignedAngle(carForwardDirection, directionToTarget, Vector3.up);
                if (turnAngle > 6)
                {
                    flag = 2;
                }
                else if(turnAngle < -6){
                    flag = 1;
                }
                //Debug.Log($"Distance to Target: {distance}");
                //Debug.Log($"Turn Angle: {turnAngle} degrees");

                commandSender.SendCommand("forward");
                
                if (flag == 1)
                {
                    commandSender.SendCommand("left");
                }
                if (flag == 2)
                {
                    commandSender.SendCommand("right");
                }
                flag = 0;
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
            yield return new WaitForSeconds(0.033f); // 30 FPS (1/30 = 0.033 seconds)
        }
    }

    // Vibration control to keep the controller awake
    private IEnumerator KeepControllerAwake()
    {
        while (isHolding)
        {
            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
            yield return new WaitForSeconds(0.1f);
        }
        StopVibration();
    }

    private void StopVibration()
    {
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
    }
}
