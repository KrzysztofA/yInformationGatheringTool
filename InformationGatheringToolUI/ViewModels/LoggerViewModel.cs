using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace InformationGatheringToolUI.ViewModels;

public class LoggerViewModel : ViewModelBase
{
  public ObservableCollection<LogEntry> Logs { get; } = [];

  public ICommand ClearCommand { get; }

  public LoggerViewModel()
  {
    ClearCommand = new RelayCommand( () => Logs.Clear() );
  }
}
