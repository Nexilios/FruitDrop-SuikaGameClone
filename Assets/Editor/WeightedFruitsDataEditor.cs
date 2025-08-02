using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(WeightedFruitsData))]
    public class WeightedFruitsDataEditor : UnityEditor.Editor
    {
        private readonly string[] _fruitOrder = {
            "Cherry", "Strawberry", "Mandarin", "Orange", "Apple", 
            "Pear", "Peach", "Pineapple", "Melon", "Watermelon"
        };
    
        public override void OnInspectorGUI()
        {
            WeightedFruitsData weightedFruitsData = (WeightedFruitsData)target;
        
            DrawDefaultInspector();
        
            GUILayout.Space(10);
        
            if (GUILayout.Button("Auto-Populate Fruits"))
            {
                PopulateFruits(weightedFruitsData);
            }
        }
    
        private void PopulateFruits(WeightedFruitsData weightedFruitsData)
        {
            // Find all FruitData assets
            string[] guids = AssetDatabase.FindAssets("t:FruitData", new[] { "Assets/Fruits Data" });
        
            // Create a dictionary to store found fruits by name
            Dictionary<string, FruitData> foundFruits = new Dictionary<string, FruitData>();
        
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                FruitData fruitData = AssetDatabase.LoadAssetAtPath<FruitData>(path);
            
                if (fruitData)
                {
                    // Use the asset name (without extension) as the key
                    string fruitName = System.IO.Path.GetFileNameWithoutExtension(path);
                    foundFruits[fruitName] = fruitData;
                }
            }
        
            // Create ordered list based on fruitOrder array
            List<WeightedFruit> orderedFruits = new List<WeightedFruit>();
        
            foreach (string fruitName in _fruitOrder)
            {
                if (foundFruits.ContainsKey(fruitName))
                {
                    orderedFruits.Add(new WeightedFruit
                    {
                        fruitData = foundFruits[fruitName],
                        weight = 1f
                    });
                }
            }
        
            // Add any fruits that weren't in our predefined order (at the end)
            foreach (var kvp in foundFruits)
            {
                if (!_fruitOrder.Contains(kvp.Key))
                {
                    orderedFruits.Add(new WeightedFruit
                    {
                        fruitData = kvp.Value,
                        weight = 1f
                    });
                }
            }
        
            weightedFruitsData.fruits = orderedFruits.ToArray();
        
            EditorUtility.SetDirty(weightedFruitsData);
        }
    }
}