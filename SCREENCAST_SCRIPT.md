# NutriBite Screencast Script

Estimated duration: 12–14 minutes

---

## Part 1 — Introduction (30 seconds)

> Hi, my name is Wenhao Zhao and this is my final assignment for Mobile Computing — 6G6Z0014. My app is called NutriBite, and the theme is Food and Drink. It's a cross-platform mobile app built with .NET MAUI that lets users browse foods and drinks, check nutrition info, and use the phone's hardware like camera, GPS, accelerometer, and text-to-speech. I'm going to walk through each part of the marking criteria now.

---

## Part 2 — UI/UX Design (2 minutes)

> **[Show the app running on the main page]**
>
> Let me start with UI and UX design. I used XAML for all the pages. The app has a bottom tab bar with three tabs — Foods, Hardware, and Settings — which makes navigation simple and intuitive.
>
> For the visual style, I used a warm colour palette with cream backgrounds, tomato red, baked orange, and basil green. This fits the food and drink theme. I used LinearGradientBrush on the headers and consistent rounded corners across all cards, so the look is unified.
>
> **[Scroll through the food list]**
>
> The main page uses a SearchBar at the top for searching, a CollectionView for the food cards, and a RefreshView so users can pull down to refresh. Each card shows the food name, calories, description, macros, and category. There's a details button on each card that navigates to a full nutrition page.
>
> **[Show the detail page]**
>
> The detail page shows everything about one food item — name, category, calories, protein, carbs, fat, description, and allergy notes.
>
> I also added a settings page where users can switch themes.
>
> **[Switch to Settings page]**
>
> I've got system theme, light theme, and dark theme options. Let me switch to dark mode.
>
> **[Switch to dark]**
>
> You can see all the colours change across the app. The gradients and card backgrounds all respond to the theme. This is done using AppThemeBinding in XAML.
>
> For performance, I kept things simple. The data is fetched once and cached locally. The search filters in memory rather than making network calls every time. I also reuse services — for example, SpeechService is used by both the detail page and the hardware page, so the speech logic isn't duplicated.

---

## Part 3 — Accessibility (1.5 minutes)

> Now accessibility. This is related to WCAG guidelines — Web Content Accessibility Guidelines. I applied three main principles.
>
> First, all interactive elements have SemanticProperties. For example, the search bar has a Hint that says "Search the food and drink list". The add button has a Hint. Every button across all pages has one. Screen readers use these hints to tell users what each control does.
>
> **[Show a few SemanticProperties in code if possible, or mention them while showing pages]**
>
> I also used HeadingLevel for semantic structure. The page title is Level1, section titles like "Nutrition overview" are Level2, and preview titles are Level3. This gives screen reader users a clear content hierarchy.
>
> Second, I added SemanticScreenReader.Announce calls in the code-behind. Whenever a status changes — like a photo being captured or a location being found — I call Announce so the screen reader reads it out automatically.
>
> **[Show Settings page]**
>
> Third, the large text mode. When I turn this on, all text across every page gets scaled up by about 22 percent. It doesn't just affect the settings page — when I switch to Foods or Hardware, the text stays large. This is because AccessibilityService.ApplyFontScale runs in OnAppearing on every page.
>
> **[Switch to another page to show large text persists]**
>
> I reference WCAG in my README as well. I focused on making content perceivable by screen readers, making controls operable with clear hints, and making the interface understandable with consistent navigation and heading levels.

---

## Part 4 — Functionality (2 minutes)

> Let me demonstrate the core functionality.
>
> **[Back on main page]**
>
> The main page shows a list of food and drink items. I can search — let me type "drink".
>
> **[Type "drink" in search]**
>
> It filters to show only the Iced Matcha Latte. The search matches against name, category, description, and tags, all case-insensitive. If I clear it, everything comes back.
>
> **[Clear search]**
>
> I can pull down to refresh the list.
>
> **[Pull down to refresh]**
>
> Let me open a detail page.
>
> **[Click Details on a card]**
>
> Here I can see all the nutrition information. There's a Read Summary button that uses text-to-speech to read the nutrition summary aloud. Let me press it.
>
> **[Press Read Summary — speech plays]**
>
> And Stop Reading to stop it.
>
> **[Press Stop Reading]**
>
> The Vibrate button triggers both vibration and haptic feedback as a meal reminder.
>
> **[Press Vibrate — alert pops up]**
>
> Now the add page. Click the Add button.
>
> **[Click Add, go to add page]**
>
> I can fill in name, pick a category, write a description, and enter the nutrition numbers. Let me add a new item.
>
> **[Fill in the form correctly]**
>
> Save. It says saved, and I'm back on the main page. The new item appears in the list.
>
> **[Show new item]**

---

## Part 5 — Validation and Error Handling (1.5 minutes)

> Now I'll show validation and error handling. Let me go back to the add page and try to save with empty fields.
>
> **[Go to Add page, leave everything blank, press Save]**
>
> You see the red validation panel appear — "Please enter a food or drink name." The phone also vibrates so the user feels the error. This is user-friendly messaging — not a programmer error like "NullReferenceException".
>
> Let me fill in just the name and leave category empty.
>
> **[Fill name only, press Save]**
>
> "Please choose a category."
>
> **[Select category, leave description empty, press Save]**
>
> "Please add a short description."
>
> Now let me put a negative number for calories.
>
> **[Enter -100 for calories, press Save]**
>
> "Please enter a valid non-negative number for calories." This checks all four numeric fields — calories, protein, carbs, and fat.
>
> For error handling in the hardware features, I wrapped every operation in try-catch blocks. Let me show you.
>
> **[Switch to Hardware page, maybe show code]**
>
> If the camera isn't supported on the device, it shows a clear message instead of crashing. Same for location — if permission is denied, it tells the user to enable it in settings. The accelerometer also checks IsSupported before starting. Every hardware feature has proper exception handling.
>
> The data layer also has fallback handling. If the mockapi.io network request fails, the app silently falls back to local data instead of showing an error or crashing.

---

## Part 6 — Mobile Hardware (3 minutes)

> Now the hardware features. I used five different hardware features in this app.
>
> **[On Hardware page]**
>
> First, the camera.
>
> **[Press Photo button]**
>
> This opens the device camera. I can take a photo and it appears on the page. The camera uses MediaPicker.CapturePhotoAsync. It handles permission denial gracefully.
>
> **[After photo is captured]**
>
> Second, location and geocoding.
>
> **[Press Locate button]**
>
> This gets the GPS coordinates — latitude and longitude — and then uses reverse geocoding to get the country, city, and region. The coordinates and address are both displayed. If geocoding doesn't return results, there's a fallback that maps coordinates to known areas.
>
> Third, the accelerometer. I added a shake-to-discover feature.
>
> **[Press Start on Shake to Discover]**
>
> Now the accelerometer is monitoring. When I shake the device, it detects the motion by calculating the acceleration delta across all three axes.
>
> **[Shake the device or simulate shake in emulator]**
>
> It picked a random food item and the counter went up to 1. There's a two-second cooldown so one shake doesn't trigger multiple times. Each shake gives haptic feedback and the recommendation changes.
>
> **[Shake again if possible]**
>
> I'll press Stop to turn it off.
>
> **[Press Stop]**
>
> Fourth, text-to-speech.
>
> **[Press Read help]**
>
> It reads the help text aloud. The SpeechService wraps TextToSpeech with a cancellation token, so I can stop it at any time.
>
> **[Press Stop speech]**
>
> And when I leave a page that's speaking, it stops automatically — I put Stop in OnDisappearing on every page.
>
> Fifth, vibration and haptic feedback.
>
> **[Press Haptic feedback]**
>
> This triggers both Vibration.Vibrate and HapticFeedback.Perform. The counter at the bottom increments so you can verify it works even if you can't feel it through the screencast.
>
> So in total I've used the camera, location with geocoding, accelerometer, text-to-speech, and vibration with haptic feedback — that's five hardware features directly integrated into the app.

---

## Part 7 — Code Quality (1 minute)

> Let me briefly show the code structure.
>
> **[Open Solution Explorer or show files in VS Code]**
>
> I separated the project into Models, Services, and Pages. FoodItem is the data model with JSON serialization attributes for the API. The Services folder has FoodCatalogService for data, SpeechService for text-to-speech, and AccessibilityService for font scaling. Each page has its XAML file for UI and a code-behind file for logic.
>
> **[Open FoodCatalogService.cs]**
>
> FoodCatalogService is reusable — both the main page and the hardware page call SearchAsync to get food data. The shake feature in HardwarePage also uses the same service to pick a random item. This is code reuse.
>
> I also have consistent naming — all page event handlers follow the On prefix pattern like OnSaveClicked, OnTakePhotoClicked. The code is organised logically with related methods grouped together.
>
> **[Open AndroidManifest.xml]**
>
> Here are the Android permissions — camera, coarse and fine location, and vibrate. I only request the permissions actually needed by the app.

---

## Part 8 — Deployment (30 seconds)

> For deployment, let me show the app running on Windows first — that's what you're seeing now.
>
> **[Show the Windows build output or the running app]**
>
> And I can also build and run it on Android.
>
> **[If possible, switch to Android emulator or show the build command output]**
>
> The app compiles for both platforms from the same codebase using .NET MAUI. I used Directory.Build.props to redirect the build output to avoid path issues on Windows. The Android manifest is configured with all the required permissions.

---

## Part 9 — GitHub Usage (30 seconds)

> Finally, GitHub. Here's my commit history.
>
> **[Open GitHub repository page, show commit history]**
>
> I committed regularly throughout development, not just at the end. I started with the initial project setup, then added the food list, hardware features, and gradually built up the app. Each commit has a descriptive message about what was changed.
>
> My README includes the app overview, author name, all the hardware features used, project structure, build instructions, and a development plan showing what's been completed and what's in progress.

---

## Part 10 — Conclusion (15 seconds)

> That covers all seven assessment criteria. NutriBite is a cross-platform food and drink app with XAML-based UI, five hardware features, WCAG-informed accessibility, form validation, error handling, and clean code structure. Thank you for watching.
