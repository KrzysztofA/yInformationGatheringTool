using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace InformationGatheringToolUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
  private object? _currentView;
  public object? CurrentView
  {
    get => _currentView;
    set => SetProperty(ref _currentView, value);
  }

  public ICommand ShowLoggerCommand { get; }

  public MainWindowViewModel()
  {
    ShowLoggerCommand = new RelayCommand( () => CurrentView = new LoggerViewModel() );
  }

  public string Greeting { get; } = "Welcome to Avalonia!";
}
