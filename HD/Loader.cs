using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HD
{
    public class Loader
    {
        public static void Load ()
        {
            Loader.MPDLL = new UnityEngine.GameObject();
            Loader.MPDLL.AddComponent<HD.Main>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.MPDLL);
        }
        public static void Unload()
        {
            GameObject.Destroy(MPDLL);
        }
        private static void _Unload()
        {

        }

        private static GameObject MPDLL;

    }
}
