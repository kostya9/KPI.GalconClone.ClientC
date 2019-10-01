using strange.extensions.command.impl;
using UnityEngine;

namespace DefaultNamespace
{
    public class StartGameCommand : Command
    {
        public override void Execute()
        {
            Debug.Log("HEY!");
        }
    }
}