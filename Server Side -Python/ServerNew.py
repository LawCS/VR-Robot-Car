import socket
from time import sleep
import main  # Import the car control functions


def control_car(distance, angle):
    try:
        # Call your car control function with distance and angle
        main.SleepeCal(angle)  # Rotate the car
        main.forward()  # Move the car forward
        sleep(distance / 5)  # Move for the given distance
        main.stop()  # Stop the car

    except Exception as e:
        print(f"Error controlling the car: {e}")


def parse_data(data):
    print("Parsing data...")

    # Split the data string by semicolon
    parts = data.split(';')
    distance = None
    angle = None

    try:
        # Extract and parse the distance (expected format: distance:10)
        distance_str = parts[0].split(':')[1].strip()
        distance = float(distance_str)

        # Extract and parse the angle (expected format: angle:90)
        angle_str = parts[1].split(':')[1].strip()
        angle = float(angle_str)

    except (IndexError, ValueError) as e:
        print(f"Error parsing data: {e}")
        print(f"Raw data: {data}")  # Print raw data for debugging
        return None, None

    print(f"Distance: {distance}, Angle: {angle}")
    return distance, angle


# Create a socket server that listens for incoming connections
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(('0.0.0.0', 65432))  # Listen on all available interfaces, port 65432
server_socket.listen()
print("Server is listening on port 65432...")

while True:
    # Accept incoming client connections
    client_socket, addr = server_socket.accept()
    print(f"Connection from {addr}")

    try:
        while True:
            # Receive data from the client
            data = client_socket.recv(1024).decode('utf-8')

            if not data:
                # If no data is received, the connection has been closed by the client
                break

            print(f"Received data: {data}")

            # Parse the incoming data (distance and angle)
            distance, angle = parse_data(data)
            if distance is None or angle is None:
                continue

            # Control the car with the parsed distance and angle
            control_car(distance, angle)

    except ConnectionResetError:
        print("Connection reset by the client")
    finally:
        # Close the client connection
        client_socket.close()
