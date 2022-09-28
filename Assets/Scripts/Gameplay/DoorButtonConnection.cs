using System;
using UnityEngine;

namespace HexGame.Gameplay
{
    [RequireComponent(typeof(Door))]
    public class DoorButtonConnection : ButtonConnection
    {
        private Door _door;

        private void Awake()
        {
            _door = GetComponent<Door>();
        }

        public override void Activate() => _door.Open();

        public override void Deactivate() => _door.Close();
    }
}