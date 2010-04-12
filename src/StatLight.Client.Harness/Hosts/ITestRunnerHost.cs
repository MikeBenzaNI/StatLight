﻿using System.Windows;
using StatLight.Core.WebServer;

namespace StatLight.Client.Harness.Hosts
{
    public interface ITestRunnerHost
    {
        void ConfigureWithClientTestRunConfiguration(ClientTestRunConfiguration clientTestRunConfiguration);
        void ConfigureWithLoadedXapData(LoadedXapData loadedXapData);
        UIElement StartRun();
    }
}