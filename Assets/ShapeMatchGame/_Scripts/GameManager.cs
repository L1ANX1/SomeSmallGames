using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeMatchGame
{
    public enum Mode { circle, box };

    public class GameManager
    {
        private GameManager()
        {
            //构造方法私有化,保证外部不能实例化

        }
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        public Mode CurrentMode { get; set; }

    }
}