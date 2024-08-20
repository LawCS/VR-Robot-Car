using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using System.Net.Sockets;
using System.Text;

public class CarControl : MonoBehaviour, IMixedRealityPointerHandler
{
    public LayerMask layerMask; // Define which layers should be considered for raycasting
    public float forceMultiplier = 10f; // Adjust the force applied to move the car

    private Vector3 targetPosition;
    private Vector3 currentPosition = new Vector3();
    private TcpClient client;

    private void Start()
    {
        // Initialize TcpClient
        client = new TcpClient("127.0.0.1", 65432);
    }

    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)

    {
        Debug.Log("pointer clicked successfully failed");
        // Perform raycast from the controller's position in its forward direction
        Ray ray = new Ray(eventData.Pointer.Position, eventData.Pointer.Rotation * Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("hello");
            // Calculate and save the target position
            targetPosition = hit.point;

            // Get the current position of the car

            // Calculate distance between current position and target position
            float distance = Vector3.Distance(currentPosition, targetPosition);

            // Calculate the direction vector
            Vector3 direction = targetPosition - currentPosition;

            // Calculate the angle between the car's forward direction and the direction to the target
            float angle = Vector3.Angle(currentPosition,targetPosition);
            currentPosition = targetPosition;
            // Send data to the server
            SendDataToServer(distance, angle);
        }
    }

    private void SendDataToServer(float distance, float angle)
    {
        Debug.Log("im sending to data..");
        NetworkStream stream = client.GetStream();

        // Create a message to send
        string message = $"Distance: {distance}; Angle: {angle}";

        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
        stream.Flush();
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData) { }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    public void OnPointerUp(MixedRealityPointerEventData eventData) { }

    private void OnApplicationQuit()
    {
        // Ensure client is closed when the application quits
        client.Close();
    }
}