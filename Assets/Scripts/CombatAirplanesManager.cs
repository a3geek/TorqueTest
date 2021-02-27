using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAirplanesManager : MonoBehaviour
{
    public static CombatAirplanesManager Instance = null;

    [SerializeField]
    private CombatAirplane prefab = null;
    [SerializeField]
    private int num = 100;
    [SerializeField]
    private float offset = 100f;
    [SerializeField]
    private float range = 25f;

    private List<CombatAirplane> redGroup = new List<CombatAirplane>();
    private List<CombatAirplane> blueGroup = new List<CombatAirplane>();


    private void Awake()
    {
        Instance = this;

        this.CreateComatAirplanes("Red", Color.red, this.redGroup, this.offset, this.range);
        this.CreateComatAirplanes("Blue", Color.blue, this.blueGroup, -this.offset, this.range);
    }

    public CombatAirplane GetClosestCombatAirplane(CombatAirplane airplane)
    {
        var group = airplane.GroupName == "Red" ? this.blueGroup : this.redGroup;
        var pos = airplane.transform.position;
        var closest = group[0];

        for(var i = 1; i < group.Count; i++)
        {
            var diff1 = closest.transform.position - pos;
            var diff2 = group[i].transform.position - pos;

            if(diff1.sqrMagnitude > diff2.sqrMagnitude)
            {
                closest = group[i];
            }
        }

        return closest;
    }

    private void CreateComatAirplanes(string groupName, Color color, List<CombatAirplane> group, float offset, float range)
    {
        var parent = new GameObject(groupName);
        parent.transform.parent = this.transform;

        for(var i = 0; i < this.num; i++)
        {
            var randX = Random.Range(-range, range);
            var randY = Random.Range(-range, range);
            var randZ = Random.Range(-range, range);
            var airplane = Instantiate(
                this.prefab, new Vector3(offset + randX, randY, randZ),
                Quaternion.identity, parent.transform
            );

            foreach(var render in airplane.GetComponentsInChildren<Renderer>())
            {
                render.material.color = color;
            }
            
            airplane.GroupName = groupName;
            group.Add(airplane);
        }
    }
}
