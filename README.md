# 🎮 Sync Dash

A fast-paced hyper-casual mobile game built in Unity where players control a glowing cube to jump over obstacles and collect orbs — while a ghost player on the left side mirrors their moves in real-time, simulating network play without an actual server.

---

## 🕹️ Game Concept

**Sync Dash** divides the screen into two vertical halves:

* **Right Side** – Player controls a glowing cube that moves forward automatically. The player taps to jump, avoids obstacles, and collects glowing orbs.
* **Left Side** – A "ghost player" mirrors the real player's actions with a configurable delay and smooth interpolation, simulating networked multiplayer behavior locally.

The game challenges players to score high while maintaining smooth gameplay and impressive visuals with optimized performance.

---

## 📦 Features

### 🔁 Core Gameplay

* Auto-running glowing cube controlled via screen tap.
* Tap to **jump** over obstacles and **collect orbs**.
* Progressive speed increase over time.
* Score system based on:

  * Number of orbs collected.

### 🧠 Real-Time Syncing

* Left side character **mirrors the player's actions** (jumping, movement, orb collection, obstacle hit).
* Optional **configurable delay** to simulate network lag.
* **Interpolation and smoothing** to mimic real-time sync accurately.
* Uses a **queue-based state buffer** to simulate multiplayer state syncing.

### 💥 Shaders & Visual Effects

* **Glowing Shader**: Applied to the main cube for a stylized look.
* **Dissolve Shader**: Obstacles dissolve when hit.
* **Orb Collection VFX**: Particle burst on orb collection.
* **Crash Effect**: Includes ripple distortion, chromatic aberration, and subtle screen shake.

### 🖼️ UI & Game Flow

* **Main Menu**: Start and Exit buttons.
* **Game Over Screen**: Restart and Main Menu options.
* **Score Display**: Visible during gameplay.
* **Speed FX**: Anime style VFX activates as game speeds up.

### 🚀 Optimization

* **Object Pooling**: Used for obstacles and orbs.
* **Frame-drop Prevention**: Efficient syncing and logic.
* **Mobile Ready**: Build size under 30MB, smooth on Android.

---

## 📹 Demo

[VIdeo Guide](https://drive.google.com/file/d/1srIeyJ_bj1ZfIBL3ktU8W9wfdFXYqnNi/view?usp=sharing)

---

## 📱 Build

* Platform: Android (.apk)
* Unity Version: **2022.3+** (LTS Recommended)
* Download APK:  [SyncDash](https://drive.google.com/file/d/1HVGFyAlhlTrTDPIK2WmzRS5RepIxawbY/view?usp=sharing)&#x20;

---

## 🛠️ How It Works (Mechanics & Logic)

### Player Controller

* Continuously moves forward on the **right half** of the screen.
* Tap = Jump
* Collides with:

  * **Obstacles** = Game Over
  * **Orbs** = Score Increase

### Ghost Player

* Placed on **left half** of the screen.
* Receives state updates (position, rotation, jump, orb collection) from Player using a **queue**.
* Interpolates between buffered states for **smooth movement**.
* Simulates slight network latency using a **configurable delay**.

---
