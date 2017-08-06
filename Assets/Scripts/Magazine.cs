using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class Magazine : MonoBehaviour
    {

        public Clip CurrentClip;

        // the data we are using to build new magazine clips
        public MagazineData CurrentMagazine;

        // A place to store loaded and empty clips
        private List<Clip> clips = new List<Clip>();

        public Clip CreateEmptyClip()
        {
            return new Clip(CurrentMagazine.MaxSize);
        }

        public Clip LoadClip(Clip _clip)
        {
            for (var i = 0; i < CurrentMagazine.MaxSize; i++)
            {
                _clip.Ammo.Add(CurrentMagazine.Supported[0]);
            }

            _clip.IsReloading = false;
            _clip.CurrentCount = CurrentMagazine.MaxSize;
            _clip.IsEmpty = false;

            return _clip;
        }

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
        public List<Munition> Ammo = new List<Munition>();
        public bool IsEmpty;
        public bool IsReloading;

       public Clip(int _size)
        {
            Size = _size;
            CurrentCount = 0;
            IsEmpty = true;
            IsReloading = false;

        }


    }
}
