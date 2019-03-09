using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EddiDataDefinitions
{
    public class CrimeReport
    {
        public DateTime timestamp { get; set; }

        public bool bounty { get; set; }

        public int shipId { get; set; }

        public string crimeEDName
        {
            get => crimeDef.edname;
            set
            {
                Crime cDef = Crime.FromEDName(value);
                this.crimeDef = cDef;
            }
        }

        // The crime description, localized
        [JsonIgnore]
        public string localizedCrime => crimeDef?.localizedName ?? null;

        // deprecated crime description (exposed to Cottle and VA)
        [JsonIgnore, Obsolete("Please use localizedCrime instead")]
        public string crime => localizedCrime;

        [JsonIgnore]
        public Crime crimeDef;

        public string system { get; set; }

        public string victim { get; set; }

        public decimal amount { get; set; }

        public CrimeReport() { }

        public CrimeReport(CrimeReport crimeReport)
        {
            bounty = crimeReport.bounty;
            shipId = crimeReport.shipId;
            crimeEDName = crimeReport.crimeEDName;
            system = crimeReport.system;
            victim = crimeReport.victim;
            amount = crimeReport.amount;
            timestamp = crimeReport.timestamp;
        }

        public CrimeReport(DateTime Timestamp, bool Bounty, int ShipId, Crime Crime, string System, decimal Amount)
        {
            timestamp = Timestamp;
            bounty = Bounty;
            shipId = ShipId;
            crimeDef = Crime;
            system = System;
            amount = Amount;
        }
    }
}
