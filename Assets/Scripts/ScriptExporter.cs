using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScriptExporter : MonoBehaviour {
    public NarrationItem firstItem;
    public string path = "Assets/narrative_export.csv";

    public void Export() {
        var lines = new List<NarrationItem>();
        var visited = new HashSet<NarrationItem>();

        // Traverse all unique paths
        Traverse(firstItem, visited, lines);

        // Export to CSV
        using (var writer = new StreamWriter(path)) {
            writer.WriteLine("location,context,source,target"); // header
            foreach (var item in lines) {
                // You can change this to output other fields as needed
                var phone = item.phone ? ", On Phone" : "";
                writer.WriteLine($"{item.name},\"Sc: {item.character}{phone}\",{item.line.Replace('\n',' ')},");
            }
        }
        Debug.Log($"Narrative exported to {path}");
    }

    private void Traverse(NarrationItem current, HashSet<NarrationItem> visited, List<NarrationItem> output) {
        if (current == null || visited.Contains(current)) return;

        output.Add(current);
        visited.Add(current);

        // If no next, stop
        if (current.next == null || current.next.Count == 0) return;

        // If all children lead to same node, traverse just once
        if (current.next.Count == 1) {
            Traverse(current.next[0].narrativeItem, visited, output);
            return;
        }

        // For branches, traverse each path separately, but after all branches, ensure 'merge' nodes are not duplicated
        foreach (var next in current.next) {
            // For each branch, create a new visited/output list to track that path
            var branchVisited = new HashSet<NarrationItem>(visited);
            var branchOutput = new List<NarrationItem>(output);
            Traverse(next.narrativeItem, branchVisited, branchOutput);

            // Merge results (avoiding duplicates)
            foreach (var item in branchOutput) {
                if (!output.Contains(item)) output.Add(item);
            }
        }
    }
}