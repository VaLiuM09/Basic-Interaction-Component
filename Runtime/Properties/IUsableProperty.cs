using System;
using VPG.Creator.Core.SceneObjects;
using VPG.Creator.Core.Properties;

namespace VPG.Creator.BasicInteraction.Properties
{
    public interface IUsableProperty : ISceneObjectProperty, ILockable
    {
        event EventHandler<EventArgs> UsageStarted;
        event EventHandler<EventArgs> UsageStopped;
        
        /// <summary>
        /// Is object currently used.
        /// </summary>
        bool IsBeingUsed { get; }
        
        /// <summary>
        /// Instantaneously simulate that the object was used.
        /// </summary>
        void FastForwardUse();
        
    }
}
