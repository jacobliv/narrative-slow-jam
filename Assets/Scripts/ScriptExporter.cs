using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScriptExporter : MonoBehaviour {
    public NarrationItem firstItem;
    public string path = "Assets/narrative_export.csv";
    public string characterPath = "Assets/character_titles.csv";

    [ContextMenu("Export Narrative To CSV")]
    public void Export() {
        var lines = new List<NarrationItem>();
        var visited = new HashSet<string>();
        Dictionary<string,string> characterTitles = new ();
        // Traverse all unique paths
        Traverse(firstItem, lines, visited);

        // Export to CSV
        using (var writer = new StreamWriter(path)) {
            writer.WriteLine("location,context,source,target"); // header
            foreach (var item in lines) {
                // You can change this to output other fields as needed
                var speakingCharacter= item.character? "Sc: " + item.character.name : "No Sc";
                if (item.character != null) {
                    characterTitles.TryAdd(item.character.name,item.character.title);

                }
                var phone = item.phone ? ", on phone" : "";
                item.next?.Reverse();
                var nextNames = (item.next != null && item.next.Count > 0)
                    ? string.Join(",", item.next
                                      .Where(n => n != null && n.narrativeItem != null)
                                      .Select(n => n.narrativeItem.name.Replace("[","").Replace("]","")))
                    : "";

                var nextLine = string.IsNullOrEmpty(nextNames) ? "" : $", Next: [{nextNames}]";
                var music = ", no music";

                if (item.day is Day.Pre or Day.One) {
                    music=$", msc: {Songs.SupernovaAlt}";
                } else if (item.day is Day.Two or Day.Three) {
                    music=$", msc: {Songs.Supernova}";
                } else if (item.day == Day.Post) {
                    music=$", msc: {Songs.BracingForImpact}";
                }

                var sfx = item.sounds.Count ==0 ? ", no sfx" :", sfx: ";
                sfx += string.Join(",", item.sounds.Select(s => s ? s.name : ""));
                var itemName = item.name.Replace("[","").Replace("]","");
                writer.WriteLine($"{itemName},\"{speakingCharacter}{phone}{music}{sfx}{nextLine}\",\"{item.line.Replace('\n',' ').Replace('\"','\'')}\",");
                if (item.next is { Count: > 1 }) {
                    int i = 0;
                    Debug.Log("Next Item");
                    foreach (var next in item.next) {
                        writer.WriteLine($"{itemName}-Option-{i+1},\"{speakingCharacter}{phone}{music}{sfx}{nextLine}\",\"{next.shortenedLine.Replace('\n',' ').Replace('\"','\'')}\",");
                        i++;
                    }
                }
            }
        }
        // write character titles to another csv
        using (var writer = new StreamWriter(characterPath)) {
            writer.WriteLine("character name, title"); // header
            foreach (var keyValuePair in characterTitles) {
                writer.WriteLine($"{keyValuePair.Key},{keyValuePair.Value}");
            }
        }

        Debug.Log($"Narrative exported to {path}");
    }
    
// a -> b
// b -> c
// c -> d1, d2
// d1 -> d1a
// d1b -> e
// d2 -> d2a
// d2b -> e
// e -> f
// f -> g

// out: a, b, c, d1, d1a, d1b, e, f, g
// iter = 2
// branch out: d2, d2a,d2b
    private void Traverse(NarrationItem current, List<NarrationItem> output, HashSet<string> visited) {
        if (current == null || visited.Contains(current.name)) return;
        
        output.Add(current);
        visited.Add(current.name);
        // If no next, stop
        if (current.next == null || current.next.Count == 0) return;

        // If all children lead to same node, traverse just once
        if (current.next.Count == 1) {
            Traverse(current.next[0].narrativeItem, output, visited);
            return;
        }
        
        int iter = output.Count;
        int i = 0;
        foreach (var next in current.next) {
            if (i == 0) {
                Traverse(next.narrativeItem, output, visited);
            }
            else {
                List<NarrationItem> branchOutput = new();
                Traverse(next.narrativeItem, branchOutput, visited);
                output.InsertRange(iter,branchOutput);
                branchOutput.Clear();
            }
            
            i++;
            
            

        }

    }


}