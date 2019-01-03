using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum collectibleType { coin, redCoin };
    public collectibleType thisCollectibleType = collectibleType.coin;
}
