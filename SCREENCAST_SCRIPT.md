# NutriBite Screencast Script

About 9–10 minutes

---

## 1. Intro + UI/UX (1.5 min)

> Hi, I'm Wenhao Zhao. This is NutriBite, my final project for Mobile Computing. The theme is Food and Drink, built with .NET MAUI.
>
> **[Show main page]**
>
> Three tabs at the bottom — Foods, Hardware, Settings. The design uses warm food-themed colours — cream, tomato red, basil green — with consistent rounded cards and gradient headers, all defined in XAML with AppThemeBinding for light and dark mode.
>
> **[Switch to dark mode in Settings]**
>
> Everything switches together. The search bar, cards, all respond to the theme. Each card shows the food name, calories, macros, and a Details button.

---

## 2. Accessibility (1 min)

> For accessibility I followed WCAG principles. Every interactive control has SemanticProperties — Hints on buttons, HeadingLevels on titles. The screen reader announces status changes through SemanticScreenReader.Announce.
>
> **[Turn on large text in Settings, switch tabs]**
>
> Large text mode scales all text by 22% across every page. It persists when switching tabs because each page calls ApplyFontScale in OnAppearing.

---

## 3. Functionality (1.5 min)

> **[Search "drink", show filtering, clear search]**
>
> Search matches name, category, description, and tags.
>
> **[Open detail page, press Read Summary, then Stop]**
>
> Detail page shows all nutrition info. Read Summary uses text-to-speech. Speech stops automatically when leaving the page.
>
> **[Go to Add page, fill in form, save]**
>
> Add page lets you enter name, category, description, and nutrition numbers. Save it and it appears in the list.

---

## 4. Validation & Error Handling (1.5 min)

> **[Leave form empty, press Save]**
>
> Red error panel: "Please enter a food or drink name". Also vibrates. Friendly messages, not programmer errors.
>
> **[Show category required, then negative calories error]**
>
> All fields validated — required text fields and non-negative numbers for calories, protein, carbs, fat.
>
> For hardware, every operation is wrapped in try-catch. Camera checks IsCaptureSupported first. Location handles permission denial. Accelerometer checks IsSupported. Network failures fall back to local data silently instead of crashing.

---

## 5. Mobile Hardware (2.5 min)

> Five hardware features in total.
>
> **[Press Photo — take picture]**
>
> Camera via MediaPicker. Photo displayed on the page.
>
> **[Press Locate — show coordinates and address]**
>
> GPS coordinates plus reverse geocoding for country, city, region. Fallback mapping if geocoding is unavailable.
>
> **[Press Start on Shake to Discover, shake device]**
>
> Accelerometer detects shake by calculating delta across X, Y, Z axes. 1.2g threshold, two-second cooldown. Random food recommended with haptic feedback.
>
> **[Shake again, then Stop]**
>
> **[Press Read help, then Stop speech]**
>
> Text-to-speech with cancellation support. Auto-stops in OnDisappearing.
>
> **[Press Haptic feedback]**
>
> Vibration and haptic feedback. Counter increments for screencast verification.

---

## 6. Code Quality (1 min)

> **[Show project structure in Solution Explorer]**
>
> Models, Services, Pages — separated clearly. FoodCatalogService is reused by main page, detail page, and the shake feature. SpeechService shared between detail and hardware pages. Consistent naming — all handlers prefixed with On.
>
> **[Show AndroidManifest]**
>
> Four permissions only — camera, location coarse and fine, vibrate. Nothing unnecessary.

---

## 7. Deployment + GitHub (45 sec)

> **[Show Windows app running]**
>
> Compiles for both Android and Windows from one codebase.
>
> **[Show GitHub commit history]**
>
> Regular commits throughout development. README has app overview, hardware table, project structure, build instructions, and development plan.

---

## 8. Wrap-up (15 sec)

> That covers all seven criteria. NutriBite — cross-platform food and drink app with five hardware features, WCAG-informed accessibility, form validation, and clean code structure. Thank you.
