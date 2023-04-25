﻿using UnityEngine;
using System;

namespace BoidSimulation.Data
{
    [Serializable]
    public class BoidsDataPreset
    {
        public Material PathsMaterial;
        public Material BoidMaterial;
        public Sprite BoidSprite;
        public int InstanceCount;
    }
}
