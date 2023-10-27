using System.Collections.Generic;
using System.Linq;
using Birdgame.Heroes;
using Birdgame.Missions.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Birdgame.Editor
{
    public class MissionsGraphDataSaver
    {
        private MissionsGraphView _graphView;
        private MissionsDatabase _database;
        private List<MissionNodeBase> _missionNodes;
        private List<MissionStaticData> _preparedDataToSave;
        private AuxiliaryNodePositionsData _nodePositionsData;

        public void SaveGraph(string assetPath, MissionsGraphView graphView)
        {
            _graphView = graphView;
            _database = AssetDatabase.LoadAssetAtPath<MissionsDatabase>(assetPath);
            _nodePositionsData = AssetDatabase.LoadAssetAtPath<AuxiliaryNodePositionsData>("Assets/Settings/AuxiliaryNodePositionData.asset");
            _missionNodes = _graphView.nodes.ToList().Cast<MissionNodeBase>().ToList();
            _nodePositionsData.NodePositionsData.Clear();
            _preparedDataToSave = new();

            foreach (var node in _missionNodes)
            {
                if (node is MissionNode)
                {
                    ParseSingleNode(node as MissionNode);
                }
                else
                {
                    ParseDoubleNode(node as DoubleMissionNode);
                }
                
            }

            _database.SaveDataFromEditor(_preparedDataToSave);
            EditorUtility.SetDirty(_nodePositionsData);
            EditorUtility.SetDirty(_database);
            AssetDatabase.SaveAssets();
        }

        private void ParseSingleNode(MissionNode node)
        {
            var dataToSave = new MissionStaticData();
            var nodeData = node.NodeData;
            
            dataToSave.MissionId = nodeData.Id;
            dataToSave.MissionType = MissionType.SingleMission;
            dataToSave.HeroUnlocks = new List<HeroType>(nodeData.HeroUnlocks);
            dataToSave.TemporarilyHides = new List<string>(nodeData.TemporarilyHides);
            dataToSave.HeroPowerRewards = new List<MissionHeroReward>(nodeData.HeroPowerRewards);
            dataToSave.Description = nodeData.Description;
            dataToSave.MapPosition = nodeData.MapPosition;
            dataToSave.DoubleMissionData = null;
            dataToSave.SimpleUnlockConditions = new List<string>();
            dataToSave.EitherOrUnlockConditions = new List<EitherOrCondition>();
            ParseInputNodes(node, dataToSave);
            
            _preparedDataToSave.Add(dataToSave);
            _nodePositionsData.NodePositionsData.Add(new NodePositionData(){ MissionId = nodeData.Id, Position = node.GetPosition() });
        }


        private void ParseDoubleNode(DoubleMissionNode node)
        {
            var nodeData = node.NodeData;
            var dataOneToSave = new MissionStaticData();
            var dataTwoToSave = new MissionStaticData();

            dataOneToSave.MissionId = nodeData.MissionAId;
            dataOneToSave.MissionType = MissionType.DoubleMission;
            dataOneToSave.HeroUnlocks = new List<HeroType>(nodeData.MissionAHeroUnlocks);
            dataOneToSave.HeroPowerRewards = new List<MissionHeroReward>(nodeData.MissionAHeroPowerRewards);
            dataOneToSave.Description = nodeData.MissionADescription;
            dataOneToSave.DoubleMissionData = new DoubleMissionData()
            {
                IsSubservient = false,
                CompetingMission = nodeData.MissionBId
            };
            
            dataTwoToSave.MissionId = nodeData.MissionBId;
            dataTwoToSave.MissionType = MissionType.DoubleMission;
            dataTwoToSave.HeroUnlocks = new List<HeroType>(nodeData.MissionBHeroUnlocks);
            dataTwoToSave.HeroPowerRewards = new List<MissionHeroReward>(nodeData.MissionBHeroPowerRewards);
            dataTwoToSave.Description = nodeData.MissionBDescription;
            dataTwoToSave.DoubleMissionData = new DoubleMissionData()
            {
                IsSubservient = true,
                CompetingMission = nodeData.MissionAId
            };

            dataOneToSave.MapPosition = nodeData.MapPosition;
            dataOneToSave.TemporarilyHides = new List<string>(nodeData.TemporarilyHides);
            dataOneToSave.SimpleUnlockConditions = new List<string>();
            dataOneToSave.EitherOrUnlockConditions = new List<EitherOrCondition>();
            ParseInputNodes(node, dataOneToSave);

            _preparedDataToSave.Add(dataOneToSave);
            _preparedDataToSave.Add(dataTwoToSave);
            _nodePositionsData.NodePositionsData.Add(new NodePositionData(){ MissionId = nodeData.MissionAId,Position = node.GetPosition() });
        }

        private void ParseInputNodes(Node node, MissionStaticData dataToSave)
        {
            foreach (var visualElement in node.inputContainer.Children())
            {
                if (visualElement is not Port port)
                {
                    continue;
                }

                if (!port.connected)
                {
                    continue;
                }

                if (port.capacity == Port.Capacity.Single)
                {
                    var outputPort = port.connections.First().output;
                    var outNode = outputPort.node as MissionNodeBase;
                    dataToSave.SimpleUnlockConditions.Add(outNode.GetOutputId(outputPort));
                }
                else if(port.connections.Count() == 2)
                {
                    var outputPort = port.connections.ElementAt(0).output;
                    var outNode = outputPort.node as MissionNodeBase;
                    var firstString = outNode.GetOutputId(outputPort);
                    
                    outputPort = port.connections.ElementAt(1).output;
                    outNode = outputPort.node as MissionNodeBase;
                    var secondString = outNode.GetOutputId(outputPort);
                    
                    dataToSave.EitherOrUnlockConditions.Add(new EitherOrCondition(){ConditionA = firstString,ConditionB = secondString});
                }
            }
        }
    }
}