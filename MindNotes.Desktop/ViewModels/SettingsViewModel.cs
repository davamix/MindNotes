using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using MindNotes.Core.Application;
using MindNotes.Core.Models;
using MindNotes.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Desktop.ViewModels;
public partial class SettingsViewModel : ObservableObject {
    private readonly IConfiguration _configuration;
    private readonly INetworkService _networkService;
    private readonly INotificationHub _notificationHub;
    [ObservableProperty]
    private Settings _settings = null;

    [ObservableProperty]
    public bool? _isOllamaConnected = null;

    [ObservableProperty]
    private bool? _isQdrantConnected = null;


    public SettingsViewModel(IConfiguration configuration,
        INetworkService networkService,
        INotificationHub notificationHub) {

        _configuration = configuration;
        _networkService = networkService;
        _notificationHub = notificationHub;

        LoadConfiguration();
    }

    private void LoadConfiguration() {
        Settings = new Settings {
            OllamaServerAddress = _configuration["AppSettings:Ollama:Server"] ?? string.Empty,
            QdrantServerAddress = _configuration["AppSettings:Qdrant:Server"] ?? string.Empty,
            EmbeddingModelName = _configuration["AppSettings:Ollama:EmbeddingModel"] ?? string.Empty,
            OutputVectorSize = _configuration["AppSettings:Ollama:OutputVectorSize"] ?? string.Empty
        };
    }

    [RelayCommand]
    private async void TestOllamaConnection(string address) {
        try {
            var isRunning = await _networkService.TestOllamaConnection();

            _notificationHub.Notify(new Notification() {
                Message = "Ollama connection test",
                Content = isRunning ? "Ollama server is running." : "Ollama server is not running.",
                Severity = isRunning ? NotificationSeverity.Success : NotificationSeverity.Error
            });

            IsOllamaConnected = isRunning;
        } catch (Exception ex) {
            _notificationHub.Notify(new Notification() {
                Message = "Ollama connection test failed",
                Content = ex.Message,
                Severity = NotificationSeverity.Error
            });

            IsOllamaConnected = false;
        }
    }

    [RelayCommand]
    private async void TestQdrantConnection(string address) {
        try {
            var isRunning = await _networkService.TestQdrantConnection();

            _notificationHub.Notify(new Notification() {
                Message = "Qdrant connection test",
                Content = isRunning ? "Qdrant server is running." : "Qdrant server is not running.",
                Severity = isRunning ? NotificationSeverity.Success : NotificationSeverity.Error
            });

            IsQdrantConnected = isRunning;
        } catch (Exception ex) {
            _notificationHub.Notify(new Notification() {
                Message = "Qdrant connection test failed",
                Content = ex.Message,
                Severity = NotificationSeverity.Error
            });

            IsQdrantConnected = false;
        }
    }

    [RelayCommand]
    private void SaveSettings() {
        _configuration["AppSettings:Ollama:Server"] = Settings.OllamaServerAddress;
        _configuration["AppSettings:Qdrant:Server"] = Settings.QdrantServerAddress;
        _configuration["AppSettings:Ollama:EmbeddingModel"] = Settings.EmbeddingModelName;
        _configuration["AppSettings:Ollama:OutputVectorSize"] = Settings.OutputVectorSize;
    }
}
