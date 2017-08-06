using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class Magazine //: MonoBehaviour
    {
        private int size;
        public int CurrentCount;
        public bool IsEmpty;
        public bool IsReloading;

        // the data we are using to build new magazine clips
        private MagazineData currentMagazine;

        private Munition currentMunition;

        // The data structure used to hold the munitions set up as a LIFO store
        private Stack<Munition> currentClip = new Stack<Munition>();

        // As this is a non mono-behavior we need a constructor
        public Magazine(MagazineData _data)
        {
            // Let's keep a reference to the magazine operating parameters
            currentMagazine = _data;

            // initialize the class properties
            size = _data.MaxSize;
            IsEmpty = true;
            CurrentCount = 0;
            IsReloading = false;

            // Load the clip using the mix of munitions specified
            loadClip(currentClip);

        }

        public Munition GetMunition()
        {
            if (currentClip.Count > 0)
            {
                currentMunition = currentClip.Pop();
                CurrentCount = currentClip.Count;
                return currentMunition;
            }

            // The clip is empty so return empty handed
            IsEmpty = true;
            return null;
        }

        public void Reload()
        {
            // This is in a separate function to allow for loading effects and other
            loadClip(currentClip);
        }

        private Stack<Munition> loadClip(Stack<Munition> _clip)
        {

            // TODO:  add logic to change the mix of munitions as specified in the MagazineData
            // This is a simple load them all the same method.
            // Possible to use an algorithm design pattern
            for (var i = 0; i < size; i++)
            {
                _clip.Push(currentMagazine.Supported[0]);
            }

            IsReloading = false;
            CurrentCount = size;
            IsEmpty = false;

            return _clip;
        }

    }


}
