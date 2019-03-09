using Eddi;
using EddiDataDefinitions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;

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
            if (record.name != null || record.name != Properties.CrimeMonitor.blank_faction)
            {
                if (record.system != null)
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
                }
                else { record.station = null; }
                crimeMonitor()?.writeRecord();
            }
        }

        private void criminalRecordUpdated(object sender, DataTransferEventArgs e)
        {
            // Update the crime monitor's information
            crimeMonitor()?.writeRecord();
        }
    }
}
