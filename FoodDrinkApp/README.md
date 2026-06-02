# NutriBite

NutriBite is a .NET MAUI "Food and Drink" coursework app. It lets users record foods and drinks, view nutrition summaries, validate user input, and demonstrate mobile device hardware features.

## Key Features

- Food and drink list with search and detail pages.
- Add-item form with required-field and numeric validation.
- Capture food photos with the camera and preview them.
- Use location to record where a meal was consumed or purchased.
- Text-to-speech for reading nutrition summaries and help content aloud.
- Vibration and haptic feedback for user alerts.
- Theme switching (light / dark / system) and large-text mode.
- Semantic labels, screen reader announcements, and clear validation messages.

## Assessment Criteria Coverage

- UI/UX & Accessibility: XAML pages, bottom tab navigation, consistent visual style, dark mode, semantic descriptions, and screen reader announcements.
- Mobile Hardware: Camera, location, text-to-speech, vibration, haptic feedback, and accelerometer.
- Functionality: List, search, add, detail, settings, and hardware demo flows.
- Validation & Error Handling: Required-field checks, numeric checks, permission errors, and hardware-unavailable alerts.
- Code Quality: Models and services separated, clear naming, reusable catalog service, and well-scoped page code-behind.
- Deployment: .NET MAUI cross-platform app targeting Android and Windows.
- GitHub Usage: Regular commits with descriptive messages — e.g. "add food list page", "implement hardware page", "add input validation".

## How to Run

Open `FoodDrinkApp.csproj` or `FoodDrinkApp.sln` in Visual Studio 2022 with the .NET MAUI workload installed.

Recommended demo targets:

- Android emulator
- Windows Machine

Windows build:

```powershell
dotnet build .\FoodDrinkApp.csproj -f net9.0-windows10.0.19041.0
```

Android build:

```powershell
dotnet build .\FoodDrinkApp.csproj -f net9.0-android
```

The project uses `Directory.Build.props` to redirect build output to `C:\MauiBuild\NutriTrack\`, avoiding Android packager issues with non-ASCII file paths.

## Screencast Demo Checklist

- Introduce the "Food and Drink" theme and the NutriBite app concept.
- Show search, detail page, and adding a new record.
- Demonstrate validation when leaving required fields blank or entering invalid numbers.
- Demonstrate camera, location, text-to-speech, vibration, haptic feedback, and accelerometer.
- Show dark mode and large-text mode.
- Walk through key code files: models, services, pages, and Android permission configuration.
- Show Android and Windows deployment results.
- Show GitHub commit history and README.
