using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace QuestSystem.QuestEditor
{
    public class QuestGraphView : GraphView
    {
        public string misionName;

        private QuestNodeSearchWindow _searchWindow;
        public Quest questRef;
        private QuestGraphView _self;
        private QuestGraphEditor editorWindow;


        public QuestGraphView(EditorWindow _editorWindow, Quest q = null)
        {
            questRef = q;
            editorWindow = (QuestGraphEditor)_editorWindow;
            styleSheets.Add(Resources.Load<StyleSheet>("QuestGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            //Grid
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            this.AddElement(GenerateEntryPointNode());
            this.AddSearchWindow(editorWindow);
            _self = this;

        }


        //TODO: Create node at desired position with fewer hide
        /*public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            if (evt.target is GraphView)
            {
                evt.menu.InsertAction(1,"Create Node", (e) => {
                    

                    var a = editorWindow.rootVisualElement; var b = editorWindow.position.position; var c = editorWindow.rootVisualElement.parent;

                    var context = new SearchWindowContext(e.eventInfo.mousePosition, a.worldBound.x, a.worldBound.y);
                    Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement, context.screenMousePosition - editorWindow.position.position);
                    Vector2 graphViewMousePosition = this.contentViewContainer.WorldToLocal(mousePosition);

                    CreateNode("NodeQuest", mousePosition);
                });
            }
        }*/

        private void AddSearchWindow(EditorWindow editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<QuestNodeSearchWindow>();
            _searchWindow.Init(this, editorWindow);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition),_searchWindow);
        }

        private Port GeneratePort(NodeQuestGraph node, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(float));
        }


        public NodeQuestGraph GenerateEntryPointNode()
        {
            var node = new NodeQuestGraph
            {
                title = "Start",
                GUID = Guid.NewGuid().ToString(),
                entryPoint = true
            };

            //Add ouput port
            var generatetPort = GeneratePort(node, Direction.Output);
            generatetPort.portName = "Next";
            node.outputContainer.Add(generatetPort);

            //Quest params
            var box = new Box();

            //
            var misionName = new TextField("Mision Name:")
            {
                value = "Temp name"
            };

            misionName.RegisterValueChangedCallback(evt =>
            {
                node.misionName = evt.newValue;
            });

            box.Add(misionName);

            //
            var isMain = new Toggle();
            isMain.label = "isMain";

            isMain.value = false;

            isMain.RegisterValueChangedCallback(evt =>
            {
                node.isMain = evt.newValue;
            });

            //isMain.SetValueWithoutNotify(false);

            box.Add(isMain);

            //
            var startDay = new IntegerField("Start Day:")
            {
                value = 0
            };

            startDay.RegisterValueChangedCallback(evt =>
            {
                node.startDay = evt.newValue;
            });

            box.Add(startDay);

            //
            var limitDay = new IntegerField("Limit Day:")
            {
                value = 0
            };

            limitDay.RegisterValueChangedCallback(evt =>
            {
                node.limitDay = evt.newValue;
            });

            box.Add(limitDay);

            node.mainContainer.Add(box);


            //Refresh visual part
            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(100, 200, 100, 150));

            return node;

        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            //Reglas de conexions
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public void CreateNode(string nodeName, Vector2 position)
        {
            AddElement(CreateNodeQuest(nodeName,position));
        }

        public NodeQuestGraph CreateNodeQuest(string nodeName, Vector2 position, TextAsset ta = null, bool end = false)
        {
            var node = new NodeQuestGraph
            {
                title = nodeName,
                GUID = Guid.NewGuid().ToString(),
                questObjectives = new List<QuestObjectiveGraph>(),
            };

            //Add Input port
            var generatetPortIn = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
            generatetPortIn.portName = "Input";
            node.inputContainer.Add(generatetPortIn);

            node.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            //Add button to add ouput
            var button = new Button(clickEvent: () =>
            {
                AddNextNodePort(node);
            });
            button.text = "New Next Node";
            node.titleContainer.Add(button);

            //Button to add more objectives
            var button2 = new Button(clickEvent: () =>
            {
                AddNextQuestObjective(node);
            });
            button2.text = "Add new Objective";

            //Hide/Unhide elements
            var hideButton = new Button(clickEvent: () =>
            {
                HideUnhide(node, button2);
            });
            hideButton.text = "Hide/Unhide";




            //Extra information
            var extraText = new ObjectField("Extra information:");
            extraText.objectType = typeof(TextAsset);

            extraText.RegisterValueChangedCallback(evt =>
            {
                node.extraText = evt.newValue as TextAsset;
            });
            extraText.SetValueWithoutNotify(ta);

            //Bool es final
            var togle = new Toggle();
            togle.label = "isFinal";

            togle.RegisterValueChangedCallback(evt =>
            {
                node.isFinal = evt.newValue;
            });
            togle.SetValueWithoutNotify(end);

            var container = new Box();
            node.mainContainer.Add(container);// Container per a tenir fons solid

            container.Add(extraText);
            container.Add(togle);
            container.Add(hideButton);
            container.Add(button2);

            node.objectivesRef = new Box();
            container.Add(node.objectivesRef);

            //Refresh la part Visual
            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(position.x, position.y, 400, 450));

            return node;
        }

        private void HideUnhide(NodeQuestGraph node, Button b)
        {
            bool show = !b.visible;
            b.visible = show;

            foreach (var objective in node.questObjectives)
            {
                if (show)
                {
                    node.objectivesRef.Add(objective);
                }
                else
                {
                    node.objectivesRef.Remove(objective);
                }
            }

            node.RefreshExpandedState();
            node.RefreshPorts();
        }

        public void AddNextNodePort(NodeQuestGraph node, string overrideName = "")
        {
            var generatetPort = GeneratePort(node, Direction.Output);
            int nPorts = node.outputContainer.Query("connector").ToList().Count;

            //generatetPort.portName = "NextNode " + nPorts;

            string choicePortName = string.IsNullOrEmpty(overrideName) ? "NextNode " + nPorts : overrideName;
            generatetPort.portName = choicePortName;

            var deleteButton = new Button(clickEvent: () => RemovePort(node, generatetPort))
            {
                text = "x"
            };
            generatetPort.contentContainer.Add(deleteButton);

            node.outputContainer.Add(generatetPort);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        private void RemovePort(NodeQuestGraph node, Port p)
        {
            var targetEdge = edges.ToList().Where(x => x.output.portName == p.portName && x.output.node == p.node);
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }

            node.outputContainer.Remove(p);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        public void removeQuestObjective(NodeQuestGraph nodes, QuestObjectiveGraph objective)
        {
            nodes.objectivesRef.Remove(objective);
            nodes.questObjectives.Remove(objective);
            nodes.RefreshExpandedState();
        }

        private void AddNextQuestObjective(NodeQuestGraph node)
        {
            var Q = new QuestObjectiveGraph();

            var deleteButton = new Button(clickEvent: () => removeQuestObjective(node, Q))
            {
                text = "x"
            };
            Q.contentContainer.Add(deleteButton);

            //Visual Box separator
            var newBox = new Box();
            Q.Add(newBox);

            node.objectivesRef.Add(Q);
            node.questObjectives.Add(Q);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        public NodeQuestGraph GetEntryPointNode()
        {
            List<NodeQuestGraph> nodeList = this.nodes.ToList().Cast<NodeQuestGraph>().ToList();
            return nodeList.First(node => node.entryPoint);
        }



    }
}