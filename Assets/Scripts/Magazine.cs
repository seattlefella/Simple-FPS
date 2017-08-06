using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class Magazine : MonoBehaviour {

  

    }

    public interface IMagizine
    {
        Clip Reload(Clip clip);
        Munition GetMunition();

        Clip CreateClip();

        int DestroyClip(Clip clip);

    }

    // a data structure that holds a collection of munitions  
    public class Clip
    {
        public int Size;
        public int CurrentCount;
        public List<Munition> ActiveClip = new List<Munition>();
        public bool IsEmpty;
        public bool IsReloading;


    }
}
