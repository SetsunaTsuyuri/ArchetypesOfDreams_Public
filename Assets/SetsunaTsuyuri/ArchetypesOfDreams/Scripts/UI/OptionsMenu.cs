using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// オプションメニュー
    /// </summary>
    public class OptionsMenu : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// 音量の変化量
        /// </summary>
        [SerializeField]
        float _volumeChangeValue = 0.1f;

        /// <summary>
        /// マスター音量ボタン
        /// </summary>
        [SerializeField]
        GameButton _masterVolume = null;

        /// <summary>
        /// BGM音量ボタン
        /// </summary>
        [SerializeField]
        GameButton _bgmVolume = null;

        /// <summary>
        /// SE音量ボタン
        /// </summary>
        [SerializeField]
        GameButton _seVolume = null;

        public override void SetUp()
        {
            void AddVolumeChangeButtonTrigger(GameButton button, AudioVolume volume)
            {
                button.AddTrriger(EventTriggerType.Move, e =>
                {
                    MoveDirection direction = button.GetMoveDirection(e);
                    switch (direction)
                    {
                        case MoveDirection.Left:
                            volume.Value -= _volumeChangeValue;
                            break;

                        case MoveDirection.Right:
                            volume.Value += _volumeChangeValue;
                            break;

                        default:
                            break;
                    }
                });
            }

            base.SetUp();

            AddVolumeChangeButtonTrigger(_masterVolume, AudioManager.MasterVolume);

            AddVolumeChangeButtonTrigger(_bgmVolume, AudioManager.BgmVolume);

            AddVolumeChangeButtonTrigger(_seVolume, AudioManager.SEVolume);

            Hide();
        }

        public override void BeCanceled()
        {
            SaveDataManager.SaveSystemData();

            Hide();

            base.BeCanceled();
        }
    }
}
