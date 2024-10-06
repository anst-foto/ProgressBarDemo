using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProgressBarDemo;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private CancellationTokenSource _cts;
    
    #region Properties

    private int? _begin;
    public int? Begin
    {
        get => _begin; 
        set => SetField(ref _begin, value);
    }
    
    private int? _end;
    public int? End
    {
        get => _end; 
        set => SetField(ref _end, value);
    }
    
    private int? _value;
    public int? Value
    {
        get => _value; 
        set => SetField(ref _value, value);
    }

    #endregion
    
    #region Commands

    public LambdaCommand CommandStart { get; }
    public LambdaCommand CommandStop { get; }

    #endregion
    
    #region Constructor

    public MainWindowViewModel()
    {
        CommandStart = new LambdaCommand( 
            execute: async _ => await DoWorkAsync(),
            canExecute: _ => Begin != null || End != null);
        
        CommandStop = new LambdaCommand( 
            execute: _ => _cts?.Cancel(),
            canExecute: _ => Begin != null || End != null);
    }
    
    #endregion

    #region Methods

    private async Task DoWorkAsync()
    {
        _cts = new CancellationTokenSource();
                
        var progress = new Progress<int>();
        progress.ProgressChanged += (_, e) => Value = e;
                
        await DemoService.DemoAsync(Begin!.Value, End!.Value, _cts.Token, progress);
    }

    #endregion
    
    #region INotifyPropertyChanged Members
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    #endregion
}