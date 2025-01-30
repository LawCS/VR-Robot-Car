import socket
import main

def handle_client(client_socket):
    try:
        while True:
            command = client_socket.recv(1024).decode('utf-8').strip()
            if not command:
                print("No data received. Client may have disconnected.")
                break

            print(f"Received command: {command}")
            if command == 'forward':
                main.forward()
            elif command == 'left':
                main.left()
            elif command == 'right':
                main.right()
            elif command == 'stop':
                main.stop()
            else:
                print("Unknown command received.")
    except Exception as e:
        print(f"An error occurred while handling a client: {e}")
    finally:
        client_socket.close()
        print("Client connection closed.")

def start_server():
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(('0.0.0.0', 65432))
    server_socket.listen(5)
    print("Server is listening on port 65432...")

    try:
        while True:
            client_socket, addr = server_socket.accept()
            print(f"Connection from {addr}")
            handle_client(client_socket)
    except Exception as e:
        print(f"An error occurred in the server: {e}")
    finally:
        server_socket.close()
        print("Server shutdown.")

if __name__ == "__main__":
    start_server()
