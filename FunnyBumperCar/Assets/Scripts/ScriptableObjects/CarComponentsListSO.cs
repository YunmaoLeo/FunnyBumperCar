using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/CarComponentListSO")]
public class CarComponentsListSO : ScriptableObject
{ 
    public List<Transform> CarBodysList;
    public List<Transform> LeftTiresList;
    public List<Transform> RightTiresList;
    public List<Transform> AddonsList;

    public List<Sprite> CarBodysSprites;
    public List<Sprite> LeftTireSprites;
    public List<Sprite> RightTireSprites;
    public List<Sprite> AddonsSprites;

}
