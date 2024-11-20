<a name="readme-top"></a>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/Ozzletroll/TAC-COM">
    <img src="https://github.com/Ozzletroll/TAC-COM/blob/main/TAC_COM/Static/Icons/live.ico" alt="Logo" width="30" height="30">
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
TAC/COM is a realtime VoIP effect processor that emulates sci-fi radio comms chatter.

- Realtime audio processing
- Push-to-talk
- Adjustable noise gate
- Themed mic click sfx
- Compatible with any VoIP software

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
2. If you don't already have it, download and install <a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0">.NET Desktop Runtime 8</a>.
3. Install virtual audio cable driver:
   <a href="https://vb-audio.com/Cable/">VB-Cable Virtual Audio Device</a>
4. Unzip and run the TAC/COM executable.

### Setup

1. Set the TAC/COM input device to your microphone input device.
2. Set the TAC/COM output device to your virtual audio cable input device.
3. Select your desired radio profile. Each profile features unique transmission click sfx and transmission static.
4. Set your chosen Push-to-talk keybind via the keybind edit button. Enable "passthrough" if you want the keybind to reach other apps, otherwise leave it disabled.

<p align="center">
  <img src="/TAC_COM/Static/Images/setup.png" alt="Screenshot of the TAC/COM window setup with a microphone input device and 'CABLE Input Input (VB-Audio Virtual Cable)' as the output device.">
</p>

## Discord Setup

Whilst instructions here are given for Discord, setup should be largely identical for any other VoIP platform.

1. Navigate to "User Settings".
2. Open "Voice & Video" under "App Settings".
3. Set input device to your virtual audio cable output device.

<p align="center">
  <img src="/TAC_COM/Static/Images/discord-setup-1.png" alt="Screenshot of Discord's Voice & Video settings, with the input device set to 'CABLE Output (VB-Audio Virtual Cable)'.">
</p>

4. Disable Discord's noise suppression.

<p align="center">
  <img src="/TAC_COM/Static/Images/discord-setup-2.png" alt="Screenshot of Discord's Voice & Video settings, with Noise Suppression set to 'None'.">
</p>

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
The threshold in dB at which noise attenuation begins. All input below the given threshold will be attenuated. Ranges from 0dB to -100dB.

### Output Level
The level boost in dB for the processed output signal. Used to normalise input/output levels. Ranges from -10dB to +10dB.

### Interference
Sets the level of the processed signal's "background noise" effect during transmission, as well as affecting the overall character of the processed signal. 
Increases signal distortion at high levels. Ranges from 0% to 100%.


<p align="right">(<a href="#readme-top">back to top</a>)</p>
