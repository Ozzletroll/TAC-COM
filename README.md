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
TAC/COM is a realtime VoIP effects processor that emulates sci-fi radio comms chatter.

- Realtime audio processing
- Push-to-talk
- Adjustable noise gate
- Themed mic click sfx
- Compatible with any VoIP software

### Built With

- C#
- WPF
- CSCore
- NWaves
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

### Setup

1. Set the TAC/COM input device to your microphone input device.
2. Set the TAC/COM output device to your virtual audio cable input device.
3. Select your desired radio profile. Each profile features unique mic clicks, transmission static and voice processing.
4. Set your chosen push-to-talk keybind via the keybind edit button. Enable "passthrough" if you want the keybind to reach other apps, otherwise leave it disabled.

<p align="center">
  <img src="/TAC_COM/Static/Images/setup.png" alt="Screenshot of the TAC/COM window setup with a microphone input device and 'CABLE Input Input (VB-Audio Virtual Cable)' as the output device.">
</p>

## Discord Setup

Whilst instructions here are given for Discord, setup should be largely identical for any other VoIP program.

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

> **Unless disabled fully, Discord's noise suppression will interfere with TAC/COM's radio mic sfx. TAC/COM uses it's own built-in adjustable noise gate that does not affect the sfx channel.**

## Noise Gate Configuration
To set TAC/COM's noise gate threshold correctly, you can use Discord's "Mic Test" feature (under User Settings -> Voice & Video). Alternatively, you can temporarily select your headset speakers as TAC/COM's output device. Either option will allow you to hear your own processed voice for testing purposes.

1. Set the noise gate threshold in TAC/COM to -100db.
2. Speak at a normal volume.
3. Gradually raise the threshold value until any background noise is eliminated. 
4. If your own voice becomes muted or suppressed, lower the threshold slightly.

Repeat this process whilst holding the push-to-talk key. Ideally you should hear your own processed voice clearly with no background noise, as well as the distinct mic open/close click tones on push-to-talk keydown/release.

> **Make sure to set the "Noise" and "Interference" levels to 0% during testing, as these setting deliberately introduce noise and distortion to the processed signal.**

## Usage
1. Click "enable" to allow audio passthrough. This will transmit your voice without any additional processing to your chosen output device.
2. Hit your chosen push-to-talk key to begin "radio" transmission. Release the key to return to regular mic passthrough.

During PTT radio transmission, the audio is deliberately compressed and distorted. Therefore, it is important to adhere to correct radio protocol:
- Wait for the channel to be clear. Do not interrupt teammates.
- Pause a moment after pressing the PTT button.
- Be direct and short when communicating.
- Speak slowly and clearly.
- Use sci-fi military jargon at every opportunity.

## Settings

### Noise Gate Threshold
The threshold in dB at which noise attenuation begins. All input below the given threshold will be attenuated. Ranges from 0dB to -100dB.

### Output Level
The level boost in dB for the processed output signal. Used to normalise input/output levels. Ranges from -10dB to +10dB.

### Noise Level
Sets the level of the processed signal's "background noise" effect during transmission. Ranges from 0% to 100%.

### Interference Level
Sets the level of the processed signal's simulated signal degradation/interference. Ranges from 0% to 100%. 
Use sparingly for dramatic effect, as values above 50% become increasingly unintelligible.

## System Tray
While running, TAC/COM creates an icon on the system tray that provides a simple visual indicator of the push-to-talk transmission state.

| Icon | Description |
| --- | --- |
| <img height="32px" width="32px" align="center" src="/TAC_COM/Static/Icons/standby.ico" alt="A crossed-out white microphone icon, indicating that TAC/COM is not transmitting any audio data."> | **Standby:** No audio being passed to the output device. |
| <img height="32px" width="32px" align="center" src="/TAC_COM/Static/Icons/enabled.ico" alt="A white microphone icon, indicating that TAC/COM is transmitting unprocessed audio."> | **Enabled:** Regular microphone input being passed to the output voice, with no radio effect applied. |
| <img height="32px" width="32px" align="center" src="/TAC_COM/Static/Icons/live.ico" alt="A red microphone icon, indicating that TAC/COM is applying the radio effect to the output."> | **Transmitting:** Processed radio effect audio being passed to output device. |

Right clicking the system icon shows a dropdown menu of additional options to show/hide the app, or toggle if TAC/COM's window should stay on top of other windows.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
