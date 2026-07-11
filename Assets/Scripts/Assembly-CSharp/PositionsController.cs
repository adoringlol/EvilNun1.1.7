using System.Collections.Generic;
using UnityEngine;

public class PositionsController : MonoBehaviour
{
	public static PositionsController instance;

	public List<PositionData> positions;

	public List<SkullData> availableSkulls;

	public List<RitualPosition> ritualPositions;

	private void Awake()
	{
		instance = this;
		if (positions == null) positions = new List<PositionData>();
		if (availableSkulls == null) availableSkulls = new List<SkullData>();
		if (ritualPositions == null) ritualPositions = new List<RitualPosition>();
	}

	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
	}

	public void AddPosition(PositionBehaviour pb)
	{
		PositionData positionData = positions.Find((PositionData p) => p.place == pb.place);
		if (positionData == null)
		{
			PositionData positionData2 = new PositionData();
			positionData2.place = pb.place;
			positionData2.points = new List<Transform>();
			positions.Add(positionData2);
		}
		positionData = positions.Find((PositionData p) => p.place == pb.place);
		if (pb.isCenter)
		{
			positionData.centerPosition = pb.transform;
			positionData.needSchoolOpened = pb.needSchoolOpened;
			positionData.type = pb.type;
		}
		positionData.points.Add(pb.transform);
	}

	public Vector3 GetPositionOfPlace(PositionPlace p)
	{
		PositionData positionData = positions.Find((PositionData pd) => pd.place == p && (!pd.needSchoolOpened || (pd.needSchoolOpened && ProgressManager.instance.schoolOpened)));
		if (positionData == null || positionData.points == null || positionData.points.Count == 0) return Vector3.zero;
		Transform point = positionData.points[Random.Range(0, positionData.points.Count)];
		return point == null ? Vector3.zero : point.position;
	}

	public void SetSkullInRitual(SkullBehaviour sk)
	{
		foreach (RitualPosition ritualPosition in ritualPositions)
		{
			if (ritualPosition.free)
			{
				availableSkulls.Find((SkullData a) => a.skull == sk.GetComponent<TouchableElement>()).inRitual = true;
				sk.transform.parent = null;
				sk.transform.position = ritualPosition.position.position;
				sk.GetComponent<TouchableElement>().enabled = false;
				sk.tag = "Default";
				ritualPosition.free = false;
				break;
			}
		}
	}

	public Vector3 GetNearestPositionToNoise(Vector3 pos)
	{
		List<PositionData> list = positions.FindAll((PositionData pd) => pd.type == PositionType.LOOK_ROOM && (!pd.needSchoolOpened || (pd.needSchoolOpened && ProgressManager.instance.schoolOpened)));
		if (list.Count > 0 && list[0].points != null && list[0].points.Count > 0 && list[0].points[0] != null)
		{
			Vector3 position = list[0].points[0].position;
			float num = Vector3.Distance(position, pos);
			{
				foreach (PositionData item in list)
				{
					foreach (Transform point in item.points)
					{
						if (Vector3.Distance(point.position, pos) < num)
						{
							position = point.position;
							num = Vector3.Distance(point.position, pos);
						}
					}
				}
				return position;
			}
		}
		return Vector3.zero;
	}

	public PositionData GetNearRoomTo(Vector3 pos)
	{
		List<PositionData> list = positions.FindAll((PositionData pd) => pd.type == PositionType.LOOK_ROOM && (!pd.needSchoolOpened || (pd.needSchoolOpened && ProgressManager.instance.schoolOpened)));
		if (TutorialController.instance != null && TutorialController.instance.isEnabled)
		{
			return list.Find((PositionData p) => p.place == PositionPlace.MAIN_ENTRANCE_ROOM);
		}
		if (list.Count > 0 && list[0].centerPosition != null)
		{
			PositionData positionData = list[0];
			float num = Vector3.Distance(positionData.centerPosition.position, pos);
			{
				foreach (PositionData item in list)
				{
					if (Vector3.Distance(item.centerPosition.position, pos) < num)
					{
						positionData = item;
						num = Vector3.Distance(item.centerPosition.position, pos);
					}
				}
				return positionData;
			}
		}
		return null;
	}

	public PositionData GetComunZoneTo(Vector3 pos)
	{
		List<PositionData> list = positions.FindAll((PositionData pd) => pd.type == PositionType.COMUN && (!pd.needSchoolOpened || (pd.needSchoolOpened && ProgressManager.instance.schoolOpened)));
		if (TutorialController.instance != null && TutorialController.instance.isEnabled)
		{
			return list.Find((PositionData p) => p.place == PositionPlace.MAIN_ENTRANCE_ROOM);
		}
		if (list.Count > 0 && list[0].centerPosition != null)
		{
			PositionData positionData = list[0];
			float num = Vector3.Distance(positionData.centerPosition.position, pos);
			{
				foreach (PositionData item in list)
				{
					if (Vector3.Distance(item.centerPosition.position, pos) < num)
					{
						positionData = item;
						num = Vector3.Distance(item.centerPosition.position, pos);
					}
				}
				return positionData;
			}
		}
		return null;
	}

	public PositionData GetRandomComunZoneTo(Vector3 pos)
	{
		List<PositionData> list = positions.FindAll((PositionData pd) => pd.type == PositionType.COMUN && (!pd.needSchoolOpened || (pd.needSchoolOpened && ProgressManager.instance.schoolOpened)));
		if (TutorialController.instance != null && TutorialController.instance.isEnabled)
		{
			return list.Find((PositionData p) => p.place == PositionPlace.MAIN_ENTRANCE_ROOM);
		}
		if (list.Count > 0)
		{
			return list[Random.Range(0, list.Count)];
		}
		return null;
	}

	public Transform GetRandomAvailableComunPosition()
	{
		List<PositionData> list = positions.FindAll((PositionData pd) => pd.type == PositionType.COMUN && (!pd.needSchoolOpened || (pd.needSchoolOpened && ProgressManager.instance.schoolOpened)));
		if (TutorialController.instance != null && TutorialController.instance.isEnabled)
		{
			PositionData entrance = list.Find((PositionData p) => p.place == PositionPlace.MAIN_ENTRANCE_ROOM);
			return entrance == null ? null : entrance.centerPosition;
		}
		if (list.Count > 0)
		{
			return list[Random.Range(0, list.Count)].centerPosition;
		}
		return null;
	}
}
