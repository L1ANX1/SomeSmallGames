using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeMatchGame
{
    public enum Mode { circle, box };

    public class GameManager
    {
        public Mode currentMode { get; set; }

    }
}