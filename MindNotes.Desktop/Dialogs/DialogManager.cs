using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Desktop.Dialogs;

public interface IDialogManager {
    Task OpenBigNoteDialog();
}

public class DialogManager : IDialogManager {
    public DialogManager() {
    }
    public Task OpenBigNoteDialog() {
        //var dialog = Ioc.Default.GetService<BigNoteDialog>();

        //ThrowIfNull(dialog, typeof(BigNoteDialog));

        //DialogHost
        //ContentDialog contentDialog;
        return Task.CompletedTask;
    }



    private static void ThrowIfNull(object? dialog, Type dialogType) {
        if (dialog == null) throw new InvalidOperationException($"Cannot open {dialogType.FullName}");
    }
}
