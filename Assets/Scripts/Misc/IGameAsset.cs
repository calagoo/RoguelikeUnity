using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameAsset
{
    int ID { get; }
    string Name { get; }
    Sprite Icon { get; }

    ProjectileData GetProjectileData();
}
