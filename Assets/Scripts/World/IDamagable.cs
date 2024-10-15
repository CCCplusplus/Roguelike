using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ChangeHP(float amount);

    void HandleDeath();

}
