# NavHead-VR
Virtual reality software development project for completion of the bachelor's degree in Computer Science and Media at the Hochschule der Medien - Germany.

## Topic  
**The Impact of Exclusive Head Movements (6DoF) on Navigation Efficiency, Accuracy, and Accessibility for Users with Physical Disabilities in Virtual Environments**  

## Research Question  
**How do exclusive head movements (6DoF) impact navigation efficiency, accuracy, and accessibility for users with physical disabilities in virtual environments?**  

## Technologies Used  
- Unity (3D Project – VR)
- C# (via JetBrains Rider)
- Blender (3D Assets)
- Meta Quest 3 (via Meta Link)

## Summary  

NavHead-VR is a virtual reality experience focused on exploring alternative and accessible interaction methods using **exclusive head movements (6DoF)**. It allows users to interact with a futuristic environment using **face alignment**, **blow-based input (simulated)**, and **keyboard-based toggles** as analog methods to test interaction logic and selection performance.

---

## How to Run the Application

### On Desktop (Developer Mode):

1. Open UnityHub and launch the NavHead-VR project.
2. In Unity, go to: Assets → NavHead → Scenes → MainScene
3. Connect your Meta Quest 3 to the computer using a USB-C cable.
4. On the headset, enable Meta Link when prompted.
5. In the headset’s floating menu, click +, then choose: UnityHub → NavHead MainScene
6. Back in Unity, press Play to enter the VR scene.

> You should now be inside the virtual room with your headset.

---

## Controls & Interaction

- The user starts in a futuristic room.
- Press **`C`** on the keyboard: A cube will appear in front of the user.
- The cube moves according to head movement.
- At the bottom right corner of the view, a selection method indicator is shown:
- STOP = Face Alignment selection (hold alignment for 4 seconds)
- Whistle icon = Blow selection (simulated)
- Press **`L`** to switch between selection modes.
- Press **`S`** to simulate the selection (e.g., blow input).

These modes allow experimental analysis of movement precision and alternative interaction efficiency.

---

## Opening the Project from a .zip File

If you downloaded the project as a .zip file, follow these steps to open and run it:

1. Unzip the file to any local folder on your computer.
2. Open Unity Hub and click Add Project.
3. Select the unzipped project folder (the one containing Assets/, Packages/, and ProjectSettings/).
4. Make sure you're using Unity version 6000.0.24f1 (the same version used for development).
5. Once the project is open in Unity, follow the steps in **How to Run the Application** to test it with the Meta Quest 3.

> Make sure the XR settings are properly configured for OpenXR with Oculus/Meta support if you're running the build for the first time.

---

## XR Settings Configuration (for Meta Quest 3)

If you’re opening the project for the first time or importing into a different Unity installation, verify the following XR configuration steps:

1. Open the XR Plug-in Management:
    - Go to Edit → Project Settings → XR Plug-in Management
    - Install the XR Plug-in Management package if prompted.
2. Enable OpenXR:
    - Under the Android tab, check the box for OpenXR (do not select Oculus here).
    - Unity will ask to install dependencies if they’re missing - accept all.
3. Set OpenXR as the default:
    - Go to Edit → Project Settings → XR Plug-in Management → OpenXR.
    - Under Features, make sure the following are checked:
        - Oculus Touch Controller Profile
        - Hand Tracking
        - Eye Gaze Interaction (optional)
     - Make sure Meta Quest Support is installed if available.
4. Switch to Android Build Platform:
    - Go to File → Build Settings
    - Select Android and click Switch Platform
5. Adjust Player Settings:
    - In Build Settings, click Player Settings:
        - Under Other Settings:
            - Set Minimum API Level to Android 10 (API level 29) or higher
            - Ensure Scripting Backend is set to IL2CPP
        - Under XR Plug-in Management, double-check OpenXR is active.

> Once configured, you can perform a Build and Run to deploy the app to your Meta Quest 3.

---

## Thesis

[Download the project thesis (PDF)](https://github.com/Kethella/NavHead-VR/blob/main/Abschlussarbeit_kc029_KethellaOliveira_43385_compressed.pdf)





