using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EddiDataDefinitions
{
    /// <summary>
    /// Details about a crew member
    /// </summary>
    public class Crew : INotifyPropertyChanged
    {
        private string _name;
        /// <summary>The crew member's name</summary>
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("name");
                }
            }
        }

        /// <summary>The crew member's ID</summary>
        public long crewId { get; set; }

        private string _faction;
        /// <summary>The crew member's faction</summary>
        public string faction
        {
            get
            {
                return _faction;
            }
            set
            {
                if (_faction != value)
                {
                    _faction = value;
                    NotifyPropertyChanged("faction");
                }
            }
        }

        /// <summary>The crew member's combat rating</summary>
        public string combatratingEDName
        {
            get => CombatRating.edname;
            set
            {
                CombatRating crDef = CombatRating.FromEDName(value);
                this.CombatRating = crDef;
            }
        }

        /// <summary>the role of this ship</summary>
        private CombatRating _CombatRating = CombatRating.FromRank(0);
        [JsonIgnore]
        public CombatRating CombatRating
        {
            get
            {
                return _CombatRating;
            }
            set
            {
                if (_CombatRating != value)
                {
                    _CombatRating = value;
                    NotifyPropertyChanged("combatrating");
                }
            }
        }

        // This string is made available for Cottle scripts that vary depending on the crew member's combat rating. 
        [JsonIgnore, Obsolete("Please use localizedName or invariantName")]
        public string combatrating => CombatRating?.localizedName;

        /// <summary>The crew member's profit share</summary>
        public int profitshare => CombatRating.profitshare;

        /// <summary>The crew member's status</summary>
        public bool active { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
