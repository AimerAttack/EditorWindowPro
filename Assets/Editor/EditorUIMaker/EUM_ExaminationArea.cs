using Amazing.Editor.Library;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_ExaminationArea
    {
        public int Depth = 0;
        public Rect Rect;
        
        protected virtual Color _Color
        {
            get => Color.blue;
        }

        protected float _Size = 2;
        
        public void Draw()
        {
            var oldColor = GUI.color;
            GUI.color = _Color;

            var tmpRect = new Rect(Rect);
            //need clip by EUM_Helper.ViewportRect
            tmpRect.xMin = Mathf.Max(tmpRect.xMin,EUM_Helper.Instance.ViewportRect.xMin);
            tmpRect.xMax = Mathf.Min(tmpRect.xMax,EUM_Helper.Instance.ViewportRect.xMax);
            tmpRect.yMin = Mathf.Max(tmpRect.yMin,EUM_Helper.Instance.ViewportRect.yMin);
            tmpRect.yMax = Mathf.Min(tmpRect.yMax,EUM_Helper.Instance.ViewportRect.yMax);
            
            //left
            if (Rect.xMin >= EUM_Helper.Instance.ViewportRect.xMin)
            {
                GUI.DrawTexture(new Rect(tmpRect.xMin,tmpRect.yMin,_Size,tmpRect.yMax - tmpRect.yMin), (Texture) EditorGUIUtility.whiteTexture);
            }
            
            //right
            if (Rect.xMax <= EUM_Helper.Instance.ViewportRect.xMax)
            {
                GUI.DrawTexture(new Rect(tmpRect.xMax,tmpRect.yMin,_Size,tmpRect.yMax - tmpRect.yMin), (Texture) EditorGUIUtility.whiteTexture);
            }
            
            //top
            if(Rect.yMin >= EUM_Helper.Instance.ViewportRect.yMin)
            {
                GUI.DrawTexture(new Rect(tmpRect.xMin, tmpRect.yMin,tmpRect.xMax - tmpRect.xMin,_Size), (Texture) EditorGUIUtility.whiteTexture);
            }
            
            //bottom
            if(Rect.yMax <= EUM_Helper.Instance.ViewportRect.yMax)
            {
                GUI.DrawTexture(new Rect(tmpRect.xMin, tmpRect.yMax,tmpRect.xMax - tmpRect.xMin,_Size), (Texture) EditorGUIUtility.whiteTexture);
            }
            
            GUI.color = oldColor;
        }
    }
}