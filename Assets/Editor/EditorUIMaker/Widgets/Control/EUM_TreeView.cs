using System.Collections.Generic;
using EditorUIMaker.Utility;
using Scriban;
using Scriban.Runtime;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_TreeView : EUM_Widget
    {
        private EUM_TreeView_Info info => Info as EUM_TreeView_Info;
        public override string TypeName => "TreeView";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_TreeView_Info(this);
            info.Label = TypeName;
            return info;
        }

        public override bool CanResize()
        {
            return false;
        }

        private SimpleTreeView _Treeview;

        private void InitIfNeed()
        {
            if (_Treeview == null)
            {
                _Treeview = SimpleTreeView.Create(info.Label,info.MinHeight);

                var datas = GenerateRandomTree(10);
                _Treeview.SetData(datas);
                _Treeview.ExpandAll();
            }
        }

        int IDCounter = 0;

        List<TreeViewItem> GenerateRandomTree(int numTotalElements)
        {
            int numRootChildren = numTotalElements / 4;
            IDCounter = 0;
            var treeElements = new List<TreeViewItem>(numTotalElements);

            var root = new TreeViewItem(IDCounter,1,"Root");

            for (int i = 0; i < numRootChildren; ++i)
            {
                int allowedDepth = 6;
                AddChildrenRecursive(root, Random.Range(5, 10), true, numTotalElements, ref allowedDepth,
                    treeElements);
            }

            return treeElements;
        }

        void AddChildrenRecursive(TreeViewItem element, int numChildren, bool force, int numTotalElements,
            ref int allowedDepth, List<TreeViewItem> treeElements)
        {
            if (element.depth >= allowedDepth)
            {
                allowedDepth = 0;
                return;
            }

            for (int i = 0; i < numChildren; ++i)
            {
                if (IDCounter > numTotalElements)
                    return;

                var child = new TreeViewItem( IDCounter++,element.depth + 1,"Element " + IDCounter);
                treeElements.Add(child);

                if (!force && Random.value < 0.5f)
                    continue;

                AddChildrenRecursive(child, Random.Range(5, 10), false, numTotalElements, ref allowedDepth,
                    treeElements);
            }
        }

        protected override void OnDrawLayout()
        {
            InitIfNeed();
            _Treeview.Content = info.Label;
            _Treeview.MinHeight = info.MinHeight;
            _Treeview.Draw();
        }

        public override string LogicCode()
        {
            var code =
                @"public void {{name}}SelectChange()
{
    CallMethod(""On{{name}}SelectChange"");
}";
            
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }

        public override string CodeForInit()
        {
            var code =
                @"Init{{name}}();";
            
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }

        public override string CodeForDefine()
        {
            var code =
                @"private SimpleTreeView _{{name}};
public SimpleTreeView {{name}} => _{{name}};
void Init{{name}}()
{
    if(_{{name}} != null)
        return;
    _{{name}} = SimpleTreeView.Create(""{{label}}"",{{minHeight}});
    _{{name}}.OnSelectionChanged += _Logic.{{name}}SelectChange;
    var expend = {{expend}};
    if(expend)
        _{{name}}.ExpandAll();
}";
            
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label",info.Label);
            sObj.Add("minHeight",info.MinHeight);
            sObj.Add("expend",info.ExpendAll);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }

        public override string Code()
        {
            var code =
                @"Init{{name}}();
_{{name}}.Content = ""{{label}}"";
_{{name}}.MinHeight = {{height}};
_{{name}}.Draw();";
            
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label",info.Label);
            sObj.Add("height",info.Height);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }
        
        SimpleTreeView _DraggingTreeview;

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x + 20, position.y, 200, 200), () =>
            {
                if (_DraggingTreeview == null)
                {
                    _DraggingTreeview = SimpleTreeView.Create(TypeName);

                    var datas = GenerateRandomTree(10);
                    _DraggingTreeview.SetData(datas);
                }
                _DraggingTreeview.Draw();
                _DraggingTreeview.ExpandAll();
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_TreeView();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}