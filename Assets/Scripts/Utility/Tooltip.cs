    // Tooltip.cs - Does NOT go in Editor folder
    using UnityEngine;
    using System.Collections;
     
    public class Tooltip : PropertyAttribute
    {
        public string EditorTooltip;
     
        public Tooltip(string EditorTooltip)
        {
            this.EditorTooltip = EditorTooltip;
        }
    }