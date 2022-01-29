using System;
using System.Collections.Generic;
using UnityEngine;

namespace DavidFDev.Audio
{
    [CreateAssetMenu(menuName = "DavidFDev/Audio/Audio Database")]
    internal sealed class AudioDatabase : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private List<AudioAsset> _audioAssets = new List<AudioAsset>();

        internal HashSet<string> _identifiers = new HashSet<string>();

        #endregion

        #region Properties

        public IReadOnlyList<AudioAsset> AudioAssets => _audioAssets;

        #endregion

        #region Methods

        private void OnValidate()
        {
            _identifiers.Clear();

            foreach (AudioAsset audioAsset in _audioAssets)
            {
                audioAsset.OnValidate();
            }
        }

        #endregion

        #region Nested types

        [Serializable]
        public sealed class AudioAsset
        {
            #region Properties

            [field: SerializeField]
            public string Identifier { get; private set; }

            [field: SerializeField]
            public string Description { get; private set; }

            [field: SerializeField]
            public UnityEngine.Object Asset { get; private set; }

            #endregion

            #region Methods

            internal void OnValidate()
            {
                if (Asset == null)
                {
                    return;
                }

                // Check that the type is a valid audio clip or sound effect asset
                Type objType = Asset.GetType();
                if (!(objType.IsEquivalentTo(typeof(AudioClip)) || objType.IsEquivalentTo(typeof(SoundEffect))))
                {
                    Debug.LogError($"{nameof(Asset)} must be an {nameof(AudioClip)} or {nameof(SoundEffect)}.");
                    Asset = null;
                    return;
                }

                // Set the identifier if not set
                if (string.IsNullOrEmpty(Identifier))
                {
                    Identifier = Asset.name;
                }
            }

            #endregion
        }

        #endregion
    }
}