using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0,0]")]
	public partial class GameManagerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 9;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private int _hostPoints;
		public event FieldEvent<int> hostPointsChanged;
		public Interpolated<int> hostPointsInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int hostPoints
		{
			get { return _hostPoints; }
			set
			{
				// Don't do anything if the value is the same
				if (_hostPoints == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_hostPoints = value;
				hasDirtyFields = true;
			}
		}

		public void SethostPointsDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_hostPoints(ulong timestep)
		{
			if (hostPointsChanged != null) hostPointsChanged(_hostPoints, timestep);
			if (fieldAltered != null) fieldAltered("hostPoints", _hostPoints, timestep);
		}
		[ForgeGeneratedField]
		private int _clientPoints;
		public event FieldEvent<int> clientPointsChanged;
		public Interpolated<int> clientPointsInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int clientPoints
		{
			get { return _clientPoints; }
			set
			{
				// Don't do anything if the value is the same
				if (_clientPoints == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_clientPoints = value;
				hasDirtyFields = true;
			}
		}

		public void SetclientPointsDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_clientPoints(ulong timestep)
		{
			if (clientPointsChanged != null) clientPointsChanged(_clientPoints, timestep);
			if (fieldAltered != null) fieldAltered("clientPoints", _clientPoints, timestep);
		}
		[ForgeGeneratedField]
		private float _playedTime;
		public event FieldEvent<float> playedTimeChanged;
		public InterpolateFloat playedTimeInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float playedTime
		{
			get { return _playedTime; }
			set
			{
				// Don't do anything if the value is the same
				if (_playedTime == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_playedTime = value;
				hasDirtyFields = true;
			}
		}

		public void SetplayedTimeDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_playedTime(ulong timestep)
		{
			if (playedTimeChanged != null) playedTimeChanged(_playedTime, timestep);
			if (fieldAltered != null) fieldAltered("playedTime", _playedTime, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			hostPointsInterpolation.current = hostPointsInterpolation.target;
			clientPointsInterpolation.current = clientPointsInterpolation.target;
			playedTimeInterpolation.current = playedTimeInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _hostPoints);
			UnityObjectMapper.Instance.MapBytes(data, _clientPoints);
			UnityObjectMapper.Instance.MapBytes(data, _playedTime);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_hostPoints = UnityObjectMapper.Instance.Map<int>(payload);
			hostPointsInterpolation.current = _hostPoints;
			hostPointsInterpolation.target = _hostPoints;
			RunChange_hostPoints(timestep);
			_clientPoints = UnityObjectMapper.Instance.Map<int>(payload);
			clientPointsInterpolation.current = _clientPoints;
			clientPointsInterpolation.target = _clientPoints;
			RunChange_clientPoints(timestep);
			_playedTime = UnityObjectMapper.Instance.Map<float>(payload);
			playedTimeInterpolation.current = _playedTime;
			playedTimeInterpolation.target = _playedTime;
			RunChange_playedTime(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _hostPoints);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _clientPoints);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _playedTime);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (hostPointsInterpolation.Enabled)
				{
					hostPointsInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					hostPointsInterpolation.Timestep = timestep;
				}
				else
				{
					_hostPoints = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_hostPoints(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (clientPointsInterpolation.Enabled)
				{
					clientPointsInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					clientPointsInterpolation.Timestep = timestep;
				}
				else
				{
					_clientPoints = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_clientPoints(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (playedTimeInterpolation.Enabled)
				{
					playedTimeInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					playedTimeInterpolation.Timestep = timestep;
				}
				else
				{
					_playedTime = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_playedTime(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (hostPointsInterpolation.Enabled && !hostPointsInterpolation.current.UnityNear(hostPointsInterpolation.target, 0.0015f))
			{
				_hostPoints = (int)hostPointsInterpolation.Interpolate();
				//RunChange_hostPoints(hostPointsInterpolation.Timestep);
			}
			if (clientPointsInterpolation.Enabled && !clientPointsInterpolation.current.UnityNear(clientPointsInterpolation.target, 0.0015f))
			{
				_clientPoints = (int)clientPointsInterpolation.Interpolate();
				//RunChange_clientPoints(clientPointsInterpolation.Timestep);
			}
			if (playedTimeInterpolation.Enabled && !playedTimeInterpolation.current.UnityNear(playedTimeInterpolation.target, 0.0015f))
			{
				_playedTime = (float)playedTimeInterpolation.Interpolate();
				//RunChange_playedTime(playedTimeInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public GameManagerNetworkObject() : base() { Initialize(); }
		public GameManagerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public GameManagerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
