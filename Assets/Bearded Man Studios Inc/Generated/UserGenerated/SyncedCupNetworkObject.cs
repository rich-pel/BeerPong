using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0,0,0,0,0]")]
	public partial class SyncedCupNetworkObject : NetworkObject
	{
		public const int IDENTITY = 9;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0f, Enabled = false };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		[ForgeGeneratedField]
		private byte _cupIndex;
		public event FieldEvent<byte> cupIndexChanged;
		public Interpolated<byte> cupIndexInterpolation = new Interpolated<byte>() { LerpT = 0f, Enabled = false };
		public byte cupIndex
		{
			get { return _cupIndex; }
			set
			{
				// Don't do anything if the value is the same
				if (_cupIndex == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_cupIndex = value;
				hasDirtyFields = true;
			}
		}

		public void SetcupIndexDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_cupIndex(ulong timestep)
		{
			if (cupIndexChanged != null) cupIndexChanged(_cupIndex, timestep);
			if (fieldAltered != null) fieldAltered("cupIndex", _cupIndex, timestep);
		}
		[ForgeGeneratedField]
		private bool _cupIsRed;
		public event FieldEvent<bool> cupIsRedChanged;
		public Interpolated<bool> cupIsRedInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool cupIsRed
		{
			get { return _cupIsRed; }
			set
			{
				// Don't do anything if the value is the same
				if (_cupIsRed == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_cupIsRed = value;
				hasDirtyFields = true;
			}
		}

		public void SetcupIsRedDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_cupIsRed(ulong timestep)
		{
			if (cupIsRedChanged != null) cupIsRedChanged(_cupIsRed, timestep);
			if (fieldAltered != null) fieldAltered("cupIsRed", _cupIsRed, timestep);
		}
		[ForgeGeneratedField]
		private Vector3 _homePosition;
		public event FieldEvent<Vector3> homePositionChanged;
		public InterpolateVector3 homePositionInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 homePosition
		{
			get { return _homePosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_homePosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_homePosition = value;
				hasDirtyFields = true;
			}
		}

		public void SethomePositionDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_homePosition(ulong timestep)
		{
			if (homePositionChanged != null) homePositionChanged(_homePosition, timestep);
			if (fieldAltered != null) fieldAltered("homePosition", _homePosition, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _homeRotation;
		public event FieldEvent<Quaternion> homeRotationChanged;
		public InterpolateQuaternion homeRotationInterpolation = new InterpolateQuaternion() { LerpT = 0f, Enabled = false };
		public Quaternion homeRotation
		{
			get { return _homeRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_homeRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x20;
				_homeRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SethomeRotationDirty()
		{
			_dirtyFields[0] |= 0x20;
			hasDirtyFields = true;
		}

		private void RunChange_homeRotation(ulong timestep)
		{
			if (homeRotationChanged != null) homeRotationChanged(_homeRotation, timestep);
			if (fieldAltered != null) fieldAltered("homeRotation", _homeRotation, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			cupIndexInterpolation.current = cupIndexInterpolation.target;
			cupIsRedInterpolation.current = cupIsRedInterpolation.target;
			homePositionInterpolation.current = homePositionInterpolation.target;
			homeRotationInterpolation.current = homeRotationInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _cupIndex);
			UnityObjectMapper.Instance.MapBytes(data, _cupIsRed);
			UnityObjectMapper.Instance.MapBytes(data, _homePosition);
			UnityObjectMapper.Instance.MapBytes(data, _homeRotation);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_cupIndex = UnityObjectMapper.Instance.Map<byte>(payload);
			cupIndexInterpolation.current = _cupIndex;
			cupIndexInterpolation.target = _cupIndex;
			RunChange_cupIndex(timestep);
			_cupIsRed = UnityObjectMapper.Instance.Map<bool>(payload);
			cupIsRedInterpolation.current = _cupIsRed;
			cupIsRedInterpolation.target = _cupIsRed;
			RunChange_cupIsRed(timestep);
			_homePosition = UnityObjectMapper.Instance.Map<Vector3>(payload);
			homePositionInterpolation.current = _homePosition;
			homePositionInterpolation.target = _homePosition;
			RunChange_homePosition(timestep);
			_homeRotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			homeRotationInterpolation.current = _homeRotation;
			homeRotationInterpolation.target = _homeRotation;
			RunChange_homeRotation(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _cupIndex);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _cupIsRed);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _homePosition);
			if ((0x20 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _homeRotation);

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
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (cupIndexInterpolation.Enabled)
				{
					cupIndexInterpolation.target = UnityObjectMapper.Instance.Map<byte>(data);
					cupIndexInterpolation.Timestep = timestep;
				}
				else
				{
					_cupIndex = UnityObjectMapper.Instance.Map<byte>(data);
					RunChange_cupIndex(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (cupIsRedInterpolation.Enabled)
				{
					cupIsRedInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					cupIsRedInterpolation.Timestep = timestep;
				}
				else
				{
					_cupIsRed = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_cupIsRed(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (homePositionInterpolation.Enabled)
				{
					homePositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					homePositionInterpolation.Timestep = timestep;
				}
				else
				{
					_homePosition = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_homePosition(timestep);
				}
			}
			if ((0x20 & readDirtyFlags[0]) != 0)
			{
				if (homeRotationInterpolation.Enabled)
				{
					homeRotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					homeRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_homeRotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_homeRotation(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (cupIndexInterpolation.Enabled && !cupIndexInterpolation.current.UnityNear(cupIndexInterpolation.target, 0.0015f))
			{
				_cupIndex = (byte)cupIndexInterpolation.Interpolate();
				//RunChange_cupIndex(cupIndexInterpolation.Timestep);
			}
			if (cupIsRedInterpolation.Enabled && !cupIsRedInterpolation.current.UnityNear(cupIsRedInterpolation.target, 0.0015f))
			{
				_cupIsRed = (bool)cupIsRedInterpolation.Interpolate();
				//RunChange_cupIsRed(cupIsRedInterpolation.Timestep);
			}
			if (homePositionInterpolation.Enabled && !homePositionInterpolation.current.UnityNear(homePositionInterpolation.target, 0.0015f))
			{
				_homePosition = (Vector3)homePositionInterpolation.Interpolate();
				//RunChange_homePosition(homePositionInterpolation.Timestep);
			}
			if (homeRotationInterpolation.Enabled && !homeRotationInterpolation.current.UnityNear(homeRotationInterpolation.target, 0.0015f))
			{
				_homeRotation = (Quaternion)homeRotationInterpolation.Interpolate();
				//RunChange_homeRotation(homeRotationInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public SyncedCupNetworkObject() : base() { Initialize(); }
		public SyncedCupNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public SyncedCupNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
