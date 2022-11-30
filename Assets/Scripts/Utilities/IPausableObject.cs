using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IPausableObject
{
    void Initialize();

    void OnPause();
    void OnResume();
}
