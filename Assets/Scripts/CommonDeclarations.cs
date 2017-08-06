using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CommonDeclarations
    {




    }

    public enum WeaponType : byte
    {
        Raycaster,
        Projectile
    }

    public enum MagazineType : byte
    {
        Single,
        Double
    }

    public enum Mix : byte
    {
        Uniform,
        Random,
        RoundRobin
    }
}