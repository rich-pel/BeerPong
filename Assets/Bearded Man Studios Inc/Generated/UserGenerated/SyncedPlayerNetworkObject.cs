using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0.15,0.15,0.15,0.15]")]
	public partial class SyncedPlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 10;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _headPosition;
		public event FieldEvent<Vector3> headPositionChanged;
		public InterpolateVector3 headPositionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 headPosition
		{
			get { return _headPosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_headPosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_headPosition = value;
				hasDirtyFields = true;
			}
		}

		public void SetheadPositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_headPosition(ulong timestep)
		{
			if (headPositionChanged != null) headPositionChanged(_headPosition, timestep);
			if (fieldAltered != null) fieldAltered("headPosition", _headPosition, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _headRotation;
		public event FieldEvent<Quaternion> headRotationChanged;
		public InterpolateQuaternion headRotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion headRotation
		{
			get { return _headRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_headRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_headRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetheadRotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_headRotation(ulong timestep)
		{
			if (headRotationChanged != null) headRotationChanged(_headRotation, timestep);
			if (fieldAltered != null) fieldAltered("headRotation", _headRotation, timestep);
		}
		[ForgeGeneratedField]
		private Vector3 _leftHandPosition;
		public event FieldEvent<Vector3> leftHandPositionChanged;
		public InterpolateVector3 leftHandPositionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 leftHandPosition
		{
			get { return _leftHandPosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_leftHandPosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_leftHandPosition = value;
				hasDirtyFields = true;
			}
		}

		public void SetleftHandPositionDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_leftHandPosition(ulong timestep)
		{
			if (leftHandPositionChanged != null) leftHandPositionChanged(_leftHandPosition, timestep);
			if (fieldAltered != null) fieldAltered("leftHandPosition", _leftHandPosition, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _leftHandRotation;
		public event FieldEvent<Quaternion> leftHandRotationChanged;
		public InterpolateQuaternion leftHandRotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion leftHandRotation
		{
			get { return _leftHandRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_leftHandRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_leftHandRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetleftHandRotationDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_leftHandRotation(ulong timestep)
		{
			if (leftHandRotationChanged != null) leftHandRotationChanged(_leftHandRotation, timestep);
			if (fieldAltered != null) fieldAltered("leftHandRotation", _leftHandRotation, timestep);
		}
		[ForgeGeneratedField]
		private Vector3 _rightHandPosition;
		public event FieldEvent<Vector3> rightHandPositionChanged;
		public InterpolateVector3 rightHandPositionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 rightHandPosition
		{
			get { return _rightHandPosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_rightHandPosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_rightHandPosition = value;
				hasDirtyFields = true;
			}
		}

		public void SetrightHandPositionDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_rightHandPosition(ulong timestep)
		{
			if (rightHandPositionChanged != null) rightHandPositionChanged(_rightHandPosition, timestep);
			if (fieldAltered != null) fieldAltered("rightHandPosition", _rightHandPosition, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rightHandRotation;
		public event FieldEvent<Quaternion> rightHandRotationChanged;
		public InterpolateQuaternion rightHandRotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rightHandRotation
		{
			get { return _rightHandRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rightHandRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x20;
				_rightHandRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrightHandRotationDirty()
		{
			_dirtyFields[0] |= 0x20;
			hasDirtyFields = true;
		}

		private void RunChange_rightHandRotation(ulong timestep)
		{
			if (rightHandRotationChanged != null) rightHandRotationChanged(_rightHandRotation, timestep);
			if (fieldAltered != null) fieldAltered("rightHandRotation", _rightHandRotation, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			headPositionInterpolation.current = headPositionInterpolation.target;
			headRotationInterpolation.current = headRotationInterpolation.target;
			leftHandPositionInterpolation.current = leftHandPositionInterpolation.target;
			leftHandRotationInterpolation.current = leftHandRotationInterpolation.target;
			rightHandPositionInterpolation.current = rightHandPositionInterpolation.target;
			rightHandRotationInterpolation.current = rightHandRotationInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _headPosition);
			UnityObjectMapper.Instance.MapBytes(data, _headRotation);
			UnityObjectMapper.Instance.MapBytes(data, _leftHandPosition);
			UnityObjectMapper.Instance.MapBytes(data, _leftHandRotation);
			UnityObjectMapper.Instance.MapBytes(data, _rightHandPosition);
			UnityObjectMapper.Instance.MapBytes(data, _rightHandRotation);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_headPosition = UnityObjectMapper.Instance.Map<Vector3>(payload);
			headPositionInterpolation.current = _headPosition;
			headPositionInterpolation.target = _headPosition;
			RunChange_headPosition(timestep);
			_headRotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			headRotationInterpolation.current = _headRotation;
			headRotationInterpolation.target = _headRotation;
			RunChange_headRotation(timestep);
			_leftHandPosition = UnityObjectMapper.Instance.Map<Vector3>(payload);
			leftHandPositionInterpolation.current = _leftHandPosition;
			leftHandPositionInterpolation.target = _leftHandPosition;
			RunChange_leftHandPosition(timestep);
			_leftHandRotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			leftHandRotationInterpolation.current = _leftHandRotation;
			leftHandRotationInterpolation.target = _leftHandRotation;
			RunChange_leftHandRotation(timestep);
			_rightHandPosition = UnityObjectMapper.Instance.Map<Vector3>(payload);
			rightHandPositionInterpolation.current = _rightHandPosition;
			rightHandPositionInterpolation.target = _rightHandPosition;
			RunChange_rightHandPosition(timestep);
			_rightHandRotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rightHandRotationInterpolation.current = _rightHandRotation;
			rightHandRotationInterpolation.target = _rightHandRotation;
			RunChange_rightHandRotation(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _headPosition);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _headRotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _leftHandPosition);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _leftHandRotation);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rightHandPosition);
			if ((0x20 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rightHandRotation);

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
				if (headPositionInterpolation.Enabled)
				{
					headPositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					headPositionInterpolation.Timestep = timestep;
				}
				else
				{
					_headPosition = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_headPosition(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (headRotationInterpolation.Enabled)
				{
					headRotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					headRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_headRotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_headRotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (leftHandPositionInterpolation.Enabled)
				{
					leftHandPositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					leftHandPositionInterpolation.Timestep = timestep;
				}
				else
				{
					_leftHandPosition = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_leftHandPosition(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (leftHandRotationInterpolation.Enabled)
				{
					leftHandRotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					leftHandRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_leftHandRotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_leftHandRotation(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (rightHandPositionInterpolation.Enabled)
				{
					rightHandPositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					rightHandPositionInterpolation.Timestep = timestep;
				}
				else
				{
					_rightHandPosition = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_rightHandPosition(timestep);
				}
			}
			if ((0x20 & readDirtyFlags[0]) != 0)
			{
				if (rightHandRotationInterpolation.Enabled)
				{
					rightHandRotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rightHandRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rightHandRotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rightHandRotation(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (headPositionInterpolation.Enabled && !headPositionInterpolation.current.UnityNear(headPositionInterpolation.target, 0.0015f))
			{
				_headPosition = (Vector3)headPositionInterpolation.Interpolate();
				//RunChange_headPosition(headPositionInterpolation.Timestep);
			}
			if (headRotationInterpolation.Enabled && !headRotationInterpolation.current.UnityNear(headRotationInterpolation.target, 0.0015f))
			{
				_headRotation = (Quaternion)headRotationInterpolation.Interpolate();
				//RunChange_headRotation(headRotationInterpolation.Timestep);
			}
			if (leftHandPositionInterpolation.Enabled && !leftHandPositionInterpolation.current.UnityNear(leftHandPositionInterpolation.target, 0.0015f))
			{
				_leftHandPosition = (Vector3)leftHandPositionInterpolation.Interpolate();
				//RunChange_leftHandPosition(leftHandPositionInterpolation.Timestep);
			}
			if (leftHandRotationInterpolation.Enabled && !leftHandRotationInterpolation.current.UnityNear(leftHandRotationInterpolation.target, 0.0015f))
			{
				_leftHandRotation = (Quaternion)leftHandRotationInterpolation.Interpolate();
				//RunChange_leftHandRotation(leftHandRotationInterpolation.Timestep);
			}
			if (rightHandPositionInterpolation.Enabled && !rightHandPositionInterpolation.current.UnityNear(rightHandPositionInterpolation.target, 0.0015f))
			{
				_rightHandPosition = (Vector3)rightHandPositionInterpolation.Interpolate();
				//RunChange_rightHandPosition(rightHandPositionInterpolation.Timestep);
			}
			if (rightHandRotationInterpolation.Enabled && !rightHandRotationInterpolation.current.UnityNear(rightHandRotationInterpolation.target, 0.0015f))
			{
				_rightHandRotation = (Quaternion)rightHandRotationInterpolation.Interpolate();
				//RunChange_rightHandRotation(rightHandRotationInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public SyncedPlayerNetworkObject() : base() { Initialize(); }
		public SyncedPlayerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public SyncedPlayerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
