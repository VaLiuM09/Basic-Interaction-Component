using System;
using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.BasicInteraction.Properties;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Tests.Builder;
using Innoactive.Creator.Tests.Utils;
using Innoactive.CreatorEditor.BasicInteraction.CourseValidation;
using Innoactive.CreatorEditor.CourseValidation;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.CreatorEditor.Tests.CourseValidation
{
    internal class CheckGrabAndSnapCollisionTests : RuntimeTests
    {
        [Test]
        public void NoErrorWithoutAnyConditions()
        {
            IStep step = new BasicStepBuilder("step").Build();
            Assert.AreEqual(0, new GrabAndSnapCollisionValidator().Validate(step, new StepContext(step, null)).Count);
        }
        
        [Test]
        public void NoErrorWithoutCollision()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<GrabPropertyMock>();
            obj.gameObject.AddComponent<SnapPropertyMock>();
            
            IStep step = new BasicStepBuilder("step")
                .AddCondition(new GrabbedCondition("test"))
                .Build();
            
            Assert.AreEqual(0, new GrabAndSnapCollisionValidator().Validate(step, new StepContext(step, null)).Count);
        }

        [Test]
        public void CollisionIsTracked()
        {
            TrainingSceneObject snapzone = TestingUtils.CreateSceneObject("snap");
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<GrabPropertyMock>().SceneObject = obj;
            obj.gameObject.AddComponent<SnapPropertyMock>().SceneObject = obj;

            IStep step = new BasicStepBuilder("step")
                .AddCondition(new GrabbedCondition("test"))
                .AddCondition(new SnappedCondition("test", "snap"))
                .Build();
            
            Assert.AreEqual(1, new GrabAndSnapCollisionValidator().Validate(step, new StepContext(step, null)).Count);
        }

        private class SnapPropertyMock : MonoBehaviour, ISnappableProperty
        {
            public ISceneObject SceneObject { get; set; }
            
#pragma warning disable CS0067
            public event EventHandler<EventArgs> Snapped;
            public event EventHandler<EventArgs> Unsnapped;
#pragma warning restore CS0067
            
            public bool IsSnapped { get; }
            public bool LockObjectOnSnap { get; }
            public ISnapZoneProperty SnappedZone { get; set; }
            public void FastForwardSnapInto(ISnapZoneProperty snapZone)
            {
                throw new NotImplementedException();
            }
        }

        private class GrabPropertyMock : MonoBehaviour, IGrabbableProperty
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
            public event EventHandler<EventArgs> Grabbed;
            public event EventHandler<EventArgs> Ungrabbed;
#pragma warning restore CS0067
            
            public bool IsGrabbed { get; }
            public void FastForwardGrab()
            {
                throw new NotImplementedException();
            }

            public void FastForwardUngrab()
            {
                throw new NotImplementedException();
            }
        }
    }
}
