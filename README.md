<a name="readme-top"></a>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/Ozzletroll/TAC-COM">
    <img src="" alt="Logo" width="45" height="40">
  </a>

<h3 align="center">TAC/COM</h3>

  <p align="center">
    Mech Pilot Voice Comms Interface
    <br />
    <a href="">Download</a>
    .
    <a href="">Roadmap</a>
    ·
    <a href="">Report Bug</a>
    ·
    <a href="">Request Feature</a>
  </p>
</div>

<!-- ABOUT THE PROJECT -->
## Features
TAC/COM is a realtime VoIP effect processor that emulates sci-fi radio comms chatter.

- Realtime vocal effects
    - Push-to-talk
    - Adjustable noise gate
    - Themed mic click sfx

### Built With

- C#
- WPF
- CSCore
- Dapplo.Windows.Input
- AdonisUI

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
7. Bind your chosen Push-to-talk keybind. Enable "passthrough" if you want the keybind to reach other apps, otherwise leave disabled.

### Discord Setup
1. Navigate to "User Settings".
2. Open "Voice and Video" under "App Settings".
3. Set input device to your virtual audio cable output device.
4. Disable Discord's noise suppression.

IMPORTANT:
Unless disabled, Discord's noise suppression will interfere with TAC/COM's radio mic sfx.

### Usage
1. Click "enable" to allow audio passthrough. This will transmit your voice without any additional processing to your chosen output device.
2. Hit your chosen Push-to-talk key to begin "radio" transmission. Release the key to return to regular mic passthrough.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
