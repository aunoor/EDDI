﻿using Newtonsoft.Json;

namespace EddiDataDefinitions
{
    /// <summary>
    /// Presence of a material
    /// </summary>
    public class MaterialPresence
    {
        [JsonIgnore]
        public Material definition { get; private set; }

        // We merged this with MaterialPercentage (which is now gone) but old scripts used different keys for the material's name so put them both here
        public string material { get; private set; }
        // ....but we prefer 'material' so ignore this for JSON
        [JsonIgnore]
        public string name => material;

        [JsonIgnore]
        public Rarity rarity { get; private set; }

        public decimal percentage { get; private set; }

        public MaterialPresence(Material definition, decimal percentage)
        {
            this.definition = definition;
            this.material = definition?.localizedName;
            this.rarity= definition?.rarity;
            this.percentage = percentage;
        }

        [JsonConstructor]
        public MaterialPresence(string material, decimal percentage)
            : this(Material.FromName(material), percentage)
        {}
    }
}
