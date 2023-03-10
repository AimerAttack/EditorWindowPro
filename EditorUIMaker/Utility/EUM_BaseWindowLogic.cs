using System.Reflection;

namespace EditorUIMaker
{
    public class EUM_BaseWindowLogic
    {
        protected void CallMethod(string methodName, params object[] args)
        {
            var method = GetType().GetMethod(methodName,BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null)
            {
                method.Invoke(this, args);
            }
        }
    }
}