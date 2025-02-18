# VR Car Controller
**THIS PROJET WAS DONE IN THE SEMESTERIAL PROJET IN THE UNIVERSITY OF HAIFA**
****if you need help, dont hesitate to mail me: khourylawrence@gmail.com****

**** Take your files from **MASTER branch**, not main!! *****


## Overview
*VR Car Controller* is a project that enables users to control a robotic car using the controllers of a Meta Quest 2 VR headset. The system is built in Unity, where all vector calculations take place, and it communicates with the car's server via HTTP. The server and Unity interact using Flask.

## Features
- Control a robotic car using VR controllers.
- Built with *Unity* and *C#* for VR interactions.
- Uses *Flask* to facilitate communication between Unity and the robotic car.
- Supports four simple commands: Forward, Left, Right, and Stop.
- Real-time command sending at *30 FPS*.
- Meta Quest 2 integration for an immersive experience.

## Installation & Setup
### 1. Unity Installation
- Download and install *Unity Hub* from [Unity's official website](https://unity.com/).
- Install a Unity version that supports Meta Quest 2 (recommended: *Unity 2021.3 LTS or later*). - ** we had 2022.3.32f1
- Set up a *3D project* in Unity. ( i dont believe you need this once you "pull" this project from github. )

### 2. Meta Quest 2 Setup
- Enable *Developer Mode* on your Meta Quest 2 headset.
- Install the *Oculus Integration* package from the Unity Asset Store.
- Ensure the headset is connected to your PC via *Oculus Link* or *Air Link*. ( cable link through USB 3 is preferred )

### 3. Server Setup (Flask & HTTP Communication)
- Install Python 3 (if not already installed) - we had 3.11.9 and it worked fine.
- Install Flask:
  bash
  pip install flask
  
- Run the Flask server (sample script included in the repository).

## Algorithm Workflow
1. The *CommandSender* script in Unity continuously sends commands at *30 FPS* when the VR controller's *primary button* is pressed.
2. The default command sent is Forward as long as *-6° < angle < 6°*.
3. If *angle > 6°*, the command changes to Right.
4. If *angle < -6°*, the command changes to Left.
5. The Unity script sends these commands to the *Flask server*, which relays them to the robotic car via HTTP.

## Technologies Used
- *Unity* (for VR environment and scripting)
- *C#* (for handling vector calculations and sending commands)
- *Python (Flask)* (for handling server-side communication)
- *HTTP requests* (to send commands to the robotic car)

## Contribution
Feel free to contribute to this project! To do so:
1. Fork the repository.
2. Create a new branch (feature-branch).
3. Make your changes and commit them.
4. Submit a pull request.
