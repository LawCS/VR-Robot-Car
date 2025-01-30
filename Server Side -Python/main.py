#%%
import requests
from time import sleep
#%%
# base URL of the JSON-RPC server (if needed, replace with the IP address of your Raspberry Pi)
# RPC_URL = 'http://raspberrypi.local:9030'
# RPC_URL = 'http://192.168.1.184:8081'
RPC_URL = 'http://192.168.149.1:9030'

# helper function to send a JSON-RPC request
def send_rpc_request(method, params=None):
    payload = {
        "jsonrpc": "2.0",
        "method": method,
        "params": params or [],
        "id": 1
    }

    try:
        response = requests.post(RPC_URL, json=payload)
        response.raise_for_status()  # raise an exception for HTTP errors
        return response.json().get("result")
    except requests.RequestException as e:
        print(f"Error: {e}")
        return e

# function for each RPC method
def get_battery_voltage():
    return send_rpc_request("GetBatteryVoltage")

def set_pwmservo(*args):
    return send_rpc_request("SetPWMServo", list(args))

def set_movement_angle(angle):
    return send_rpc_request("SetMovementAngle", [angle])

def set_brush_motor(*args):
    return send_rpc_request("SetBrushMotor", list(args))

def get_sonar_distance():
    return send_rpc_request("GetSonarDistance")

'''
sets sonar led on/0 or off/1
'''
def set_sonar_rgb_mode(mode=0):
    return send_rpc_request("SetSonarRGBMode", [mode])

def set_sonar_rgb(r, g, b, index=0):
    return send_rpc_request("SetSonarRGB", [index, r, g, b])

def set_sonar_rgb_breath_cycle(index, color, cycle):
    return send_rpc_request("SetSonarRGBBreathCycle", [index, color, cycle])

def set_sonar_rgb_start_symphony():
    return send_rpc_request("SetSonarRGBStartSymphony")

def set_avoidance_speed(speed=50):
    return send_rpc_request("SetAvoidanceSpeed", [speed])

def set_sonar_distance_threshold(new_threshold=30):
    return send_rpc_request("SetSonarDistanceThreshold", [new_threshold])

def get_sonar_distance_threshold():
    return send_rpc_request("GetSonarDistanceThreshold")

def load_func(new_func=0):
    return send_rpc_request("LoadFunc", [new_func])

def unload_func():
    return send_rpc_request("UnloadFunc")

def start_func():
    return send_rpc_request("StartFunc")

def stop_func():
    return send_rpc_request("StopFunc")

def finish_func():
    return send_rpc_request("FinishFunc")

def heartbeat():
    return send_rpc_request("Heartbeat")

def get_running_func():
    return send_rpc_request("GetRunningFunc")

def color_tracking(*target_color):
    return send_rpc_request("ColorTracking", list(target_color))

def color_tracking_wheel(state=0):
    return send_rpc_request("ColorTrackingWheel", [state])

def visual_patrol(*target_color):
    return send_rpc_request("VisualPatrol", list(target_color))

def color_detect(*target_color):
    return send_rpc_request("ColorDetect", list(target_color))

def set_lab_value(*lab_value):
    return send_rpc_request("SetLABValue", list(lab_value))

def get_lab_value():
    return send_rpc_request("GetLABValue")

def save_lab_value(color=''):
    return send_rpc_request("SaveLABValue", [color])

def have_lab_adjust():
    return send_rpc_request("HaveLABAdjust")

def set_movement_velocity(velocity, direction, angular_rate):
    return send_rpc_request("SetMovement", [velocity, direction, angular_rate])

def forward():
    set_movement_velocity(50, 90, 0)

def backwards():
    set_movement_velocity(50, 270, 0)
def left():
    set_movement_velocity(50, 0, -90)

def right():
    set_movement_velocity(50, 0, 90)

def stop():
    set_movement_velocity(0, 0, 0)

def SleepeCal(ang):
    flag=0
    sleepingTime = (ang*2)/360
    if sleepingTime > 0:
        flag=1
    else:
        sleepingTime *= (-1)
    print(sleepingTime)
    if flag==1:
        rightRotation(sleepingTime*1.1)
    else:

        leftRotation(sleepingTime*1.1)
    return sleepingTime

def rightRotation(sleepingTime):
    set_movement_velocity(50,0,0)
    sleep(sleepingTime)
    stop()

def leftRotation(sleepingTime):
     set_movement_velocity(-50, 0, 0)
     sleep(sleepingTime)
     stop()


from flask import Flask, request, jsonify
import requests
from time import sleep

app = Flask(__name__)


@app.route('/command', methods=['POST'])
def handle_command():
    data = request.get_json()
    command = data.get('command')
    print("Raw data:", request.data)  # Print raw data received


    if command == 'forward':
        forward()
    elif command == 'left':
        left()
    elif command == 'right':
        right()
    elif command == 'stop':
        stop()
    else:
        stop()  # Default action if no valid command is received
        sleep(0.5)
        return jsonify({'status': 'Invalid or no command received, stopping'}), 400

    return jsonify({'status': 'Command received and executed'}), 200


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=9030)  # Adjust the port as necessary

#if __name__ == "__main__":
    # print(f'Battery Voltage: {get_battery_voltage()}')
    # print(f'Sonar Distance: {get_sonar_distance()}')
    # print(set_sonar_rgb_mode(0))
    # print(f'rgb: {set_sonar_rgb(r=0, g=255, b=0, index=0)}')
    # # print(set_sonar_rgb_breath_cycle(0, 0, 0))
    # # print(set_sonar_rgb_start_symphony())
    #


    # set_movement_velocity(velocity=0, direction=0, angular_rate=0)
    # sleep(1.1)
    #stop()

# %%
