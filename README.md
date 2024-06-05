# Table of Contents
- [Introduction](#unity-recorder)
- [Features](#features)
- [Installation](#installation)
- [Setup](#setup)
- [Usage](#usage)
  - [Recording Objects](#recording-objects)
  - [Rewinding Time](#rewinding-time)
- [Configuration Options](#configuration-options)
  - [FPS Setting](#fps-setting)
  - [Duration Setting](#duration-setting)
  - [Layer Selection](#layer-setting)
- [Example Usage](#example-usage)
- [Troubleshooting](#troubleshooting)
- [FAQs](#faqs)

# Unity Recorder
__Unity Recorder__ is a tool designed to record the state of objects in your Unity game based on their layer. It allows you to specify the frame rate (FPS) and the duration of the recording in seconds. Additionally, it provides the functionality to rewind the time of those objects, effectively allowing you to replay their movements and actions.

## Features
- __Object Recording:__ Records the state of objects in the scene based on their layer.
- __Custom FPS:__ Allows setting a specific frame rate for recording.
- __Duration Control:__ Specify the recording duration in seconds.
- __Time Rewind:__ Rewind the time of recorded objects to replay their states.

## Installation
To install the Unity Recorder, follow these steps:

- Download the Unity Recorder package [here](https://github.com/Yan-COSTODE/UnityRecorder/releases/latest).
- Open your Unity project.
- Import the Unity Recorder package:
  - Go to `Assets > Import Package > Custom Package`.
  - Select the downloaded Unity Recorder package file.
  - Click `Import`.

## Setup
After installation, set up the Unity Recorder in your project:

- __Add the Recorder Script:__
  - Create an empty GameObject in your scene (e.g., `GameObject > Create Empty`).
  - Attach the `ObjectRecorder` script to this GameObject.
- __Configure Layers:__
  - Ensure the objects you want to record are assigned to the appropriate layers.
  - Go to `Edit > Project Settings > Tags and Layers` to manage layers.

## Usage

### Recording Objects
To start recording objects:

- Select the `ObjectRecorder` GameObject.
- In the Inspector, configure the recording settings:
  - __FPS:__ Set the frame rate for recording.
  - __Duration:__ Set the duration of the recording in seconds.
  - __Layer:__ Specify the layer of objects to ignore.

### Rewinding Time
To rewind the recorded objects:

- After recording, use the provided UI or script method to rewind the time.
- Call the `LaunchPlayback()` method on the `ObjectRecorder` script to rewind the objects' state.

## Configuration Options

### FPS Setting
- __Description__: Sets the frames per second for recording.
- __Usage__: `[SerializeField, Range(1, 144)] private int iRefreshRate = 30;`

### Duration Setting
- __Description__: Sets the duration in seconds for recording.
- __Usage__: `[SerializeField, Range(0.0f, 10.0f)] private float fMaxRecordedTime = 5.0f;`

### Layer Selection
- __Description__: Specifies the layer of objects to ignore.
- __Usage__: `[SerializeField] private LayerMask toIgnore;`

## Example Usage
Here's a basic example of how to set up and use the Unity Recorder in a script:
```csharp
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ObjectRecorder.Instance.LaunchPlayback();
    }
}
```

## Troubleshooting
- __Recording Not Starting:__ Ensure the `ObjectRecorder` script is attached to a GameObject and the layer settings are correct.
- __Objects Not Rewinding:__ Check if the `LaunchPlayback` method is being called correctly and the objects are on the specified layer.
- __Performance Issues:__ Reduce the recording FPS or duration if experiencing performance drops.

## FAQs
- __Does the recorder affect the performance of my game?__
  - Recording at high FPS or for long durations may impact performance. Adjust settings as needed for optimal performance.
  - I would suggest prioritizing FPS over duration to maintain a better visual effect.
