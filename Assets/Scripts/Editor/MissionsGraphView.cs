using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Birdgame.Editor
{
    public class MissionsGraphView : GraphView
    {
        public MissionsGraphView()
        {
            Insert(0, new GridBackground());
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            AddElement(CreateSingleNodeView());
        }

        public MissionNode AddSingleMissionNodeToView()
        {
            var newNode = CreateSingleNodeView();
            AddElement(newNode);

            newNode.RefreshExpandedState();
            newNode.RefreshPorts();
            return newNode;
        }

        public DoubleMissionNode AddDoubleMissionNodeToView()
        {
            var newNode = GenerateDoubleNode();
            AddElement(newNode);

            newNode.RefreshExpandedState();
            newNode.RefreshPorts();
            return newNode;
        }

        private MissionNode CreateSingleNodeView()
        {
            var node = new MissionNode(RemovePort)
            {
                title = "New Mission"
            };

            var pos = contentViewContainer.WorldToLocal(new Vector2(0, 0));
            node.SetPosition(new Rect(pos.x, pos.y + 100, 300, 250));

            return node;
        }

        private DoubleMissionNode GenerateDoubleNode()
        {
            var node = new DoubleMissionNode(RemovePort)
            {
                title = "New Mission"
            };

            var pos = contentViewContainer.WorldToLocal(new Vector2(0, 0));
            node.SetPosition(new Rect(pos.x, pos.y + 100, 400, 250));

            node.RefreshExpandedState();
            node.RefreshPorts();

            return node;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            foreach (var port in ports)
            {
                if (startPort != port && startPort.node != port.node && port.connections.Count() <= 1)
                {
                    compatiblePorts.Add(port);
                }
            }

            return compatiblePorts;
        }

        private void RemovePort(Node node, Port port)
        {
            if (port.connected)
            {
                DeleteElements(port.connections);
            }

            if (port.direction == Direction.Output)
            {
                node.outputContainer.Remove(port);
            }
            else
            {
                node.inputContainer.Remove(port);
            }
        }
    }
}