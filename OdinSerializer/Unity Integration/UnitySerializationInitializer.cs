﻿//-----------------------------------------------------------------------
// <copyright file="UnitySerializationInitializer.cs" company="Sirenix IVS">
// Copyright (c) 2018 Sirenix IVS
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

namespace Jirdus.Internal.OdinSerializer
{
    using UnityEngine;

    /// <summary>
    /// Utility class which initializes the Sirenix serialization system to be compatible with Unity.
    /// </summary>
    public static class UnitySerializationInitializer
    {
        private static readonly object LOCK = new object();
        private static bool initialized = false;

        /// <summary>
        /// Initializes the Sirenix serialization system to be compatible with Unity.
        /// </summary>
        private static void Initialize()
        {
            if (!initialized)
            {
                lock (LOCK)
                {
                    if (!initialized)
                    {
                        // Ensure that the config instance is loaded before deserialization of anything occurs.
                        // If we try to load it during deserialization, Unity will throw exceptions, as a lot of
                        // the Unity API is disallowed during serialization and deserialization.
                        GlobalSerializationConfig.LoadInstanceIfAssetExists();
                        initialized = true;
                    }
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeRuntime()
        {
            Initialize();
        }

#if UNITY_EDITOR

        [UnityEditor.InitializeOnLoadMethod]
#endif
        private static void InitializeEditor()
        {
            Initialize();
        }
    }
}