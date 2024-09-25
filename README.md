<a name="readme-top"></a>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/Ozzletroll/TAC-COM">
    <img src="https://github.com/Ozzletroll/TAC-COM/blob/main/Static/Icons/live.ico" alt="Logo" width="30" height="30">
  </a>
<h3 align="center">TAC/COM</h3>

  <p align="center">
    Mech Pilot Voice Comms Interface
    <br />
    <a href="https://github.com/Ozzletroll/TAC-COM/releases">Download</a>
    ·
    <a href="https://github.com/Ozzletroll/TAC-COM/issues/new?assignees=&labels=bug&projects=&template=bug_report.md&title=%5BBUG%5D">Report Bug</a>
    ·
    <a href="https://github.com/Ozzletroll/TAC-COM/issues/new?assignees=&labels=enhancement&projects=&template=feature_request.md&title=%5BFEATURE+REQUEST%5D">Request Feature</a>
  </p>
</div>

<!-- ABOUT THE PROJECT -->
## Features
TAC/COM is a realtime VoIP effect processor for Windows that emulates sci-fi radio comms chatter.

- Realtime vocal processing
- Push-to-talk
- Adjustable noise gate
- Themed mic click sfx

### Built With

- C#
- WPF
- CSCore
- Dapplo.Windows.Input
- AdonisUI

### Requirements

- Windows 10 or later

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

### Installation

1. Download the latest <a href="https://github.com/Ozzletroll/TAC-COM/releases">release</a>.
2. Install virtual audio cable driver:
   <a href="https://vb-audio.com/Cable/">VB-Cable Virtual Audio Device</a>
3. Unzip and run the TAC/COM executable.
4. Set the TAC/COM input device to your microphone input device.
5. Set the TAC/COM output device to your virtual audio cable output device.
6. Select your desired radio profile. Each profile features unique transmission click sfx and adjustable background noise.
7. Set your chosen Push-to-talk keybind via the keybind edit button. Enable "passthrough" if you want the keybind to reach other apps, otherwise leave it disabled.

## Discord Setup

Whilst instructions here are given for Discord, setup should be largely identical for any other VoIP platform.

1. Navigate to "User Settings".
2. Open "Voice & Video" under "App Settings".
3. Set input device to your virtual audio cable output device.
4. Disable Discord's noise suppression.

> **Unless disabled, Discord's noise suppression will interfere with TAC/COM's radio mic sfx. TAC/COM uses it's own built-in adjustable noise gate that does not affect the sfx channel.**

## Usage
1. Click "enable" to allow audio passthrough. This will transmit your voice without any additional processing to your chosen output device.
2. Hit your chosen Push-to-talk key to begin "radio" transmission. Release the key to return to regular mic passthrough.

## Tips
To set TAC/COM's noise gate threshold correctly, you can use Discord's "Mic Test" feature (under User Settings -> Voice & Video), allowing you to hear TAC/COM's output.

1. Set the noise gate threshold in TAC/COM to -100db.
2. Speak at a normal volume.
3. Gradually raise the threshold value until any background noise is eliminated. 
4. If your own voice becomes muted or suppressed, lower the threshold slightly.

Repeat this process whilst holding the Push-to-talk key. Ideally you should hear your own processed voice clearly, as well as the distinct mic open/close click tones on keydown/release.

> **Make sure to set the "Interference" level to 0% during testing, as this setting deliberately introduces noise to the processed signal.**

## Settings

### Noise Gate Threshold
The threshold in dB at which noise attenuation begins. Ranges from 0dB to -100dB.

### Output Level
The level boost in dB for the processed output signal. Ranges from -10dB to +10dB.

### Interferece
Sets the level of the processed signal's "background noise" effect, as well as affecting the overall "character" of the processed signal, increasing signal distortion at high levels. Ranges from 0% to 100%.


<p align="right">(<a href="#readme-top">back to top</a>)</p>
