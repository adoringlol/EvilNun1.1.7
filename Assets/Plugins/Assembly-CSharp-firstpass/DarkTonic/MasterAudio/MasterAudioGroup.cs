using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTonic.MasterAudio
{
	public class MasterAudioGroup : MonoBehaviour
	{
		public enum TargetDespawnedBehavior
		{
			None = 0,
			Stop = 1,
			FadeOut = 2
		}

		public enum VariationSequence
		{
			Randomized = 0,
			TopToBottom = 1
		}

		public enum VariationMode
		{
			Normal = 0,
			LoopedChain = 1,
			Dialog = 2
		}

		public enum ChainedLoopLoopMode
		{
			Endless = 0,
			NumberOfLoops = 1
		}

		public enum LimitMode
		{
			None = 0,
			FrameBased = 1,
			TimeBased = 2
		}

		public const float UseCurveSpatialBlend = -99f;

		public const string NoBus = "[NO BUS]";

		public const int MinNoRepeatVariations = 3;

		public int busIndex = -1;

		public MasterAudio.ItemSpatialBlendType spatialBlendType = MasterAudio.ItemSpatialBlendType.ForceTo3D;

		public float spatialBlend = 1f;

		public bool isSelected;

		public bool isExpanded = true;

		public float groupMasterVolume = 1f;

		public int retriggerPercentage = 100;

		public VariationMode curVariationMode;

		public bool alwaysHighestPriority;

		public float chainLoopDelayMin;

		public float chainLoopDelayMax;

		public ChainedLoopLoopMode chainLoopMode;

		public int chainLoopNumLoops;

		public bool useDialogFadeOut;

		public float dialogFadeOutTime = 0.5f;

		public VariationSequence curVariationSequence;

		public bool useNoRepeatRefill = true;

		public bool useInactivePeriodPoolRefill;

		public float inactivePeriodSeconds = 5f;

		public List<SoundGroupVariation> groupVariations = new List<SoundGroupVariation>();

		public MasterAudio.AudioLocation bulkVariationMode;

		public bool resourceClipsAllLoadAsync = true;

		public bool logSound;

		public bool copySettingsExpanded;

		public int selectedVariationIndex;

		public bool expandLinkedGroups;

		public List<string> childSoundGroups = new List<string>();

		public List<string> endLinkedGroups = new List<string>();

		public MasterAudio.LinkedGroupSelectionType linkedStartGroupSelectionType;

		public MasterAudio.LinkedGroupSelectionType linkedStopGroupSelectionType;

		public LimitMode limitMode;

		public int limitPerXFrames = 1;

		public float minimumTimeBetween = 0.1f;

		public bool useClipAgePriority;

		public bool limitPolyphony;

		public int voiceLimitCount = 1;

		public TargetDespawnedBehavior targetDespawnedBehavior = TargetDespawnedBehavior.FadeOut;

		public float despawnFadeTime = 0.3f;

		public bool isUsingOcclusion;

		public bool willOcclusionOverrideRaycastOffset;

		public float occlusionRayCastOffset;

		public bool willOcclusionOverrideFrequencies;

		public float occlusionMaxCutoffFreq;

		public float occlusionMinCutoffFreq = 22000f;

		public bool isSoloed;

		public bool isMuted;

		public bool soundPlayedEventActive;

		public string soundPlayedCustomEvent = string.Empty;

		public bool willCleanUpDelegatesAfterStop = true;

		public int frames;

		private List<int> _activeAudioSourcesIds;

		private string _objectName = string.Empty;

		private Transform _trans;

		private float _originalVolume = 1f;

		public MasterAudio.InternetFileLoadStatus GroupLoadStatus
		{
			get
			{
				MasterAudio.InternetFileLoadStatus result = MasterAudio.InternetFileLoadStatus.Loaded;
				for (int i = 0; i < Trans.childCount; i++)
				{
					SoundGroupVariation component = Trans.GetChild(i).GetComponent<SoundGroupVariation>();
					if (component.audLocation == MasterAudio.AudioLocation.FileOnInternet)
					{
						if (component.internetFileLoadStatus == MasterAudio.InternetFileLoadStatus.Failed)
						{
							result = MasterAudio.InternetFileLoadStatus.Failed;
							break;
						}
						if (component.internetFileLoadStatus == MasterAudio.InternetFileLoadStatus.Loading)
						{
							result = MasterAudio.InternetFileLoadStatus.Loading;
						}
					}
				}
				return result;
			}
		}

		public float SpatialBlendForGroup
		{
			get
			{
				switch (MasterAudio.Instance.mixerSpatialBlendType)
				{
				case MasterAudio.AllMixerSpatialBlendType.ForceAllTo2D:
					return 0f;
				case MasterAudio.AllMixerSpatialBlendType.ForceAllTo3D:
					return 1f;
				case MasterAudio.AllMixerSpatialBlendType.ForceAllToCustom:
					return MasterAudio.Instance.mixerSpatialBlend;
				default:
					switch (spatialBlendType)
					{
					case MasterAudio.ItemSpatialBlendType.ForceTo2D:
						return 0f;
					case MasterAudio.ItemSpatialBlendType.ForceTo3D:
						return 1f;
					case MasterAudio.ItemSpatialBlendType.ForceToCustom:
						return spatialBlend;
					default:
						return -99f;
					}
				}
			}
		}

		public int ActiveVoices
		{
			get
			{
				return ActiveAudioSourceIds.Count;
			}
		}

		public int TotalVoices
		{
			get
			{
				return base.transform.childCount;
			}
		}

		public bool WillCleanUpDelegatesAfterStop
		{
			set
			{
				willCleanUpDelegatesAfterStop = value;
			}
		}

		public GroupBus BusForGroup
		{
			get
			{
				if (busIndex < 2)
				{
					return null;
				}
				int num = busIndex - 2;
				if (num >= MasterAudio.GroupBuses.Count)
				{
					return null;
				}
				return MasterAudio.GroupBuses[num];
			}
		}

		public float OriginalVolume
		{
			get
			{
				return _originalVolume;
			}
			set
			{
				_originalVolume = value;
			}
		}

		public bool LoggingEnabledForGroup
		{
			get
			{
				return logSound || MasterAudio.LogSoundsEnabled;
			}
		}

		public int ChainLoopCount { get; set; }

		public string GameObjectName
		{
			get
			{
				if (string.IsNullOrEmpty(_objectName))
				{
					_objectName = base.name;
				}
				return _objectName;
			}
		}

		public bool UsesNoRepeat
		{
			get
			{
				return curVariationSequence == VariationSequence.Randomized && groupVariations.Count >= 3 && useNoRepeatRefill;
			}
		}

		private Transform Trans
		{
			get
			{
				if (_trans != null)
				{
					return _trans;
				}
				_trans = base.transform;
				return _trans;
			}
		}

		private List<int> ActiveAudioSourceIds
		{
			get
			{
				if (_activeAudioSourcesIds != null)
				{
					return _activeAudioSourcesIds;
				}
				_activeAudioSourcesIds = new List<int>(Trans.childCount);
				return _activeAudioSourcesIds;
			}
		}

		public event Action LastVariationFinishedPlay;

		private void Start()
		{
			_objectName = base.name;
			int count = ActiveAudioSourceIds.Count;
			if (count > 0)
			{
			}
			bool flag = false;
			if (Trans.parent != null)
			{
				base.gameObject.layer = Trans.parent.gameObject.layer;
			}
			for (int i = 0; i < Trans.childCount; i++)
			{
				SoundGroupVariation component = Trans.GetChild(i).GetComponent<SoundGroupVariation>();
				if (!(component == null))
				{
					SoundGroupVariationUpdater component2 = component.GetComponent<SoundGroupVariationUpdater>();
					if (!(component2 != null))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				Debug.LogError("One or more Variations of Sound Group '" + GameObjectName + "' do not have the SoundGroupVariationUpdater component and will not function properly. Please stop and fix this by opening the Master Audio Manager window and clicking the Upgrade MA Prefab button before continuing.");
			}
		}

		private void OnDisable()
		{
			for (int i = 0; i < Trans.childCount; i++)
			{
				SoundGroupVariation component = Trans.GetChild(i).GetComponent<SoundGroupVariation>();
				if (!(component == null) && component.audLocation == MasterAudio.AudioLocation.FileOnInternet)
				{
					AudioResourceOptimizer.RemoveLoadedInternetClip(component.internetFileUrl);
				}
			}
		}

		public void AddActiveAudioSourceId(int varInstanceId)
		{
			if (!ActiveAudioSourceIds.Contains(varInstanceId))
			{
				ActiveAudioSourceIds.Add(varInstanceId);
				GroupBus busForGroup = BusForGroup;
				if (busForGroup != null)
				{
					busForGroup.AddActiveAudioSourceId(varInstanceId);
				}
			}
		}

		public void RemoveActiveAudioSourceId(int varInstanceId)
		{
			ActiveAudioSourceIds.Remove(varInstanceId);
			GroupBus busForGroup = BusForGroup;
			if (busForGroup != null)
			{
				busForGroup.RemoveActiveAudioSourceId(varInstanceId);
			}
		}

		public void FireLastVariationFinishedPlay()
		{
			if (this.LastVariationFinishedPlay != null)
			{
				this.LastVariationFinishedPlay();
			}
		}

		public void SubscribeToLastVariationFinishedPlay(Action finishedCallback)
		{
			this.LastVariationFinishedPlay = null;
			LastVariationFinishedPlay += finishedCallback;
		}

		public void UnsubscribeFromLastVariationFinishedPlay()
		{
			this.LastVariationFinishedPlay = null;
		}
	}
}
