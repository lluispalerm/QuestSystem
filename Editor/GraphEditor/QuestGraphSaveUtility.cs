using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.Windows;
using System;

namespace QuestSystem.QuestEditor
{
    public class QuestGraphSaveUtility
    {
        private QuestGraphView _targetGraphView;

        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<NodeQuestGraph> node => _targetGraphView.nodes.ToList().Cast<NodeQuestGraph>().ToList();

        private List<NodeQuest> _cacheNodes = new List<NodeQuest>();

        public static QuestGraphSaveUtility GetInstance(QuestGraphView targetGraphView)
        {
            return new QuestGraphSaveUtility
            {
                _targetGraphView = targetGraphView,
            };
        }

        private void creteNodeQuestAssets(Quest Q, ref List<NodeQuest> NodesInGraph)
        {
            int j = 0;
            CheckFolders(Q);

            string path = QuestConstants.MISIONS_FOLDER + $"/{Q.misionName}/Nodes";
            string tempPath = QuestConstants.MISIONS_FOLDER + $"/{Q.misionName}/Temp";
            //Move all nodes OUT to temp
            if (AssetDatabase.IsValidFolder(path)) {
                AssetDatabase.CreateFolder(QuestConstants.MISIONS_FOLDER + $"{Q.misionName}", "Temp");

                var debug = AssetDatabase.MoveAsset(path, tempPath);
            }


            Debug.Log("GUID: " + AssetDatabase.CreateFolder(QuestConstants.MISIONS_FOLDER + $"/{Q.misionName}", "Nodes"));
            
            //Order by position 
            List<NodeQuestGraph> nodeList = node.Where(node => !node.entryPoint).ToList();

            foreach (var nodequest in nodeList)
            {
                //Visual part
                string nodeSaveName = Q.misionName + "_Node" + j;
                NodeQuest saveNode; 

                //Si existe en temps
                bool alredyExists = false;
                if (alredyExists = !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(tempPath + "/" + nodeSaveName + ".asset")))
                {
                    saveNode = AssetDatabase.LoadAssetAtPath<NodeQuest>(tempPath + "/" + nodeSaveName + ".asset");
                }
                else
                {
                    saveNode = ScriptableObject.CreateInstance<NodeQuest>();
                }

                saveNode.GUID = nodequest.GUID;
                saveNode.position = nodequest.GetPosition().position;

                //Quest Part
                saveNode.isFinal = nodequest.isFinal;
                saveNode.extraText = nodequest.extraText;
                saveNode.nodeObjectives = createObjectivesFromGraph(nodequest.questObjectives);

                if(!alredyExists)
                    AssetDatabase.CreateAsset(saveNode, $"{QuestConstants.MISIONS_FOLDER}/{Q.misionName}/Nodes/{nodeSaveName}.asset");
                else
                {
                    AssetDatabase.MoveAsset(tempPath + "/" + nodeSaveName + ".asset", path + "/" + nodeSaveName + ".asset");
                }

                EditorUtility.SetDirty(saveNode);
                AssetDatabase.SaveAssets(); 

                NodesInGraph.Add(saveNode);
                j++;
            }

            AssetDatabase.DeleteAsset(tempPath);

        }

        public void CheckFolders(Quest Q)
        {
            if (!AssetDatabase.IsValidFolder(QuestConstants.RESOURCES_PATH))
            {
                AssetDatabase.CreateFolder(QuestConstants.PARENT_PATH, QuestConstants.RESOURCES_NAME);
            }

            if (!AssetDatabase.IsValidFolder(QuestConstants.MISIONS_FOLDER))
            {
                AssetDatabase.CreateFolder(QuestConstants.RESOURCES_PATH, QuestConstants.MISIONS_NAME);
            }

            if (!AssetDatabase.IsValidFolder(QuestConstants.MISIONS_FOLDER + $"/{Q.misionName}"))
            {
                AssetDatabase.CreateFolder(QuestConstants.MISIONS_FOLDER, $"{Q.misionName}");
            }


        }

        private void saveConections(Quest Q, List<NodeQuest> nodesInGraph)
        {
            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
            Q.ResetNodeLinksGraph();


            foreach (NodeQuest currentNode in nodesInGraph)
            {
                currentNode.nextNode.Clear();
            }


            for (int i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as NodeQuestGraph;
                var inputNode = connectedPorts[i].input.node as NodeQuestGraph;

                Q.nodeLinkData.Add(new Quest.NodeLinksGraph
                {
                    baseNodeGUID = outputNode.GUID,
                    portName = connectedPorts[i].output.portName,
                    targetNodeGUID = inputNode.GUID
                });

                //Add to next node list
                NodeQuest baseNode = nodesInGraph.Find(n => n.GUID == outputNode.GUID);
                NodeQuest targetNode = nodesInGraph.Find(n => n.GUID == inputNode.GUID);

                if (targetNode != null && baseNode != null)
                    baseNode.nextNode.Add(targetNode);
            }
        }

        public void SaveGraph(Quest Q)
        {
            if (!Edges.Any()) return;


            List<NodeQuest> NodesInGraph = new List<NodeQuest>();
            // Nodes
            creteNodeQuestAssets(Q, ref NodesInGraph);

            // Conections
            saveConections(Q, NodesInGraph);

            //Last Quest parameters

            var startNode = node.Find(node => node.entryPoint); //Find the first node Graph
            Q.startDay = startNode.startDay;
            Q.limitDay = startNode.limitDay;
            Q.isMain = startNode.isMain;

            
            //Questionable
            var firstMisionNode = Edges.Find(x => x.output.portName == "Next");
            var firstMisionNode2 = firstMisionNode.input.node as NodeQuestGraph;
            string GUIDfirst = firstMisionNode2.GUID;
            Q.firtsNode = NodesInGraph.Find(n => n.GUID == GUIDfirst);

            EditorUtility.SetDirty(Q);


        }

        public void LoadGraph(Quest Q)
        {
            if (Q == null)
            {
                EditorUtility.DisplayDialog("Error!!", "Quest aprece como null, revisa el scriptable object", "OK");
                return;
            }

            NodeQuest[] getNodes = Resources.LoadAll<NodeQuest>($"{QuestConstants.MISIONS_NAME}/{ Q.misionName}/Nodes");
            _cacheNodes = new List<NodeQuest>(getNodes);

            clearGraph(Q);
            LoadNodes(Q);
            ConectNodes(Q);
        }

        private void clearGraph(Quest Q)
        {
            node.Find(x => x.entryPoint).GUID = Q.nodeLinkData[0].baseNodeGUID;

            foreach (var node in node)
            {
                if (node.entryPoint)
                {
                    
                    var aux = node.mainContainer.Children().ToList();
                    var aux2 = aux[2].Children().ToList();

                    // C
                    TextField misionName = aux2[0] as TextField;
                    Toggle isMain = aux2[1] as Toggle;
                    IntegerField startDay = aux2[2] as IntegerField;
                    IntegerField limitDay = aux2[3] as IntegerField;

                    misionName.value = Q.misionName;
                    isMain.value = Q.isMain;
                    startDay.value = Q.startDay;
                    limitDay.value = Q.limitDay;

                    // 
                    node.limitDay = Q.limitDay;
                    node.startDay = Q.startDay;
                    node.isMain = Q.isMain;
                    node.misionName = Q.misionName;

                    continue;
                }

                //Remove edges
                Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                //Remove Node
                _targetGraphView.RemoveElement(node);
            }
        }

        private void LoadNodes(Quest Q)
        {
            foreach (var node in _cacheNodes)
            {
                var tempNode = _targetGraphView.CreateNodeQuest(node.name, Vector2.zero, node.extraText, node.isFinal);
                //Load node variables
                tempNode.GUID = node.GUID;
                tempNode.extraText = node.extraText;
                tempNode.isFinal = node.isFinal;
                tempNode.RefreshPorts();

                if (node.nodeObjectives != null) {
                    foreach (QuestObjective qObjective in node.nodeObjectives)
                    {
                        //CreateObjectives
                        QuestObjectiveGraph objtemp = new QuestObjectiveGraph(qObjective.keyName, qObjective.maxItems, qObjective.actualItems,
                                                          qObjective.description, qObjective.hiddenObjective, qObjective.autoExitOnCompleted);


                        var deleteButton = new Button(clickEvent: () => _targetGraphView.removeQuestObjective(tempNode, objtemp))
                        {
                            text = "x"
                        };
                        objtemp.Add(deleteButton);

                        var newBox = new Box();
                        objtemp.Add(newBox);


                        objtemp.actualItems = qObjective.actualItems;
                        objtemp.description = qObjective.description;
                        objtemp.maxItems = qObjective.maxItems;
                        objtemp.keyName = qObjective.keyName;
                        objtemp.hiddenObjective = qObjective.hiddenObjective;
                        objtemp.autoExitOnCompleted = qObjective.autoExitOnCompleted;

                        tempNode.objectivesRef.Add(objtemp);
                        tempNode.questObjectives.Add(objtemp);
                    }
                }
                

                _targetGraphView.AddElement(tempNode);

                var nodePorts = Q.nodeLinkData.Where(x => x.baseNodeGUID == node.GUID).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddNextNodePort(tempNode));


            }
        }
        
        private void ConectNodes(Quest Q)
        {
            List<NodeQuestGraph> nodeListCopy = new List<NodeQuestGraph>(node);

            for (int i = 0; i < nodeListCopy.Count; i++)
            {
                var conections = Q.nodeLinkData.Where(x => x.baseNodeGUID == nodeListCopy[i].GUID).ToList();

                for (int j = 0; j < conections.Count(); j++)
                {
                    string targetNodeGUID = conections[j].targetNodeGUID;
                    var targetNode = nodeListCopy.Find(x => x.GUID == targetNodeGUID);
                    LinkNodes(nodeListCopy[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(_cacheNodes.First(x => x.GUID == targetNodeGUID).position, new Vector2(150, 200)));
                }
            }
        }

        private void LinkNodes(Port outpor, Port inport)
        {
            var tempEdge = new Edge
            {
                output = outpor,
                input = inport
            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);


        }

        public QuestObjective[] createObjectivesFromGraph(List<QuestObjectiveGraph> qog)
        {
            List<QuestObjective> Listaux = new List<QuestObjective>();

            foreach (QuestObjectiveGraph obj in qog)
            {
                QuestObjective aux = new QuestObjective
                {
                    keyName = obj.keyName,
                    maxItems = obj.maxItems,
                    actualItems = obj.actualItems,
                    description = obj.description,
                    hiddenObjective = obj.hiddenObjective,
                    autoExitOnCompleted = obj.autoExitOnCompleted
                };

                Listaux.Add(aux);

            }

            return Listaux.ToArray();
        }


    }
}