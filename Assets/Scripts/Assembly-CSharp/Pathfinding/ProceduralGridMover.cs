using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_procedural_grid_mover.php")]
	public class ProceduralGridMover : VersionedMonoBehaviour
	{
		public float updateDistance = 10f;

		public Transform target;

		public bool floodFill = true;

		private GridGraph graph;

		private GridNodeBase[] buffer;

		public bool updatingGraph { get; private set; }

		private void Start()
		{
			if (AstarPath.active == null)
			{
				throw new Exception("There is no AstarPath object in the scene");
			}
			graph = AstarPath.active.data.FindGraphWhichInheritsFrom(typeof(GridGraph)) as GridGraph;
			if (graph == null)
			{
				throw new Exception("The AstarPath object has no GridGraph or LayeredGridGraph");
			}
			UpdateGraph();
		}

		private void Update()
		{
			if (graph != null)
			{
				Vector3 a = PointToGraphSpace(graph.center);
				Vector3 b = PointToGraphSpace(target.position);
				if (VectorMath.SqrDistanceXZ(a, b) > updateDistance * updateDistance)
				{
					UpdateGraph();
				}
			}
		}

		private Vector3 PointToGraphSpace(Vector3 p)
		{
			return graph.transform.InverseTransform(p);
		}

		public void UpdateGraph()
		{
			if (updatingGraph)
			{
				return;
			}
			updatingGraph = true;
			IEnumerator ie = UpdateGraphCoroutine();
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(IWorkItemContext context, bool force)
			{
				if (floodFill)
				{
					context.QueueFloodFill();
				}
				if (force)
				{
					while (ie.MoveNext())
					{
					}
				}
				bool flag;
				try
				{
					flag = !ie.MoveNext();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, this);
					flag = true;
				}
				if (flag)
				{
					updatingGraph = false;
				}
				return flag;
			}));
		}

		private IEnumerator UpdateGraphCoroutine()
		{
			Vector3 dir = PointToGraphSpace(target.position) - PointToGraphSpace(graph.center);
			dir.x = Mathf.Round(dir.x);
			dir.z = Mathf.Round(dir.z);
			dir.y = 0f;
			if (dir == Vector3.zero)
			{
				yield break;
			}
			Int2 offset = new Int2(-Mathf.RoundToInt(dir.x), -Mathf.RoundToInt(dir.z));
			graph.center += graph.transform.TransformVector(dir);
			graph.UpdateTransform();
			int width = graph.width;
			int depth = graph.depth;
			int layers = graph.LayerCount;
			LayerGridGraph layeredGraph = graph as LayerGridGraph;
			GridNodeBase[] nodes = ((layeredGraph == null) ? ((GridNodeBase[])graph.nodes) : ((GridNodeBase[])layeredGraph.nodes));
			if (buffer == null || buffer.Length != width * depth)
			{
				buffer = new GridNodeBase[width * depth];
			}
			if (Mathf.Abs(offset.x) <= width && Mathf.Abs(offset.y) <= depth)
			{
				IntRect recalculateRect = new IntRect(0, 0, offset.x, offset.y);
				if (recalculateRect.xmin > recalculateRect.xmax)
				{
					int xmax = recalculateRect.xmax;
					recalculateRect.xmax = width + recalculateRect.xmin;
					recalculateRect.xmin = width + xmax;
				}
				if (recalculateRect.ymin > recalculateRect.ymax)
				{
					int ymax = recalculateRect.ymax;
					recalculateRect.ymax = depth + recalculateRect.ymin;
					recalculateRect.ymin = depth + ymax;
				}
				IntRect connectionRect = recalculateRect.Expand(1);
				connectionRect = IntRect.Intersection(connectionRect, new IntRect(0, 0, width, depth));
				for (int l = 0; l < layers; l++)
				{
					int layerOffset = l * width * depth;
					for (int i = 0; i < depth; i++)
					{
						int num = i * width;
						int num2 = (i + offset.y + depth) % depth * width;
						for (int j = 0; j < width; j++)
						{
							buffer[num2 + (j + offset.x + width) % width] = nodes[layerOffset + num + j];
						}
					}
					yield return null;
					for (int k = 0; k < depth; k++)
					{
						int num3 = k * width;
						for (int m = 0; m < width; m++)
						{
							int num4 = num3 + m;
							GridNodeBase gridNodeBase = buffer[num4];
							if (gridNodeBase != null)
							{
								gridNodeBase.NodeInGridIndex = num4;
							}
							nodes[layerOffset + num4] = gridNodeBase;
						}
						int num5;
						int num6;
						if (k >= recalculateRect.ymin && k < recalculateRect.ymax)
						{
							num5 = 0;
							num6 = depth;
						}
						else
						{
							num5 = recalculateRect.xmin;
							num6 = recalculateRect.xmax;
						}
						for (int n = num5; n < num6; n++)
						{
							GridNodeBase gridNodeBase2 = buffer[num3 + n];
							if (gridNodeBase2 != null)
							{
								gridNodeBase2.ClearConnections(false);
							}
						}
					}
					yield return null;
				}
				int yieldEvery = 1000;
				int approxNumNodesToUpdate = Mathf.Max(Mathf.Abs(offset.x), Mathf.Abs(offset.y)) * Mathf.Max(width, depth);
				yieldEvery = Mathf.Max(yieldEvery, approxNumNodesToUpdate / 10);
				int counter = 0;
				for (int z = 0; z < depth; z++)
				{
					int xmin;
					int xmax2;
					if (z >= recalculateRect.ymin && z < recalculateRect.ymax)
					{
						xmin = 0;
						xmax2 = width;
					}
					else
					{
						xmin = recalculateRect.xmin;
						xmax2 = recalculateRect.xmax;
					}
					for (int num7 = xmin; num7 < xmax2; num7++)
					{
						graph.RecalculateCell(num7, z, false, false);
					}
					counter += xmax2 - xmin;
					if (counter > yieldEvery)
					{
						counter = 0;
						yield return null;
					}
				}
				for (int num8 = 0; num8 < depth; num8++)
				{
					int xmin2;
					int xmax3;
					if (num8 >= connectionRect.ymin && num8 < connectionRect.ymax)
					{
						xmin2 = 0;
						xmax3 = width;
					}
					else
					{
						xmin2 = connectionRect.xmin;
						xmax3 = connectionRect.xmax;
					}
					for (int num9 = xmin2; num9 < xmax3; num9++)
					{
						graph.CalculateConnections(num9, num8);
					}
					counter += xmax3 - xmin2;
					if (counter > yieldEvery)
					{
						counter = 0;
						yield return null;
					}
				}
				yield return null;
				for (int num10 = 0; num10 < depth; num10++)
				{
					for (int num11 = 0; num11 < width; num11++)
					{
						if (num11 == 0 || num10 == 0 || num11 == width - 1 || num10 == depth - 1)
						{
							graph.CalculateConnections(num11, num10);
						}
					}
				}
				if (!floodFill)
				{
					graph.GetNodes(delegate(GraphNode node)
					{
						node.Area = 1u;
					});
				}
				yield break;
			}
			int yieldEvery2 = Mathf.Max(depth * width / 20, 1000);
			int counter2 = 0;
			for (int z2 = 0; z2 < depth; z2++)
			{
				for (int num12 = 0; num12 < width; num12++)
				{
					graph.RecalculateCell(num12, z2);
				}
				counter2 += width;
				if (counter2 > yieldEvery2)
				{
					counter2 = 0;
					yield return null;
				}
			}
			for (int z3 = 0; z3 < depth; z3++)
			{
				for (int num13 = 0; num13 < width; num13++)
				{
					graph.CalculateConnections(num13, z3);
				}
				counter2 += width;
				if (counter2 > yieldEvery2)
				{
					counter2 = 0;
					yield return null;
				}
			}
		}
	}
}
