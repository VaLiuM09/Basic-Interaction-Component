using System;
using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.BasicInteraction.Properties;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Tests.Builder;
using Innoactive.Creator.Tests.Utils;
using Innoactive.CreatorEditor.BasicInteraction.CourseValidation;
using Innoactive.CreatorEditor.CourseValidation;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.CreatorEditor.Tests.CourseValidation
{
    internal class SnapzoneHighlightedTests : RuntimeTests
    {
        [Test]
        public void NoCollisionWithoutHighlight()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<SnapZonePropertyMock>().SceneObject = obj;

            IStep step = new BasicStepBuilder("step")
                .AddCondition(new SnappedCondition("test", "snap"))
                .Build();
            
            Assert.AreEqual(0, new SnapzoneHighlightedValidator().Validate(step, new StepContext(step, null)).Count);
        }

        
        [Test]
        public void NoCollisionWithOnlyHighlight()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<SnapZonePropertyMock>().SceneObject = obj;

            IStep step = new BasicStepBuilder("step")
                .AddBehavior(new HighlightObjectBehavior("snap", Color.cyan))
                .Build();
            
            Assert.AreEqual(0, new SnapzoneHighlightedValidator().Validate(step, new StepContext(step, null)).Count);
        }

        
        [Test]
        public void NoCollisionTargetingDifferent()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<SnapZonePropertyMock>().SceneObject = obj;

            IStep step = new BasicStepBuilder("step")
                .AddBehavior(new HighlightObjectBehavior("snap", Color.cyan))
                .AddCondition(new SnappedCondition("test", "snap"))
                .Build();
            
            Assert.AreEqual(0, new SnapzoneHighlightedValidator().Validate(step, new StepContext(step, null)).Count);
        }
        
        [Test]
        public void CollisionOnSameObject()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<SnapZonePropertyMock>().SceneObject = obj;
            obj.gameObject.AddComponent<HighlightPropertyMock>().SceneObject = obj;

            IStep step = new BasicStepBuilder("step")
                .AddBehavior(new HighlightObjectBehavior("test", Color.cyan))
                .AddCondition(new SnappedCondition("nothing", "test"))
                .Build();
            
            Assert.AreEqual(1, new SnapzoneHighlightedValidator().Validate(step, new StepContext(step, null)).Count);
        }


        private class HighlightPropertyMock : MonoBehaviour, IHighlightProperty
        {
            public ISceneObject SceneObject { get; set; }
            
#pragma warning disable CS0067
            public event EventHandler<EventArgs> Highlighted;
            public event EventHandler<EventArgs> Unhighlighted;
#pragma warning restore CS0067
            
            public bool IsHighlighted { get; }
            public void Highlight(Color highlightColor)
            {
                throw new NotImplementedException();
            }

            public void Unhighlight()
            {
                throw new NotImplementedException();
            }
        }
        
        private class SnapZonePropertyMock : MonoBehaviour, ISnapZoneProperty
        {
            public ISceneObject SceneObject { get; set; }
            
#pragma warning disable CS0067
            public event EventHandler<LockStateChangedEventArgs> Locked;
            public event EventHandler<LockStateChangedEventArgs> Unlocked;
#pragma warning restore CS0067
            
            public bool IsLocked { get; }
            public void SetLocked(bool lockState)
            {
                throw new NotImplementedException();
            }

#pragma warning disable CS0067
            public event EventHandler<EventArgs> ObjectSnapped;
            public event EventHandler<EventArgs> ObjectUnsnapped;
#pragma warning restore CS0067
            
            public bool IsObjectSnapped { get; }
            public ISnappableProperty SnappedObject { get; set; }
            public GameObject SnapZoneObject { get; }
            public void Configure(IMode mode)
            {
                throw new NotImplementedException();
            }
        }
    }
}
