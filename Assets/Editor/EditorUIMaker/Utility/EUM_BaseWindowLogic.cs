namespace EditorUIMaker
{
    public class EUM_BaseWindowLogic
    {
        protected void CallMethod(string methodName, params object[] args)
        {
            var method = GetType().GetMethod(methodName);
            if (method != null)
            {
                method.Invoke(this, args);
            }
        }
    }
}