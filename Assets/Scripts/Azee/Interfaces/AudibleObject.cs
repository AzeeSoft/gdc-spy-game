using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Azee.Interfaces
{
    public interface AudibleObject
    {
        Vector3? LocateFromNoise(GameObject targetObj);
    }
}
