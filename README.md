https://github.com/ghhenrisar/Clock555

# Clock555

A customizable WPF desktop gadget built in Visual Studio Code, featuring a live clock and flexible window management.

## ✅ Core Features Implemented:

* **Live UK Clock**: Displays current time in HH:mm format, updating every second.
* **Transparent, Draggable WPF Window**: The main window is transparent, has a dark-themed gradient background, and can be easily dragged across the screen when not in full-screen mode.
* **Settings Panel for Clock Font Size**: A dedicated settings window allows users to adjust the clock's font size. The slider now supports very large font sizes (up to 400 or more, as configured).
* **Settings Persistence**: Saves and loads the clock's font size, window's normal position (`Left` and `Top`), and normal size (`Width` and `Height`) to/from `settings.ini`. It also remembers if the app was in full-screen mode upon last close.
* **Default Settings**: The app now defaults to a large font size (e.g., 800) and starts in full-screen mode (if `settings.ini` is absent or doesn't explicitly state otherwise).
* **Window Position and Size Management**:
    * **Dynamic Sizing**: In normal mode, the window dynamically resizes to fit its content (`SizeToContent="WidthAndHeight"`).
    * **Full-Screen Toggle**: A dedicated button allows switching between a stored "normal" window size/position (which dynamically sizes) and a full-screen display covering the entire primary monitor.
    * **Position Validation**: Automatically corrects and centers the window on the primary screen if loaded normal coordinates are off-screen or invalid.
* **GitHub Integration**: Project is managed on GitHub.

## 🔧 Recent Changes & Bug Fixes:

* **Removed Weather Forecast Features**: All code and UI elements related to fetching and displaying weather information (`WeatherViewModel.cs`, `WeatherModel.cs`, and associated logic in XAML and C#) have been completely removed to streamline the application.
* **Resolved Initial Build Errors**: Fixed various compilation errors across C# and XAML files (e.g., "Identifier expected", "Syntax error", "XML not valid", conditional expression ambiguity) caused by potential hidden characters, copy-paste issues, or syntax mistakes.
* **Fixed Window Disappearing Bug**: Resolved the issue where the window would "disappear" after dragging by correcting `settings.ini` key parsing for window position and implementing robust on-screen position validation.
* **Ensured Default Settings Application**: Corrected the timing of font size application on startup, ensuring that default or loaded settings are immediately visible without needing to open the settings panel.
* **Optimized Clock Timer Cleanup**: Adjusted `MainWindow.xaml.cs` to correctly stop the `DispatcherTimer` on window closure, removing unnecessary `Dispose()` calls.
* **Cleaned up Redundant Code**: Removed the unused `_isLoaded` flag.

## ⚠️ Outstanding Tasks / Optional Next Features:

* **Font Size Persistence Testing**: Although implemented, further testing is recommended to confirm consistent persistence of very large font sizes across various scenarios.
* **Theme Customization**: Implement more extensive options for UI colors, custom backgrounds, and a dedicated dark mode beyond the current basic dark theme.
* **Calendar/Date Display**: Integrate and make the `CalendarControl` (which is present in `MainWindow.xaml` but currently hidden) visible and functional, potentially allowing users to toggle its visibility.
* **App Packaging**: Further exploration of advanced packaging options, such as code-signing the executable for more professional distribution.