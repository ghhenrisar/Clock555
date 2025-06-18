https://github.com/ghhenrisar/CustomGadgetApp

# CustomGadgetApp

A customizable WPF desktop gadget built in Visual Studio Code, now streamlined for core functionality.

## ✅ Core Features Implemented:

- **Live UK Clock**: Displays current time including seconds (HH:mm:ss) and updates every second.
- **Transparent, Draggable WPF Window**: The main window is transparent, allows basic dark theming with a gradient background, and can be easily dragged across the screen.
- **Settings Panel for Clock Font Size**: A dedicated settings window allows users to adjust the clock's font size.
- **Settings Persistence**: Saves and loads the clock's font size and the window's position (`Left` and `Top` coordinates) to/from `settings.ini`.
- **Window Position Validation**: Automatically corrects and centers the window on the primary screen if loaded coordinates are off-screen or invalid, ensuring it's always visible when launched.
- **GitHub Integration**: Project is managed on GitHub.

## 🔧 Recent Changes & Bug Fixes:

- **Removed Weather Forecast Features**: All code and UI elements related to fetching and displaying weather information (including `WeatherViewModel.cs` and `WeatherModel.cs` and associated logic in `MainWindow.xaml.cs` and `MainWindow.xaml`) have been completely removed to simplify the project.
- **Fixed Initial Build Errors**: Resolved compilation errors across C# files caused by potential hidden characters or formatting issues.
- **Resolved Window Disappearing Bug**: Fixed the issue where the window would "disappear" after dragging. This was addressed by:
  - Ensuring `LoadSettings()` correctly parses both `Left`/`Top` and `WindowLeft`/`WindowTop` keys from `settings.ini` for backward compatibility.
  - Adding a validation step to reset the window position to a visible screen area if loaded coordinates are invalid or off-screen.
- **Optimized Clock Timer Cleanup**: Adjusted `MainWindow.xaml.cs` to correctly stop the `DispatcherTimer` on window closure without attempting to `Dispose()` it (as `DispatcherTimer` does not have that method).

## ⚠️ Outstanding Tasks / Optional Next Features:

- **Font Size Persistence Testing**: Further testing is needed to confirm consistent persistence of font size settings across application restarts.
- **Theme Customization**: Implement more extensive options for UI colors and a dedicated dark mode.
- **Calendar/Date Display**: Integrate and make the `CalendarControl` (already present in `MainWindow.xaml`, but currently hidden) visible and functional.
- **App Packaging**: Prepare the application for distribution to other users.
