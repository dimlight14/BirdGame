using System.Collections.Generic;
using System.Linq;
using Birdgame.Heroes;
using Birdgame.Missions.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Birdgame.Editor
{
    public class MissionGraphDataLoader
    {
        private MissionsGraphView _graphView;
        private MissionsDatabase _database;
        private AuxiliaryNodePositionsData _nodesPositionsData;
        private Dictionary<string, MissionNodeBase> _missionNodes;

        public void LoadGraph(string assetPath, MissionsGraphView graphView)
        {
            _graphView = graphView;
            _database = AssetDatabase.LoadAssetAtPath<MissionsDatabase>(assetPath);
            _nodesPositionsData = AssetDatabase.LoadAssetAtPath<AuxiliaryNodePositionsData>("Assets/Settings/AuxiliaryNodePositionData.asset");
            _missionNodes = new Dictionary<string, MissionNodeBase>(_database.MissionsDataById.Count);

            ClearGraph();
            LoadNodes();
            LoadConnections();
            _missionNodes.Clear();
        }

        private void ClearGraph()
        {
            foreach (var nodeEdge in _graphView.edges)
            {
                _graphView.RemoveElement(nodeEdge);
            }

            foreach (var node in _graphView.nodes)
            {
                _graphView.RemoveElement(node);
            }
        }

        private void LoadNodes()
        {
            foreach (var missionToLoad in _database.MissionsDataById.Values)
            {
                if (missionToLoad.MissionType == MissionType.SingleMission)
                {
                    CreateSingleMissionNode(missionToLoad);
                }
                else
                {
                    if (!missionToLoad.DoubleMissionData.IsSubservient)
                    {
                        CreateDoubleMissionNode(missionToLoad);
                    }
                }
            }
        }

        private void CreateSingleMissionNode(MissionStaticData missionToLoad)
        {
            var newNode = _graphView.AddSingleMissionNodeToView();
            
            newNode.NodeData.Id = missionToLoad.MissionId;
            newNode.NodeData.HeroUnlocks = new List<HeroType>(missionToLoad.HeroUnlocks);
            newNode.NodeData.HeroPowerRewards = new List<MissionHeroReward>(missionToLoad.HeroPowerRewards);
            newNode.NodeData.MapPosition = missionToLoad.MapPosition;
            newNode.NodeData.Description = missionToLoad.Description;
            newNode.NodeData.TemporarilyHides = new List<string>(missionToLoad.TemporarilyHides);
            
            newNode.SetId(missionToLoad.MissionId);
            TrySetNodePosition(newNode, missionToLoad.MissionId);
            newNode.RefreshExpandedState();
            _missionNodes.Add(missionToLoad.MissionId, newNode);
        }

        private void CreateDoubleMissionNode(MissionStaticData missionToLoad)
        {
            var competingMission = _database.MissionsDataById[missionToLoad.DoubleMissionData.CompetingMission];
            var newNode = _graphView.AddDoubleMissionNodeToView();

            newNode.NodeData.MissionAId = missionToLoad.MissionId;
            newNode.NodeData.MissionAHeroUnlocks = new List<HeroType>(missionToLoad.HeroUnlocks);
            newNode.NodeData.MissionAHeroPowerRewards = new List<MissionHeroReward>(missionToLoad.HeroPowerRewards);
            newNode.NodeData.MissionADescription = missionToLoad.Description;

            newNode.NodeData.MissionBId = competingMission.MissionId;
            newNode.NodeData.MissionBHeroUnlocks = new List<HeroType>(competingMission.HeroUnlocks);
            newNode.NodeData.MissionBHeroPowerRewards = new List<MissionHeroReward>(competingMission.HeroPowerRewards);
            newNode.NodeData.MissionBDescription = competingMission.Description;

            newNode.NodeData.MapPosition = missionToLoad.MapPosition;
            newNode.NodeData.TemporarilyHides = new List<string>(missionToLoad.TemporarilyHides);

            newNode.SetId(missionToLoad.MissionId, true);
            newNode.SetId(competingMission.MissionId, false);
            TrySetNodePosition(newNode, missionToLoad.MissionId);
            newNode.RefreshExpandedState();
            _missionNodes.Add(missionToLoad.MissionId, newNode);
            _missionNodes.Add(competingMission.MissionId, newNode);
        }

        private void TrySetNodePosition(Node node, string missionId)
        {
            var position = _nodesPositionsData.NodePositionsData?.FirstOrDefault(data => data.MissionId == missionId);
            if (position == null)
            {
                return;
            }

            node.SetPosition(position.Position);
        }

        private void LoadConnections()
        {
            foreach (var missionToLoad in _database.MissionsDataById.Values)
            {
                if (missionToLoad.SimpleUnlockConditions.Count == 0 && missionToLoad.EitherOrUnlockConditions.Count == 0)
                {
                    continue;
                }

                foreach (var unlockingMission in missionToLoad.SimpleUnlockConditions)
                {
                    CreateSingleConnection(unlockingMission, missionToLoad);
                }

                foreach (var eitherOrMissions in missionToLoad.EitherOrUnlockConditions)
                {
                    CreateEitherOrConnection(eitherOrMissions, missionToLoad);
                }
            }
        }

        private void CreateSingleConnection(string unlockingMission, MissionStaticData missionToLoad)
        {
            if (!_missionNodes.TryGetValue(unlockingMission, out var outputNode))
            {
                return;
            }

            var inputNode = _missionNodes[missionToLoad.MissionId];
            var outputPort = outputNode.LoadNode(Direction.Output, Port.Capacity.Single, unlockingMission, missionToLoad.MissionId);
            var inputPort = inputNode.LoadNode(Direction.Input, Port.Capacity.Single, unlockingMission, missionToLoad.MissionId);
            
            var edge = new Edge() { output = outputPort, input = inputPort };
            inputPort.Connect(edge);
            outputPort.Connect(edge);
            _graphView.Add(edge);
        }

        private void CreateEitherOrConnection(EitherOrCondition eitherOrMissions, MissionStaticData missionToLoad)
        {
            if (!_missionNodes.TryGetValue(eitherOrMissions.ConditionA, out var outputNodeA))
            {
                return;
            }

            if (!_missionNodes.TryGetValue(eitherOrMissions.ConditionB, out var outputNodeB))
            {
                return;
            }

            var inputNode = _missionNodes[missionToLoad.MissionId];
            var outputPortA = outputNodeA.LoadNode(Direction.Output, Port.Capacity.Single, eitherOrMissions.ConditionA, missionToLoad.MissionId);
            var outputPortB = outputNodeB.LoadNode(Direction.Output, Port.Capacity.Single, eitherOrMissions.ConditionB, missionToLoad.MissionId);
            var inputPort = inputNode.LoadNode(Direction.Input, Port.Capacity.Multi, eitherOrMissions.ConditionA + "/" + eitherOrMissions.ConditionB, missionToLoad.MissionId);

            var edge = new Edge() { output = outputPortA, input = inputPort };
            inputPort.Connect(edge);
            outputPortA.Connect(edge);
            _graphView.Add(edge);

            edge = new Edge() { output = outputPortB, input = inputPort };
            inputPort.Connect(edge);
            outputPortB.Connect(edge);
            _graphView.Add(edge);
        }
    }
}