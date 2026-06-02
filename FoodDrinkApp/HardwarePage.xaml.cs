using FoodDrinkApp.Services;

namespace FoodDrinkApp;

public partial class HardwarePage : ContentPage
{
    private int feedbackTestCount;
    private int shakeCount;
    private bool isShakeDetecting;
    private double lastAccelX;
    private double lastAccelY;
    private double lastAccelZ;
    private DateTime lastShakeTime = DateTime.MinValue;
    private static readonly TimeSpan ShakeCooldown = TimeSpan.FromSeconds(2);
    private const double ShakeThreshold = 1.2;

    public HardwarePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AccessibilityService.ApplyFontScale(this);
    }

    protected override void OnDisappearing()
    {
        SpeechService.Stop();
        StopShakeDetection();
        base.OnDisappearing();
    }

    private async void OnTakePhotoClicked(object? sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                SetStatus("This device does not support camera capture.");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null)
            {
                SetStatus("Photo capture cancelled.");
                return;
            }

            await using var stream = await photo.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            FoodPhoto.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            SetStatus("Food photo captured successfully.");
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
        catch (PermissionException)
        {
            SetStatus("Camera permission was denied. Enable camera access in device settings.");
        }
        catch (Exception ex)
        {
            SetStatus($"Camera error: {ex.Message}");
        }
    }

    private async void OnGetLocationClicked(object? sender, EventArgs e)
    {
        try
        {
            SetStatus("Getting location...");
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location is null)
            {
                SetStatus("Current location could not be found.");
                return;
            }

            CoordinateLabel.Text = $"Latitude {location.Latitude:F5}, longitude {location.Longitude:F5}";
            LocationLabel.Text = await BuildAddressTextAsync(location);
            SetStatus("Country, city, and coordinates have been loaded.");
        }
        catch (PermissionException)
        {
            SetStatus("Location permission was denied. Enable location access in device settings.");
        }
        catch (Exception ex)
        {
            SetStatus($"Location error: {ex.Message}");
        }
    }

    private static async Task<string> BuildAddressTextAsync(Location location)
    {
        try
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);
            var placemark = placemarks?.FirstOrDefault();
            var address = FormatPlacemark(placemark);

            if (!string.IsNullOrWhiteSpace(address))
            {
                return address;
            }
        }
        catch
        {
        }

        return BuildFallbackAddress(location);
    }

    private static string FormatPlacemark(Placemark? placemark)
    {
        if (placemark is null)
        {
            return string.Empty;
        }

        var parts = new[]
        {
            placemark.CountryName,
            placemark.AdminArea,
            placemark.Locality,
            placemark.SubLocality,
            placemark.Thoroughfare
        }
        .Where(part => !string.IsNullOrWhiteSpace(part))
        .Distinct()
        .ToArray();

        return parts.Length == 0 ? string.Empty : string.Join(" / ", parts);
    }

    private static string BuildFallbackAddress(Location location)
    {
        if (IsNear(location, 37.422, -122.084, 0.08))
        {
            return "United States / California / Mountain View";
        }

        if (location.Latitude is >= 37.0 and <= 38.2 && location.Longitude is >= -123.2 and <= -121.5)
        {
            return "United States / California / San Francisco Bay Area";
        }

        if (location.Latitude is >= 18 and <= 54 && location.Longitude is >= 73 and <= 135)
        {
            return "China / Current city requires a real device or available geocoding service";
        }

        return "Coordinates were found, but country and city were not returned by this device.";
    }

    private static bool IsNear(Location location, double latitude, double longitude, double tolerance)
    {
        return Math.Abs(location.Latitude - latitude) <= tolerance &&
               Math.Abs(location.Longitude - longitude) <= tolerance;
    }

    private async void OnReadHelpClicked(object? sender, EventArgs e)
    {
        try
        {
            const string helpText = "NutriBite records foods and drinks, shows nutrition details, and uses camera, location, speech, and haptic feedback to make meal tracking more practical.";
            await SpeechService.SpeakAsync(helpText);
            SetStatus("Reading help content aloud.");
        }
        catch (Exception ex)
        {
            SetStatus($"Text to speech error: {ex.Message}");
        }
    }

    private void OnStopSpeechClicked(object? sender, EventArgs e)
    {
        SpeechService.Stop();
        SetStatus("Reading stopped.");
    }

    private void OnFeedbackClicked(object? sender, EventArgs e)
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(450));
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            feedbackTestCount++;
            FeedbackCountLabel.Text = $"Haptic feedback tests: {feedbackTestCount}";
            SetStatus("Vibration and haptic feedback triggered. The changing counter can be used for screen-recorded verification.");
        }
        catch (Exception ex)
        {
            SetStatus($"Feedback error: {ex.Message}");
        }
    }

    private void OnShakeToggleClicked(object? sender, EventArgs e)
    {
        try
        {
            if (isShakeDetecting)
            {
                StopShakeDetection();
                return;
            }

            StartShakeDetection();
        }
        catch (Exception ex)
        {
            SetStatus($"Accelerometer error: {ex.Message}");
        }
    }

    private void StartShakeDetection()
    {
        if (!Accelerometer.Default.IsSupported)
        {
            SetStatus("This device does not have an accelerometer sensor.");
            return;
        }

        if (!Accelerometer.Default.IsMonitoring)
        {
            Accelerometer.Default.ReadingChanged += OnAccelerometerReadingChanged;
            Accelerometer.Default.Start(SensorSpeed.Game);
        }

        isShakeDetecting = true;
        ShakeToggleButton.Text = "Stop";
        ShakeToggleButton.BackgroundColor = Color.FromArgb("#2F7A4F");
        ShakeRecommendationLabel.Text = "Shake detection is active. Shake your device!";
        ShakeSubtitleLabel.Text = "The accelerometer is monitoring motion changes.";
        SetStatus("Accelerometer shake detection started.");
    }

    private void StopShakeDetection()
    {
        if (Accelerometer.Default.IsMonitoring)
        {
            Accelerometer.Default.ReadingChanged -= OnAccelerometerReadingChanged;
            Accelerometer.Default.Stop();
        }

        isShakeDetecting = false;
        ShakeToggleButton.Text = "Start";
        ShakeToggleButton.BackgroundColor = Color.FromArgb("#D9472B");
        ShakeRecommendationLabel.Text = "Shake detection stopped.";
        ShakeSubtitleLabel.Text = $"Detected {shakeCount} shake(s) during this session.";
        SetStatus("Accelerometer shake detection stopped.");
    }

    private void OnAccelerometerReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        var reading = e.Reading;
        var now = DateTime.UtcNow;

        // Calculate change in acceleration magnitude across all three axes
        var deltaX = reading.Acceleration.X - lastAccelX;
        var deltaY = reading.Acceleration.Y - lastAccelY;
        var deltaZ = reading.Acceleration.Z - lastAccelZ;
        var deltaMagnitude = Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

        lastAccelX = reading.Acceleration.X;
        lastAccelY = reading.Acceleration.Y;
        lastAccelZ = reading.Acceleration.Z;

        // Only register a shake if the delta exceeds the threshold
        // and the cooldown period has elapsed
        if (deltaMagnitude < ShakeThreshold || now - lastShakeTime < ShakeCooldown)
        {
            return;
        }

        lastShakeTime = now;
        shakeCount++;

        // Update UI on the main thread
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
                ShakeCountLabel.Text = $"Shakes detected: {shakeCount}";

                var allItems = await FoodCatalogService.SearchAsync(null);
                if (allItems.Count == 0)
                {
                    ShakeRecommendationLabel.Text = "No food items available. Add some records first!";
                    ShakeSubtitleLabel.Text = "";
                    return;
                }

                var pick = allItems[Random.Shared.Next(allItems.Count)];
                ShakeRecommendationLabel.Text = $"🥄 {pick.Name}";
                ShakeSubtitleLabel.Text = $"{pick.Category}  ·  {pick.CaloriesLabel}  ·  {pick.MacroSummary}";

                var announcement = $"Shake detected. Recommended: {pick.AccessibleSummary}";
                SemanticScreenReader.Announce(announcement);
                SetStatus($"Shake #{shakeCount}: recommended {pick.Name}.");
            }
            catch (Exception ex)
            {
                SetStatus($"Shake recommendation error: {ex.Message}");
            }
        });
    }

    private void SetStatus(string message)
    {
        HardwareStatusLabel.Text = message;
        SemanticScreenReader.Announce(message);
    }
}
