using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using MindNotes.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Desktop.ViewModels;
public partial class SettingsViewModel : ObservableObject {
    private readonly IConfiguration _configuration;

    [ObservableProperty]
    private Settings _settings = null;

    [ObservableProperty]
    public bool? _isOllamaConnected = null;

    [ObservableProperty]
    private bool? _isQdrantConnected = null;


    public SettingsViewModel(IConfiguration configuration) {
        _configuration = configuration;

        LoadConfiguration();
    }

    private void LoadConfiguration() {
        Settings = new Settings {
            OllamaServerAddress = _configuration["AppSettings:Ollama:Server"] ?? string.Empty,
            QdrantServerAddress = _configuration["AppSettings:Qdrant:Server"] ?? string.Empty
        };
    }

    [RelayCommand]
    private void TestOllamaConnection(string address) {
        Debug.WriteLine("Test Ollama connection command");
        IsOllamaConnected = true;

    }

    [RelayCommand]
    private void TestQdrantConnection(string address) {
        Debug.WriteLine("Test Qdrant connection command");
        IsQdrantConnected = false;
    }

    [RelayCommand]
    private void SaveSettings() {
        _configuration["AppSettings:Ollama:Server"] = Settings.OllamaServerAddress;
        _configuration["AppSettings:Qdrant:Server"] = Settings.QdrantServerAddress;
    }
}
