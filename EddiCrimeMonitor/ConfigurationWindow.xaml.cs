﻿using Eddi;
using EddiDataDefinitions;
using System;
using System.Threading;
using System.Windows;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace EddiCrimeMonitor
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ConfigurationWindow : UserControl
    {
        private CrimeMonitor crimeMonitor()
        {
            return (CrimeMonitor)EDDI.Instance.ObtainMonitor("Crime monitor");
        }

        public ConfigurationWindow()
        {
            InitializeComponent();

            criminalRecord.ItemsSource = crimeMonitor()?.criminalrecord;

            CrimeMonitorConfiguration configuration = CrimeMonitorConfiguration.FromFile();
            crimeProfitShareInt.Text = configuration.profitShare?.ToString(CultureInfo.InvariantCulture);
        }

        private void addRecord(object sender, RoutedEventArgs e)
        {
            FactionRecord record = new FactionRecord(Properties.CrimeMonitor.blank_faction);
            crimeMonitor()?.criminalrecord.Add(record);
            crimeMonitor()?.writeRecord();
        }

        private void removeRecord(object sender, RoutedEventArgs e)
        {
            FactionRecord record = (FactionRecord)((Button)e.Source).DataContext;
            crimeMonitor()?._RemoveRecord(record);
            crimeMonitor()?.writeRecord();
        }

        private void updateRecord(object sender, RoutedEventArgs e)
        {
            FactionRecord record = (FactionRecord)((Button)e.Source).DataContext;
            if (record.faction != Properties.CrimeMonitor.blank_faction)
            {
                Button updateButton = (Button)sender;
                updateButton.Foreground = Brushes.Red;
                updateButton.FontWeight = FontWeights.Bold;

                Thread factionStationThread = new Thread(() =>
                {
                    crimeMonitor()?.GetFactionData(record, record.system);
                    Dispatcher?.Invoke(() =>
                    {
                        updateButton.Foreground = Brushes.Black;
                        updateButton.FontWeight = FontWeights.Regular;
                    });
                })
                {
                    IsBackground = true
                };
                factionStationThread.Start();
                crimeMonitor()?.writeRecord();
            }
        }

        private void profitShareChanged(object sender, TextChangedEventArgs e)
        {
            CrimeMonitorConfiguration configuration = CrimeMonitorConfiguration.FromFile();
            try
            {
                int? profitShare = string.IsNullOrWhiteSpace(crimeProfitShareInt.Text) ? 0 : Convert.ToInt32(crimeProfitShareInt.Text, CultureInfo.InvariantCulture);
                crimeMonitor().profitShare = profitShare;
                configuration.profitShare = profitShare;
                configuration.ToFile();
            }
            catch
            {
                // Bad user input; ignore it
            }
        }

        private void EnsureValidInteger(object sender, TextCompositionEventArgs e)
        {
            // Match valid characters
            Regex regex = new Regex(@"[0-9]");
            // Swallow the character doesn't match the regex
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void criminalRecordUpdated(object sender, DataTransferEventArgs e)
        {
            // Update the crime monitor's information
            crimeMonitor()?.writeRecord();
        }
    }
}
