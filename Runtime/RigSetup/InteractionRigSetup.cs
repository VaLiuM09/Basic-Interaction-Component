﻿using System;
using System.Collections.Generic;
using System.Linq;
using VPG.Creator.Core.Utils;
using UnityEngine;

namespace VPG.Creator.BasicInteraction.RigSetup
{
    /// <summary>
    /// Will setup the interaction rig on awake. Priority is from top to bottom of the list, only rigs which full fill
    /// all RigUsabilityChecker will be initialized.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class InteractionRigSetup : MonoBehaviour
    {
        /// <summary>
        /// Info struct about one rig.
        /// </summary>
        [Serializable]
        public struct RigInfo
        {
            public string Name;
            public bool Enabled;
        }

        /// <summary>
        /// Information about possible interaction rigs, serializable.
        /// </summary>
        [SerializeField]
        public RigInfo[] PossibleInteractionRigs = new RigInfo[0];
        
        [Tooltip("Dummy Trainee object")]
        public GameObject DummyTrainee;

        /// <summary>
        /// Enforced provider will be use.
        /// </summary>
        protected static InteractionRigProvider enforcedProvider = null;
        
        private void Awake()
        {
            InteractionRigProvider rigProvider = null;
            
            if (enforcedProvider != null)
            {
                rigProvider = enforcedProvider;
            }
            else if (PossibleInteractionRigs != null)
            {
                rigProvider = FindAvailableInteractionRig();
            }

            if (rigProvider != null)
            {
                rigProvider.PreSetup();
                Vector3 position = Vector3.zero;
                Quaternion rotation = new Quaternion();

                if (DummyTrainee != null)
                {
                    position = DummyTrainee.transform.position;
                    rotation = DummyTrainee.transform.rotation;
                    DestroyImmediate(DummyTrainee);
                }

                GameObject prefab = rigProvider.GetPrefab();
                if (prefab != null)
                {
                    GameObject instance = Instantiate(rigProvider.GetPrefab());
                    instance.name = instance.name.Replace("(Clone)", "");
                    instance.transform.position = position;
                    instance.transform.rotation = rotation;
                }

                rigProvider.OnSetup();
            }
            else if (DummyTrainee != null)
            {
                DestroyImmediate(DummyTrainee);
            }
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Updates the current list of all rigs available.
        /// </summary>
        public List<InteractionRigProvider> UpdateRigList()
        {
            List<RigInfo> rigs = PossibleInteractionRigs.ToList();

            IEnumerable<Type> foundTypes = ReflectionUtils.GetConcreteImplementationsOf<InteractionRigProvider>();
            List<InteractionRigProvider> foundProvider = foundTypes.Select(type =>
                (InteractionRigProvider) ReflectionUtils.CreateInstanceOfType(type)).ToList();

            bool isFirstTime = rigs.Count == 0;
            
            foreach (InteractionRigProvider provider in foundProvider)
            {
                if (rigs.All(rigProvider => rigProvider.Name != provider.Name))
                {
                    rigs.Add(new RigInfo()
                    {
                        Name = provider.Name,
                        Enabled = true,
                    });
                }
            }
            
            // If provider get removed we have to fix the list.
            rigs.RemoveAll(info => foundProvider.Any(provider => provider.Name == info.Name) == false);

            // On initializing the list we want to move none to lowest priority.
            if (isFirstTime)
            {
                RigInfo rigInfo = rigs.Find(info => info.Name == "<None>");
                rigs.Remove(rigInfo);
                rigs.Add(rigInfo);
            }
            
            OrderFoundProvider(foundProvider);
            PossibleInteractionRigs = rigs.ToArray();
            
            return foundProvider;
        }

        private void OrderFoundProvider(List<InteractionRigProvider> foundProvider)
        {
            foundProvider = PossibleInteractionRigs.Select(info => foundProvider.Find(provider => provider.Name == info.Name)).ToList();
        }
        
        private InteractionRigProvider FindAvailableInteractionRig()
        {
            IEnumerable<InteractionRigProvider> availableRigs = ReflectionUtils.GetConcreteImplementationsOf<InteractionRigProvider>()
                .Select(type => (InteractionRigProvider) ReflectionUtils.CreateInstanceOfType(type))
                .Where(provider => provider.CanBeUsed());

            foreach (RigInfo rigInfo in PossibleInteractionRigs)
            {
                if (rigInfo.Enabled)
                {
                    InteractionRigProvider provider = availableRigs.FirstOrDefault(p => p.Name == rigInfo.Name);
                    if (provider != null)
                    {
                        return provider;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Enforces the giving Rig to be used, if possible.
        /// </summary>
        /// <param name="prefab">Prefab of the rig to be used.</param>
        public static void SetEnforcedInteractionRig(InteractionRigProvider provider)
        {
            enforcedProvider = provider;
        }
    }
}