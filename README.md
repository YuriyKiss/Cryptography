# Cryptography Application

## Functioning application with ability to encode and decode text using different algorithms (Caesar cipher, RSA, Vigener, etc.) built with Unity/C#.

I've had cryptography course at my university this term and, to be honest, this aplication is basically my approach to solving tasks our teacher gave us. 
Decision to leave this repository public is spontaneous and mostly without any goal. Hopefully someone finds it and has some profit out of it (idk, maybe you have similar task?)

## Application window
![image](https://user-images.githubusercontent.com/59531932/175290402-29464a75-8117-4c61-8f9a-bec67668e580.png)

Application window contains two text fields for encrypted/decrypted text, load/save data for both text fields, dropdown menu to select encryption algorithm, encrypt/decrypt button, author info and exit button. 
Pretty much that's it with some special features for individual algorithms.

## Features
Five different algorithms:
* Caesar cipher (has additional option for frequency distribution and brute force)
* Tritemius encryption (encryption oprions are 2-vector, 3-vector and slogan, additional options are frequency distribution and brute force)
* XOR cipher (key can be auto-generated or loaded from file)
* Vigener encryption
* RSA encryption

Also, this application contains Diffie-Hellman key generator, it is poorly made since it should connect between two or more users which my application successfully does not.

You can open images which are automatically converted to base64 thus you can encrypt them.

## Installation
1. Install Unity Editor (ver. 2020.3.34f1 or newer) via Unity Hub
2. Clone repository
3. Open Cryptography project via "Open" button in Unity Hub
4. Press "Play" button OR press Ctrl+B to Build & Run application

## TODO
* It is possible to open image by converting it to base64, but reverse process currently does not exist in this app.
* This application needs unit-tests.. i have like.. two tests for first two ciphers and that's clearly not enough.
* Some code is clearly made in a hurry and mostly there are no comments throughout the project.

## Used packages:
TextMesh Pro - you can't go anywhere without it, really

[Standalone File Browser](https://github.com/gkngkc/UnityStandaloneFileBrowser) which greatly helped to call windows file browser to save/load files.
