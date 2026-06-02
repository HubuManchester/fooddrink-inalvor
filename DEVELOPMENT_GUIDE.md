# NutriBite Project Development Guide

## 1. Project Overview

This project is a cross-platform mobile app built with .NET MAUI, named **NutriBite**. The theme aligns with the "Food and Drink" requirements from the assignment brief. The core functionality covers recording foods and drinks, viewing nutritional information, and enhancing the user experience through mobile device hardware.

Key app features:

- Browse a list of food and drink items
- Search foods, drinks, categories, or tags
- Add new food or drink records
- View nutritional details
- Capture food photos
- Get meal location including country, city, region, and coordinates
- Text-to-speech for reading nutrition summaries or help content
- Stop speech playback to prevent it from continuing after leaving the page
- Vibration and haptic feedback
- Large-text mode, dark/light themes, and screen reader announcements

## 2. Assessment Criteria Compliance

| Assessment Criterion | Weight | Current Implementation |
|---|---|---|
| UI/UX Design and Accessibility | 30% | Multi-page UI built with XAML; warm food-themed colour palette; bottom tab navigation; dark/light/system theme support; large-text mode; semantic hints and screen reader announcements. |
| Use of Mobile Hardware | 20% | Camera, Location/Geolocation, Geocoding, Text-to-speech, Vibration, Haptic feedback, and Accelerometer — exceeding the high-mark threshold of 4 hardware features. |
| Functionality | 20% | Food list, search, detail view, add record, hardware demo, settings page, speech stop, location address display, and shake-to-discover. |
| Validation and Error Handling | 10% | Add-item page checks required fields and non-negative numbers; camera, location, TTS, vibration, and accelerometer all include exception handling and user prompts. |
| Code Quality | 10% | Layered architecture: Models, Services, Pages/code-behind; reusable services for speech, font scaling, and food data logic; clear naming conventions. |
| Deployment | 5% | Project supports Android and Windows; verified via command-line builds. |
| GitHub Usage | 5% | Project includes README and this development guide; requires regular, meaningful commits on GitHub. |

Conclusion: The project meets the core requirements of the coursework in terms of functional coverage. Note that the final mark depends primarily on the screencast — each criterion must be demonstrated clearly during recording.

## 3. Project Structure

```text
FoodDrinkApp/
  App.xaml
  App.xaml.cs
  AppShell.xaml
  AppShell.xaml.cs
  MainPage.xaml
  MainPage.xaml.cs
  AddItemPage.xaml
  AddItemPage.xaml.cs
  FoodDetailPage.xaml
  FoodDetailPage.xaml.cs
  HardwarePage.xaml
  HardwarePage.xaml.cs
  SettingsPage.xaml
  SettingsPage.xaml.cs
  Models/
    FoodItem.cs
  Services/
    AccessibilityService.cs
    FoodCatalogService.cs
    SpeechService.cs
    MockApiConfig.cs
  Platforms/
    Android/
      AndroidManifest.xml
  Resources/
    Styles/
      Colors.xaml
      Styles.xaml
```

## 4. Key File Descriptions

### App.xaml / App.xaml.cs

`App.xaml` loads the global resource dictionary, including colours and control styles.

`App.xaml.cs` is one of the application entry points. It creates the main window and loads `AppShell`:

```csharp
return new Window(new AppShell());
```

### AppShell.xaml / AppShell.xaml.cs

`AppShell.xaml` defines the app navigation structure. It uses a bottom TabBar:

- Foods
- Hardware
- Settings

`AppShell.xaml.cs` registers the detail and add-item page routes:

```csharp
Routing.RegisterRoute(nameof(AddItemPage), typeof(AddItemPage));
Routing.RegisterRoute(nameof(FoodDetailPage), typeof(FoodDetailPage));
```

This allows the main page to navigate to the add and detail pages via Shell.

### MainPage.xaml / MainPage.xaml.cs

The main page displays the food and drink list.

Key controls:

- `SearchBar` — search foods, drinks, categories, or tags
- `CollectionView` — display food cards
- `Button` — navigate to add page or detail page
- `RefreshView` — pull-to-refresh the list

Core logic:

```csharp
FoodCollection.ItemsSource = FoodCatalogService.Search(query);
```

The main page does not maintain data directly; it fetches the list through `FoodCatalogService`, keeping the code cleaner.

### AddItemPage.xaml / AddItemPage.xaml.cs

The add page lets users create a new food or drink record.

Form fields:

- Name
- Category
- Description
- Calories
- Protein
- Carbs
- Fat
- Allergy note

Validation logic in `ValidateForm`:

- Name must not be empty
- Category must be selected
- Description must not be empty
- Calories, protein, carbs, and fat must be non-negative numbers

Validation failures display an error panel and trigger vibration feedback:

```csharp
ShowValidation(validationMessage);
Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(250));
```

On successful save:

```csharp
FoodCatalogService.Add(new FoodItem { ... });
```

### FoodDetailPage.xaml / FoodDetailPage.xaml.cs

The detail page displays nutritional information for a single food or drink item.

Displayed content:

- Name
- Category
- Calories
- Protein, carbs, fat
- Description
- Allergy note

The detail page supports:

- Reading the nutrition summary aloud
- Stopping speech
- Vibration alert

Speech uses `SpeechService`:

```csharp
await SpeechService.SpeakChineseAsync(currentItem.AccessibleSummary);
```

Speech is automatically stopped when leaving the page:

```csharp
protected override void OnDisappearing()
{
    SpeechService.Stop();
    base.OnDisappearing();
}
```

This prevents audio from continuing after navigating away.

### HardwarePage.xaml / HardwarePage.xaml.cs

The hardware page centrally demonstrates mobile device hardware capabilities.

Implemented hardware features:

| Feature | API |
|---|---|
| Photo capture | `MediaPicker.Default.CapturePhotoAsync()` |
| Location | `Geolocation.Default.GetLocationAsync()` |
| Reverse geocoding | `Geocoding.Default.GetPlacemarksAsync()` |
| Accelerometer (shake) | `Accelerometer.Default.ReadingChanged` |
| Text-to-speech | `TextToSpeech.Default.SpeakAsync()`, wrapped in `SpeechService` |
| Vibration | `Vibration.Default.Vibrate()` |
| Haptic feedback | `HapticFeedback.Default.Perform()` |

Location logic:

```csharp
var location = await Geolocation.Default.GetLocationAsync(request);
CoordinateLabel.Text = $"Lat {location.Latitude:F5}, Lon {location.Longitude:F5}";
LocationLabel.Text = await BuildAddressTextAsync(location);
```

Reverse geocoding is used first. If the emulator does not return a country and city, fallback logic is applied. For example, the default Google emulator coordinates `37.422, -122.084` will display:

```text
United States / California / Mountain View
```

Accelerometer shake-to-discover logic:

- Start/Stop button toggles `Accelerometer` monitoring at `SensorSpeed.Game`
- Computes the Euclidean norm of delta across X, Y, Z axes
- Triggers a "shake" when the delta exceeds a 1.2g threshold
- 2-second cooldown prevents duplicate triggers per shake
- On each detected shake, a random food item is selected from `FoodCatalogService`
- Recommendation displayed with haptic feedback and screen reader announcement
- Shake counter shown for screen-recorded verification
- Accelerometer is automatically stopped in `OnDisappearing()`

Haptic feedback includes a counter for easy screencast verification:

```csharp
feedbackTestCount++;
FeedbackCountLabel.Text = $"Haptic feedback tests: {feedbackTestCount}";
```

### SettingsPage.xaml / SettingsPage.xaml.cs

The settings page handles accessibility-related features.

Currently supports:

- Follow system theme
- Light theme
- Dark theme
- Large-text mode

Large-text mode is implemented through `AccessibilityService`. Toggling the switch immediately scales text on the current page; navigating to other pages applies font scaling in each page's `OnAppearing`.

```csharp
AccessibilityService.LargeTextEnabled = e.Value;
AccessibilityService.ApplyFontScale(this);
```

### Models/FoodItem.cs

`FoodItem` is the food/drink data model.

Key properties:

- `Name`
- `Category`
- `Description`
- `Calories`
- `Protein`
- `Carbs`
- `Fat`
- `AllergyNote`
- `Tags`

It also provides computed properties for UI display and speech:

```csharp
public string CaloriesLabel => $"{Calories} kcal";
public string MacroSummary => $"Protein {Protein}g, carbs {Carbs}g, fat {Fat}g";
public string AccessibleSummary => $"{Name}. {Category}. {Calories} kcal. {MacroSummary}. {AllergyNote}";
```

### Services/FoodCatalogService.cs

The food data service reads food information from mockapi.io or local fallback data:

- Prioritises mockapi.io REST API requests
- Falls back to local sample data when the network is unavailable or no API is configured
- Searches foods
- Gets details by ID
- Adds new food records

Search matches against:

- Name
- Category
- Description
- Tags

The mockapi.io endpoint is configured in:

```text
FoodDrinkApp/Services/MockApiConfig.cs
```

Change `EndpointUrl` to the Resource address generated by mockapi.io:

```csharp
public const string EndpointUrl = "https://682xxxx.mockapi.io/api/v1/foods";
```

See `MOCKAPI_SETUP.md` in the project root for detailed setup instructions.

### Services/SpeechService.cs

The speech service provides a unified wrapper for Text-to-speech.

Features:

- Stops any previous speech before starting a new one
- Supports stopping speech mid-playback
- Prefers English locale
- Configures volume and pitch for a slightly more natural sound

Core code:

```csharp
var options = new SpeechOptions
{
    Volume = 0.9f,
    Pitch = 1.05f,
    Locale = await FindEnglishLocaleAsync()
};
```

Note: speech quality depends on the TTS voice packs installed on the device. Physical devices usually sound more natural than Android emulators.

### Services/AccessibilityService.cs

The accessibility font-scaling service.

It traverses Label, Button, Entry, Editor, Picker, and SearchBar controls on the current page, scaling or restoring their font sizes based on the large-text toggle.

### Platforms/Android/AndroidManifest.xml

Android permission configuration file.

Currently includes:

```xml
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-permission android:name="android.permission.VIBRATE" />
```

These permissions correspond to camera, location, and vibration features.

### Resources/Styles/Styles.xaml

Global control styles.

Currently unified for:

- Button
- Entry
- Editor
- Picker
- SearchBar
- Label
- Shell TabBar

The visual style uses a warm cream background, tomato red, baked orange, and basil green palette to match the food and drink theme.

## 5. Build & Run

Because the original project path contained non-ASCII characters and `&`, which can cause issues with Android build tooling, `Directory.Build.props` redirects build output to:

```text
C:\MauiBuild\FoodDrinkApp\
```

Windows build:

```powershell
dotnet build .\FoodDrinkApp\FoodDrinkApp.csproj -f net9.0-windows10.0.19041.0 --no-incremental
```

Android build:

```powershell
dotnet build .\FoodDrinkApp\FoodDrinkApp.csproj -f net9.0-android --no-incremental
```

Android run:

```powershell
dotnet build .\FoodDrinkApp\FoodDrinkApp.csproj -f net9.0-android -t:Run
```

If Visual Studio shows "Cannot start this project for Android", there is usually no online device. Start an Android emulator or connect a physical device first.

## 6. Screencast Demo Guide

Recommended recording order:

1. Introduce the app theme: Food and Drink, app name "NutriBite".
2. Show the main page UI, search bar, food cards, and bottom navigation.
3. Search for a food item, e.g. "Drink" or "Breakfast".
4. Open a detail page and show nutritional information.
5. Tap "Read Summary", then "Stop Speech", and explain that speech stops automatically when leaving the page.
6. Tap "Vibrate", explaining the use of Vibration and Haptic feedback.
7. Open the add page, deliberately leave fields blank or enter invalid numbers, and show validation and error prompts.
8. Correctly add a food record, go back to the main page, and show the new entry.
9. Open the hardware page and demonstrate the camera.
10. Demonstrate location, explaining that country, city, region, and coordinates are displayed.
11. Show "Shake to Discover" — start accelerometer monitoring, shake the device, and show the random recommendation.
12. Demonstrate read help aloud and stop speech.
13. Demonstrate haptic feedback and explain the changing counter for screencast verification.
14. Open the settings page and demonstrate dark/light themes and large-text mode.
15. Walk through the code structure: Models, Services, XAML pages, AndroidManifest permissions.
16. Show Android and Windows build or run results.
17. Show GitHub commit history and README.

## 7. Important Reminders

- GitHub usage accounts for 5% — commit regularly, not in a single push at the end.
- The screencast is the primary marking source — clearly explain each assessment criterion.
- If location does not show a real city in the emulator, set a location in Emulator Extended Controls, or test on a physical device.
- Text-to-speech voice quality depends on the system TTS engine; for more natural output, install high-quality TTS voices on the test device.
