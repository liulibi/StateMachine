using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable 
{
    bool IsAggroed { get; set; }//仇恨

    bool IsWithinStrikingDistance {  get; set; }

    void SetAggroStatus(bool isAggroed);

    void SetStrikingDistanceBool(bool isWithinStrikingDistance);

}
