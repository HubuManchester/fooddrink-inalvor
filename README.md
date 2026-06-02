[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/uM_GSLJS)

# NutriBite (食光营养助手) — Major Assignment

**Food and Drink** cross-platform mobile app built with .NET MAUI.

**Author:** Wenhao Zhao (赵文浩)  
**Module:** 6G6Z0014 Mobile Computing  
**Institution:** Manchester Metropolitan University  
**Assessment ID:** 1CWK100

---

## Overview

NutriBite is a cross-platform mobile application that lets users browse, search, add, and explore food and drink records with rich nutritional information. The app integrates multiple mobile hardware features — camera, location, accelerometer, text-to-speech, vibration, and haptic feedback — to make meal tracking practical and engaging.

---

## Features

- Browse a list of food and drink items with nutritional summaries
- Search by name, category, description, or tags
- View detailed nutrition info (calories, protein, carbs, fat, allergens)
- Add new food/drink records with form validation
- Capture food photos using the device camera
- Get meal location with country, city, region, and GPS coordinates
- **Shake to Discover** — shake the device to get a random meal recommendation via the accelerometer
- Text-to-speech: read nutrition summaries and help content aloud
- Vibration and haptic feedback
- Dark mode, light mode, and follow-system theme
- Large-text accessibility mode
- Screen reader support with semantic annotations

---

## Hardware Features Used

| Feature | API | Page |
|---|---|---|
| Camera | `MediaPicker.CapturePhotoAsync()` | Hardware |
| Location / Geolocation | `Geolocation.GetLocationAsync()` | Hardware |
| Geocoding (reverse) | `Geocoding.GetPlacemarksAsync()` | Hardware |
| Accelerometer | `Accelerometer.ReadingChanged` | Hardware |
| Text-to-Speech | `TextToSpeech.SpeakAsync()` | Hardware / Detail |
| Vibration | `Vibration.Vibrate()` | Hardware / Add Item |
| Haptic Feedback | `HapticFeedback.Perform()` | Hardware / Add Item |

---

## Accessibility

- Light / dark / system theme toggle
- Large-text mode (22 % scale, applied across all pages)
- Semantic heading levels and hints on interactive controls
- `SemanticScreenReader.Announce()` for status changes
- References WCAG 2 guidelines for mobile accessibility

---

## Project Structure

```
FoodDrinkApp/
├── App.xaml / App.xaml.cs              # Application entry
├── AppShell.xaml / AppShell.xaml.cs     # Shell navigation (TabBar)
├── MainPage.xaml / MainPage.xaml.cs     # Food list + search
├── AddItemPage.xaml / AddItemPage.xaml.cs    # Add new record
├── FoodDetailPage.xaml / FoodDetailPage.xaml.cs  # Nutrition detail
├── HardwarePage.xaml / HardwarePage.xaml.cs    # Hardware demos
├── SettingsPage.xaml / SettingsPage.xaml.cs    # Theme + accessibility
├── Models/
│   └── FoodItem.cs                     # Data model
├── Services/
│   ├── FoodCatalogService.cs           # Data layer (mockapi.io + fallback)
│   ├── SpeechService.cs                # TTS wrapper
│   ├── AccessibilityService.cs         # Font scaling
│   └── MockApiConfig.cs                # API endpoint config
└── Platforms/
    └── Android/
        └── AndroidManifest.xml          # Permissions
```

---

## Data Source

The app prioritises data from a mockapi.io REST API. If the API is not configured or the network is unavailable, it falls back to local sample data so the app never crashes during a demo.

To configure the API endpoint, edit `FoodDrinkApp/Services/MockApiConfig.cs`.

See `MOCKAPI_SETUP.md` for step-by-step setup instructions.

---

## Build & Run

### Windows

```powershell
dotnet build .\FoodDrinkApp\FoodDrinkApp.csproj -f net9.0-windows10.0.19041.0 --no-incremental
```

### Android

```powershell
dotnet build .\FoodDrinkApp\FoodDrinkApp.csproj -f net9.0-android --no-incremental
dotnet build .\FoodDrinkApp\FoodDrinkApp.csproj -f net9.0-android -t:Run
```

> **Note:** The build output is redirected to `C:\MauiBuild\FoodDrinkApp\` via `Directory.Build.props` to avoid path issues with non-ASCII characters.

---

## Development Plan

| Phase | Task | Status |
|---|---|---|
| 1 | Project scaffold, Shell navigation, theme colours | ✅ Done |
| 2 | Food list UI, search, detail page | ✅ Done |
| 3 | Add-item form with validation | ✅ Done |
| 4 | Camera, location, geocoding hardware integration | ✅ Done |
| 5 | Text-to-speech, vibration, haptic feedback | ✅ Done |
| 6 | mockapi.io REST API integration | ✅ Done |
| 7 | Accessibility: themes, large text, screen reader | ✅ Done |
| 8 | Accelerometer shake-to-discover | ✅ Done |
| 9 | Tablet deployment, cross-platform polish | In progress |
| 10 | Screencast recording | In progress |
