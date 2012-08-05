using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Common.Models.Doodads
{
    [DataContract]
    public class DoodadPlacement
    {
        [DataMember]
        public DoodadType DoodadType { get; set; }

        [DataMember]
        public Vector2 Position { get; set; }

        [DataMember]
        public float Rotation { get; set; }

        [DataMember]
        public Rectangle Source { get; set; }

        [DataMember]
        public Team Team { get; set; }

        [DataMember]
        public string WaypointColor { get; set; }
    }
}