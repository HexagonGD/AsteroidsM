using Asteroids.Core;
using Asteroids.Core.Weapon.Implementation;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids.UI
{
    public class ShipInfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _positionTMP;
        [SerializeField] private TMP_Text _rotationTMP;
        [SerializeField] private TMP_Text _chargesTMP;
        [SerializeField] private TMP_Text _reloadTMP;

        private Unit _ship;
        private LazerWeapon _lazerWeapon;

        [Inject]
        public void Construct(Unit ship, LazerWeapon lazerWeapon)
        {
            _ship = ship;
            _lazerWeapon = lazerWeapon;
        }

        private void Update()
        {
            _positionTMP.text = $"Position: {_ship.Data.Position.x:0.00} {_ship.Data.Position.y:0.00}";
            _rotationTMP.text = $"Rotation: {_ship.Data.Rotation:0.00}";
            _chargesTMP.text = $"Charges: {_lazerWeapon.Charges}";
            _reloadTMP.text = $"Reload: {_lazerWeapon.RemainingTimeForCharge:0.00}";
        }
    }
}