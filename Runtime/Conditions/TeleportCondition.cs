using System.Runtime.Serialization;
using VPG.Creator.Core;
using VPG.Creator.Core.Utils;
using VPG.Creator.Core.Validation;
using VPG.Creator.Core.Attributes;
using VPG.Creator.Core.Conditions;
using VPG.Creator.Core.SceneObjects;
using VPG.Creator.BasicInteraction.Properties;

namespace VPG.Creator.BasicInteraction.Conditions
{
    /// <summary>
    /// Condition which is completed when a teleportation action was executed into the referenced <see cref="ITeleportationProperty"/>.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-conditions.html#teleport")]
    public class TeleportCondition : Condition<TeleportCondition.EntityData>
    {
        [DisplayName("Teleport")]
        [DataContract(IsReference = true)]
        public class EntityData : IConditionData
        {
            [DataMember]
            [DisplayName("Teleportation Point")]
#if CREATOR_PRO
            [CheckForCollider]
#endif
            public ScenePropertyReference<ITeleportationProperty> TeleportationPoint { get; set; }

            /// <inheritdoc />
            public bool IsCompleted { get; set; }

            /// <inheritdoc />
            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }
        }

        public TeleportCondition() : this( "")
        {
        }

        public TeleportCondition(ITeleportationProperty teleportationPoint, string name = null) : this(TrainingReferenceUtils.GetNameFrom(teleportationPoint), name)
        {
        }

        public TeleportCondition(string teleportationPoint, string name = "Teleport")
        {
            Data.TeleportationPoint = new ScenePropertyReference<ITeleportationProperty>(teleportationPoint);
            Data.Name = name;
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }
            
            /// <inheritdoc />
            public override void Start()
            {
                base.Start();
                Data.TeleportationPoint.Value.Initialize();
            }

            /// <inheritdoc />
            protected override bool CheckIfCompleted()
            {
                return Data.TeleportationPoint.Value.WasUsedToTeleport;
            }
        }

        private class EntityAutocompleter : Autocompleter<EntityData>
        {
            public EntityAutocompleter(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Complete()
            {
                Data.TeleportationPoint.Value.FastForwardTeleport();
            }
        }

        /// <inheritdoc />
        public override IProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }

        /// <inheritdoc />
        protected override IAutocompleter GetAutocompleter()
        {
            return new EntityAutocompleter(Data);
        }
    }
}
