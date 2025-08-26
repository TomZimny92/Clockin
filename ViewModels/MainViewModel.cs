using Clockin.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Clockin.Services;
using System.Text.Json.Serialization;

namespace Clockin.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private const string LastTabUsedKey = "LastTabUsedKey";

        private bool _isCheckedIn;
        public bool IsCheckedIn
        {
            get => _isCheckedIn;
            set
            {
                if (SetProperty(ref _isCheckedIn, value))
                {
                    UpdateCommandStates();
                }
            }
        }

        private ObservableCollection<TimeEntry>? _timeEntries;
        public ObservableCollection<TimeEntry>? TimeEntries
        {
            get => _timeEntries;
            set => SetProperty(ref _timeEntries, value);
        }

        private string? _currentTime;
        public string? CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }

        private string? _totalElapsedTime;
        public string? TotalElapsedTime
        {
            get => _totalElapsedTime;
            set => SetProperty(ref _totalElapsedTime, value);
        }

        private TabContext? _currentTab;
        public TabContext? CurrentTab
        {
            get => _currentTab;
            set => SetProperty(ref _currentTab, value);
        }

        public ICommand ClockinCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand ShowSummaryCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand PreferencesCommand { get; }

        private IDispatcherTimer? _clockTimer;

        public MainViewModel()
        {
            ClockinCommand = new AsyncRelayCommand(ExecuteClockin, CanExecuteClockin);
            CheckoutCommand = new AsyncRelayCommand(ExecuteCheckout, CanExecuteCheckout);
            ShowSummaryCommand = new AsyncRelayCommand(ExecuteShowResult);
            ResetCommand = new AsyncRelayCommand(ExecuteReset);
            PreferencesCommand = new AsyncRelayCommand(ExecutePreferences);

            _ = InitializeData();
            SetupClock();
        }

        private async Task InitializeData()
        {
            try
            {
                // get last tab used
                string? lastTabRaw = await SecureStorage.Default.GetAsync(LastTabUsedKey);
                if (!string.IsNullOrEmpty(lastTabRaw))
                {
                    string? currentTabContextRaw = await SecureStorage.Default.GetAsync(lastTabRaw);
                    if (!string.IsNullOrEmpty(currentTabContextRaw))
                    {
                        CurrentTab = JsonSerializer.Deserialize<TabContext>(currentTabContextRaw);                        
                    }
                    else
                    {
                        CurrentTab = TabContext.CreateNewTabData();                        
                    }
                }
                else
                {
                    CurrentTab = TabContext.CreateNewTabData();
                }

                if (CurrentTab != null)
                {
                    IsCheckedIn = CurrentTab.IsCheckedIn;
                    TimeEntries = CurrentTab.TimeEntries;
                    CalculateElapsedTime();
                }
                else
                {
                    IsCheckedIn = false;
                    TimeEntries = [];
                    TotalElapsedTime = "00:00:00";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"some of the data failed to load: {ex}");
            }
            finally
            {
                UpdateCommandStates();
            }
        }

        private void SetupClock()
        {
            // probably need to have Timer dependently injected
            _clockTimer = App.Current?.Dispatcher.CreateTimer();
            if (_clockTimer != null)
            {
                _clockTimer.Interval = TimeSpan.FromSeconds(1);
                _clockTimer.Tick += (s, e) =>
                {
                    CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                    // Update summary continuously if checked in
                    if (IsCheckedIn)
                    {
                        CalculateElapsedTime();
                    }
                };
                _clockTimer.Start();
            }
        }

        private async Task ExecuteClockin()
        {
            IsCheckedIn = true;
            if (TimeEntries != null)
            {
                TimeEntries.Add(new TimeEntry { ClockinTime = DateTime.Now, CheckoutTime = null });
                UpdateCommandStates();
                await SaveStateAsync();
            }
            else
            {
                // initialize TimeEntries with default, non-null value?
                Console.WriteLine("ExecuteClockin blew up");
            }
        }

        private bool CanExecuteClockin()
        {
            return !IsCheckedIn;
        }

        private async Task ExecuteCheckout()
        {
            IsCheckedIn = false;
            DateTime checkoutTime = DateTime.Now;

            // Find the last open check-in entry and update it
            if (TimeEntries != null)
            {
                var lastEntry = TimeEntries.LastOrDefault(e => !e.CheckoutTime.HasValue);
                if (lastEntry != null)
                {
                    lastEntry.CheckoutTime = checkoutTime;
                }
                else
                {
                    // Fallback: If somehow checkout is clicked without a check-in,
                    // add a placeholder entry.
                    // I don't like this. You're probably using it wrong. Here, let me show you.
                    TimeEntries.Add(new TimeEntry { ClockinTime = DateTime.MinValue, CheckoutTime = checkoutTime });
                }

                UpdateCommandStates();
                CalculateElapsedTime(); // Update summary immediately after checkout
                await SaveStateAsync();
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        private bool CanExecuteCheckout()
        {
            return IsCheckedIn;
        }

        private void CalculateElapsedTime()
        {
            TimeSpan total = TimeSpan.Zero;
            if (TimeEntries != null)
            {
                foreach (var entry in TimeEntries)
                {
                    if (entry.CheckoutTime.HasValue)
                    {
                        total += entry.Duration;
                    }
                    else if (IsCheckedIn && entry == TimeEntries.LastOrDefault(e => !e.CheckoutTime.HasValue))
                    {
                        // If currently checked in, add duration from check-in to now for the active session
                        total += (DateTime.Now - entry.ClockinTime);
                    }
                }
                TotalElapsedTime = $"{total:hh\\:mm\\:ss}";
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        private async Task ExecuteShowResult()
        {
            try
            {
                if (TotalElapsedTime != null && TimeEntries != null)
                {
                    var summaryPage = new SummaryPage();
                    var summaryViewModel = new SummaryViewModel(TotalElapsedTime, TimeEntries);
                    summaryPage.BindingContext = summaryViewModel;

                    if (Application.Current != null)
                    {
                        await Application.Current.Windows[0].Navigation.PushModalAsync(summaryPage);
                    }
                }
                else
                {
                        App.Current?.Windows[0]?.Page?.DisplayAlert("Summary", "Elapsed Time or Time Entries not found. There's nothing to report.", "OK");                                        
                }
                // DoTheMath(); moving this to SummaryPage. Don't need it here
                // extract the dates from the timeentries to display as sheet
                // might have to do that in the modal
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }



        private async Task ExecuteReset()
        {
            if (TimeEntries != null)
            {
                TimeEntries.Clear();
                IsCheckedIn = false;
                TotalElapsedTime = "00:00:00";
                UpdateCommandStates();
                await SaveStateAsync();
                App.Current?.Windows[0]?.Page?.DisplayAlert("Reset", "All time entries have been cleared.", "OK");
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        private async Task ExecutePreferences()
        {
            // show the modal
            try
            {
                var preferencesPage = new PreferencesPage();
                var preferencesViewModel = new PreferencesViewModel();
                preferencesPage.BindingContext = preferencesViewModel;
                if (Application.Current != null)
                {
                    await Application.Current.Windows[0].Navigation.PushModalAsync(preferencesPage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading hourly rate: {ex.Message}");
            }
        }

        private async Task SaveStateAsync()
        {
            try
            {
                if (CurrentTab  != null)
                {
                    CurrentTab.IsCheckedIn = IsCheckedIn;
                    CurrentTab.TimeEntries = TimeEntries;
                    await SecureStorage.Default.SetAsync(LastTabUsedKey, CurrentTab.Id.ToString());
                    await SecureStorage.Default.SetAsync(CurrentTab.Id.ToString(), JsonSerializer.Serialize(CurrentTab.TimeEntries));
                }
                else
                {
                    Console.WriteLine("somehow CurrentTab is null");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void UpdateCommandStates()
        {
            ((AsyncRelayCommand)ClockinCommand).NotifyCanExecuteChanged();
            ((AsyncRelayCommand)CheckoutCommand).NotifyCanExecuteChanged();
        }
    }
}
