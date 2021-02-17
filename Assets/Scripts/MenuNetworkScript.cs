using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror{


public class MenuNetworkScript : MonoBehaviour
{

    NetworkManager manager;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }


}
}