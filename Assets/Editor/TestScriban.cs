using Scriban;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TestScriban
    {
        [MenuItem("Tools/Do")]
        public static void Do()
        {
            Logic();
        }

        static void Logic()
        {
            var template = Template.Parse("Hello {{name}}!");
            var result = template.Render(new { name = "World" }); 
            
            Debug.Log(result);
        }
    }
}