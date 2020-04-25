// This file is part of the TaiwuTools <https://github.com/vizv/TaiwuTools/>.
// Copyright (C) 2020  Taiwu Modding Community Members
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using YamlDotNet.Serialization;

namespace UnityUIKit.Core
{
    [Serializable]
    [YamlOnlySerializeSerializable]
    public abstract class ManagedGameObject : IManagedObject
    {
        private string name = null;

        [YamlSerializable]
        public string Name
        {
            set
            {
                name = value;
                if (gameObject) gameObject.name = name;
            }
            get
            {
                if (name == null) name = $"Unnammed UIKit GameObject <{GetType().FullName}>";
                return name;
            }
        }

        [YamlSerializable]
        public Dictionary<Type, ManagedComponent.ComponentAttributes> Components = new Dictionary<Type, ManagedComponent.ComponentAttributes>();
        [YamlSerializable]
        public List<ManagedGameObject> Children = new List<ManagedGameObject>();


        // IManagedObject
        private GameObject gameObject;
        public GameObject GameObject
        {
            get
            {
                if (!gameObject) Create();
                return gameObject;
            }
        }


        public T Get<T>() where T : Component => GameObject.GetComponent<T>() ?? GameObject.AddComponent<T>();
        public Component Get(Type type) => GameObject.GetComponent(type) ?? GameObject.AddComponent(type);


        public bool Created => !Destroyed;
        public bool Destroyed => !gameObject;
        public bool IsActive => GameObject.activeSelf;

        public LayoutElement LayoutElement => Get<LayoutElement>();
        public RectTransform RectTransform => Get<RectTransform>();


        public void SetParent(ManagedGameObject managedGameObject, bool worldPositionStays = false) => SetParent(managedGameObject.GameObject, worldPositionStays);
        public void SetParent(GameObject gameObject, bool worldPositionStays = false) => SetParent(gameObject.transform, worldPositionStays);
        public void SetParent(Transform transform, bool worldPositionStays = false) => GameObject.transform.SetParent(transform, worldPositionStays);


        public void SetActive(bool value) => GameObject.SetActive(value);

        public T AddComponent<T>() where T : Component => GameObject.AddComponent<T>();


        public virtual void Create(bool active = true)
        {
            if (gameObject) return;
            gameObject = new GameObject(name);
            gameObject.SetActive(active);

            foreach (var componentPair in Components)
            {
                var component = GameObject.AddComponent(componentPair.Key) as ManagedComponent;
                if (!component) continue;

                component.Apply(componentPair.Value);
            }

            foreach (var child in Children) child.SetParent(this);
        }


        public virtual void Destroy(bool destroyChild = true)
        {
            if (!gameObject) return;

            if (!destroyChild) gameObject.transform.DetachChildren();

            UnityEngine.Object.Destroy(gameObject);
            gameObject = null;
        }


        [YamlSerializable]
        [YamlMember(Alias = "MGO.Type",Order = -2)]
        public virtual string Type => this.GetType().FullName;
    }
}
