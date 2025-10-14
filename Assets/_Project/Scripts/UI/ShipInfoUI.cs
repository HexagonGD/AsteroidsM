using Asteroids.Core;
using TMPro;
using UnityEngine;

namespace Asteroids.UI
{
    public class ShipInfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _positionTMP;
        [SerializeField] private TMP_Text _rotationTMP;
        [SerializeField] private TMP_Text _chargesTMP;
        [SerializeField] private TMP_Text _reloadTMP;

        public void UpdateInfo(ShipSystem shipSystem)
        {
            _positionTMP.text = $"Position: {shipSystem.ShipUnit.Data.Position.x:0.00} {shipSystem.ShipUnit.Data.Position.y:0.00}";
            _rotationTMP.text = $"Rotation: {shipSystem.ShipUnit.Data.Rotation:0.00}";
            _chargesTMP.text = $"Charges: {shipSystem.LazerCharges}";
            _reloadTMP.text = $"Reload: {shipSystem.LazerRemainingTime:0.00}";
        }
    }
}