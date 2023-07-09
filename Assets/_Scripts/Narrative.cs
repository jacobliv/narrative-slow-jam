using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Narrative", menuName = "Narrative/Narrative")]
public class Narrative : ScriptableObject {
    public List<NarrationItem> narrative;
}
