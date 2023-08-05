using UnityEngine;

public class NodeGraphHandler : MonoBehaviour
{
    public StateGraph nodeGraph;

    public void ProcessNodeGraph()
    {
        if (nodeGraph == null)
        {
            Debug.Log("Node Graph is null");
            return;
        }

        StateNode initialNode = nodeGraph.current;

        while (nodeGraph.current != null)
        {
            nodeGraph.current.OnEnter();
            nodeGraph.current.MoveNext();
        }

        nodeGraph.current = initialNode;
    }
}