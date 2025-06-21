# Clock555

**GitHub Repository**: [https://github.com/ghhenrisar/Clock555](https://github.com/ghhenrisar/Clock555)

A customizable WPF desktop gadget built with C# in Visual Studio Code, featuring a live clock and robust window management.

---

## ✅ Core Features Implemented:

* **Live Clock**: Displays the current time in HH:mm format, updating every second.
* **Draggable, Transparent Window**: The main window is transparent with a dark theme and can be dragged across the screen.
* **Settings Panel**: A dedicated settings window allows for real-time adjustment of the clock's font size.
* **Dual-Mode Settings Persistence**: The app saves all settings to `settings.ini`, maintaining two distinct profiles for:
    * **Normal Mode**: Remembers the window's last known size, position (`Left`, `Top`), and a specific font size.
    * **Full-Screen Mode**: Remembers the full-screen state and a separate font size for a cinematic view.
* **"Always on Top" Mode**: In its normal (windowed) mode, the clock will now float on top of all other applications, including the Windows Taskbar.
* **Intelligent Startup**: The app validates any saved position on startup to prevent loading off-screen (e.g., on a disconnected monitor) and will reset to center if the position is invalid.

## 🔧 Recent Changes & Bug Fixes:

* **Complete Settings Overhaul**: Reworked the entire settings logic to independently save and load separate font sizes and geometry for both windowed and full-screen modes.
* **Fixed Window Position Saving**: Resolved a series of bugs where the window's position would not save correctly when toggling modes or opening the settings panel. The latest position is now reliably saved to `settings.ini`.
* **Fixed "Always on Top" Taskbar Issue**: Implemented a robust P/Invoke solution to force the window to remain on top of the Windows Taskbar, resolving a stubborn OS-level layering issue.
* **Resolved Layout and Sizing Bugs**:
    * Fixed an issue where the window would not dynamically resize after the layout was changed.
    * Corrected the XAML layout to remove unwanted vertical space between the control buttons and the clock display.
* **Fixed XAML Build Errors**: Resolved a markup compilation error (`MC3000: 'x' is an undeclared prefix`) by correcting the resource dictionary's position in the XAML file.
* **Git Repository Cleanup**: Created and configured a `.gitignore` file to properly ignore build artifacts (`bin/`, `obj/`) and runtime-generated files (`log.txt`, `settings.ini`).

---
*This README was last updated on Saturday, 21 June 2025.*